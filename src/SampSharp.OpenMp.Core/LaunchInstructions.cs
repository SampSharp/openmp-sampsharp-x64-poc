using System.Diagnostics;

namespace SampSharp.OpenMp.Core;

internal static class LaunchInstructions
{
    public static void Write()
    {
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("SampSharp");
        Console.WriteLine("-------------------------------------");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("ERROR: This SampSharp gamemode must be run using an open.mp server.");
        Console.ResetColor();
        Console.WriteLine("See <<TODO: Documentation URL>> for more information.");
        Console.WriteLine();

        if (IsRunningInVisualStudio())
        {
            var dir = GetProjectDir();
            if (dir != null)
            {
                Console.WriteLine("It appears you are running this application in Visual Studio. Would you like SampSharp to update your launchSettings.json with the following configuration?");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("""
                                  -------------------------------------
                                  {
                                    "profiles": {
                                      "open.mp": {
                                        "commandName": "Executable",
                                        "executablePath": "C:\path\to\server\omp-server.exe",
                                        "workingDirectory": "C:\path\to\server\",
                                        "commandLineArgs": "-c sampsharp.directory=$(TargetDir) -c sampsharp.assembly=\"$(TargetName)\""
                                      }
                                    }
                                  }
                                  -------------------------------------
                                  """);
                Console.WriteLine();
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING: This will replace any existing launch profiles for your project.");
                Console.ResetColor();

                if (!PromptYesNo())
                {
                    return;
                }
                
                var props = dir.CreateSubdirectory("Properties");

                var serverDir = PromptServerDirectory();
                
                var launchSettingsPath = Path.Combine(props.FullName, "launchSettings.json");

                serverDir = serverDir.Replace(@"\", @"\\");

                File.WriteAllText(launchSettingsPath, 
                    $$"""
                    {
                      "profiles": {
                        "open.mp": {
                          "commandName": "Executable",
                          "executablePath": "{{serverDir}}omp-server.exe",
                          "workingDirectory": "{{serverDir}}",
                          "commandLineArgs": "-c sampsharp.directory=$(TargetDir) -c sampsharp.assembly=\"$(TargetName)\""
                        }
                      }
                    }
                    """);

                Console.WriteLine($"File written to {launchSettingsPath}");
                Console.WriteLine();
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You will find the 'open.mp' launch option in the dropdown next to the 'Start' button in Visual Studio.");
                Console.ResetColor();
            }
        }
    }

    private static bool PromptYesNo()
    {
        Console.Write("Press (y)es / (n)o: ");
        while (true)
        {
            var key = Console.ReadKey();
            
            switch (key.Key)
            {
                case ConsoleKey.N:
                    Console.WriteLine();
                    return false;
                case ConsoleKey.Y:
                    Console.WriteLine();
                    return true;
            }
        }

    }

    private static string PromptServerDirectory()
    {
        while (true)
        {
            Console.Write("Enter the path to your open.mp server directory: ");

            var dir = Console.ReadLine();
            if(!Directory.Exists(dir))
            {
                Console.WriteLine("Directory not found.");
                continue;
            }

            var exe = Path.Combine(dir, "omp-server.exe");

            if (!File.Exists(exe))
            {
                Console.WriteLine("Invalid directory.");
            }

            if (!dir.EndsWith('/') && !dir.EndsWith('\\'))
            {
                dir += Path.DirectorySeparatorChar;
            }
            return dir;
        }
    }

    private static bool IsRunningInVisualStudio()
    {
        return Debugger.IsAttached && Environment.GetEnvironmentVariable("VisualStudioVersion") != null;
    }

    private static DirectoryInfo? GetProjectDir()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        for (var i = 0; i < 5; i++)
        {
            var csproj = dir.GetFiles("*.csproj");

            if (csproj.Length == 0)
            {
                dir = dir.Parent;
                if (dir == null)
                {
                    break;
                }

                continue;
            }

            return dir;
        }

        return null;
    }
}