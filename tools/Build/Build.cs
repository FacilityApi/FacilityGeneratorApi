return BuildRunner.Execute(args, build =>
{
	var gitLogin = new GitLoginInfo("FacilityApiBot", Environment.GetEnvironmentVariable("BUILD_BOT_PASSWORD") ?? "");

	var dotNetBuildSettings = new DotNetBuildSettings
	{
		NuGetApiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY"),
		DocsSettings = new DotNetDocsSettings
		{
			GitLogin = gitLogin,
			GitAuthor = new GitAuthorInfo("FacilityApiBot", "facilityapi@gmail.com"),
			SourceCodeUrl = "https://github.com/FacilityApi/FacilityGeneratorApi/tree/master/src",
		},
		PackageSettings = new DotNetPackageSettings
		{
			GitLogin = gitLogin,
			PushTagOnPublish = x => $"nuget.{x.Version}",
		},
	};

	build.AddDotNetTargets(dotNetBuildSettings);

	build.Target("codegen")
		.DependsOn("build")
		.Describe("Generates code from the FSD")
		.Does(() => CodeGen(verify: false));

	build.Target("verify-codegen")
		.DependsOn("build")
		.Describe("Ensures the generated code is up-to-date")
		.Does(() => CodeGen(verify: true));

	build.Target("test")
		.DependsOn("verify-codegen");

	void CodeGen(bool verify)
	{
		var fsdFilePath = Path.Combine("fsd", "FacilityGeneratorApi.fsd");
		var verifyOption = verify ? "--verify" : null;
		var slash = Path.DirectorySeparatorChar;

		RunDotNetTool("fsdgencsharp", fsdFilePath, Path.Combine("src", "Facility.GeneratorApi") + slash, "--clean", "--nullable", "--newline", "lf", verifyOption);
		RunDotNetTool("fsdgenmd", fsdFilePath, Path.Combine("docs", "api") + slash, "--clean", "--newline", "lf", verifyOption);
		RunDotNetTool("fsdgenjs", fsdFilePath, Path.Combine("ts", "src") + slash, "--typescript", "--newline", "lf", verifyOption);
	}

	build.Target("build-npm")
		.Describe("Builds the npm package.")
		.Does(() =>
		{
			RunNpmFrom("./ts", "install");
			RunNpmFrom("./ts", "run", "build");
		});

	build.Target("publish-npm")
		.DependsOn("build-npm")
		.Describe("Publishes the npm package.")
		.Does(() =>
		{
			var token = Environment.GetEnvironmentVariable("NPM_ACCESS_TOKEN");
			if (token is null)
				throw new BuildException("Missing NPM_ACCESS_TOKEN.");
			File.WriteAllText("./ts/.npmrc", $"//registry.npmjs.org/:_authToken={token}");

			RunNpmFrom("./ts", "publish");
		});

	build.Target("build-docker")
		.Describe("Builds the docker container image.")
		.Does(() =>
		{
			RunApp("docker", "build", ".", "-t", "facilityapi/facility-generator-api");
		});

	void RunNpmFrom(string directory, params string[] args) =>
		RunApp("npm",
			new AppRunnerSettings
			{
				Arguments = args,
				WorkingDirectory = directory,
				UseCmdOnWindows = true,
				IsExitCodeSuccess = x => args[0] == "publish" ? x is 0 or 1 : x is 0,
			});
});
