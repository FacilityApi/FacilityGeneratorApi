#tool "nuget:?package=fsdgencsharp"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionFileName = "FacilityGeneratorApi.sln";

void CodeGen(bool verify)
{
	ExecuteProcess($@"cake\fsdgencsharp\tools\fsdgencsharp.exe",
		@"fsd\FacilityGeneratorApi.fsd src\Facility.GeneratorApi.WebApi\Models --clean --namespace Facility.GeneratorApi.WebApi.Models" + (verify ? " --verify" : ""));
}

Task("CodeGen")
	.Does(() => CodeGen(verify: false));

Task("Clean")
	.Does(() =>
	{
		CleanDirectories($"src/**/bin");
		CleanDirectories($"src/**/obj");
	});

Task("VerifyCodeGen")
	.IsDependentOn("Clean")
	.Does(() => CodeGen(verify: true));

Task("Build")
	.IsDependentOn("VerifyCodeGen")
	.Does(() =>
  {
    DotNetCoreRestore("src/Facility.GeneratorApi.WebApi");
    DotNetCoreBuild("src/Facility.GeneratorApi.WebApi", new DotNetCoreBuildSettings { Configuration = configuration });
  });

Task("Default")
	.IsDependentOn("Build");

void ExecuteProcess(string exePath, string arguments)
{
	int exitCode = StartProcess(exePath, arguments);
	if (exitCode != 0)
		throw new InvalidOperationException($"{exePath} failed with exit code {exitCode}.");
}

RunTarget(target);
