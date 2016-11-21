#tool "nuget:https://www.nuget.org/api/v2/?package=fsdgencsharp"
#tool "nuget:https://www.nuget.org/api/v2/?package=fsdgenjs"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionFileName = "FacilityGeneratorApi.sln";

void CodeGen(bool verify)
{
	string verifyOption = verify ? "--verify" : "";
	ExecuteProcess(@"cake\fsdgencsharp\tools\fsdgencsharp.exe",
		$@"fsd\FacilityGeneratorApi.fsd src\Facility.GeneratorApi\ --clean {verifyOption}");
	ExecuteProcess(@"cake\fsdgenjs\tools\fsdgenjs.exe",
		$@"fsd\FacilityGeneratorApi.fsd ts\src\ --typescript {verifyOption}");
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
		DotNetCoreRestore("src/Facility.GeneratorApi");
		DotNetCoreRestore("src/Facility.GeneratorApi.Services");
		DotNetCoreRestore("src/Facility.GeneratorApi.WebApi");

		var buildSettings = new DotNetCoreBuildSettings { Configuration = configuration };
		DotNetCoreBuild("src/Facility.GeneratorApi", buildSettings);
		DotNetCoreBuild("src/Facility.GeneratorApi.Services", buildSettings);
		DotNetCoreBuild("src/Facility.GeneratorApi.WebApi", buildSettings);
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
