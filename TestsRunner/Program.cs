using System.Diagnostics;

namespace TestsRunner
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Starting test runner...");

            var projectPath = Path.GetFullPath("../TestsDuo2/TestsDuo2.csproj");
            if (!File.Exists(projectPath))
            {
                Console.WriteLine($"Error: Project file not found at {projectPath}");
                return 1;
            }

            // Clean output directory first
            Console.WriteLine("Cleaning output directory...");
            RunProcess("dotnet", $"clean \"{projectPath}\"");

            // Restore packages
            Console.WriteLine("Restoring packages...");
            RunProcess("dotnet", $"restore \"{projectPath}\"");

            // Build with specific options to avoid WindowsAppSDK issues
            Console.WriteLine("Building test project...");
            var buildResult = RunProcess("dotnet", $"build \"{projectPath}\" -p:EnableMsixTooling=false -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=false -p:DisableMsixProjectCapability=true");
            
            if (buildResult != 0)
            {
                Console.WriteLine("Build failed! Cannot run tests.");
                return buildResult;
            }

            // Run tests
            Console.WriteLine("Running tests...");
            var testResult = RunProcess("dotnet", $"test \"{projectPath}\" --no-build");
            
            return testResult;
        }

        static int RunProcess(string command, string arguments)
        {
            using var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            
            process.OutputDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
            
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            
            return process.ExitCode;
        }
    }
}
