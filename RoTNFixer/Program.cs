using System;
using System.IO;

class Program
{
    static void Main()
    {
        string userName = Environment.UserName;
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "curseforge\\minecraft\\Instances");
        Console.WriteLine($"Directories in {path}:");

        try
        {
            ListDirectories(path);

            Console.Write("Enter the number of the directory to execute code (or 0 to exit): ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int selectedOption) && selectedOption >= 1 && selectedOption <= Directory.GetDirectories(path).Length)
            {
                ExecuteCode(Directory.GetDirectories(path)[selectedOption - 1]);
            }
            else
            {
                Console.WriteLine("Invalid option or directory does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void ListDirectories(string path)
    {
        if (Directory.Exists(path))
        {
            string[] directories = Directory.GetDirectories(path);

            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {directories[i]}");
            }
        }
        else
        {
            Console.WriteLine("The specified directory does not exist.");
        }
    }

    static void ExecuteCode(string selectedDirectory)
    {
        string copydir = Path.Combine(selectedDirectory, "mods");

        Console.WriteLine($"Executing code for directory: {selectedDirectory}");

        string resourcePacksPath = Path.Combine(selectedDirectory, "resourcepacks", "Official ROTN tweaks");

        if (Directory.Exists(resourcePacksPath))
        {
            Console.WriteLine("Valid ROTN modpack: 'Official ROTN tweaks' exists in /resourcepacks.");
            CopyFilesFromModsDirectory(copydir);
        }
        else
        {
            Console.WriteLine("Not a valid ROTN modpack: 'Official ROTN tweaks' directory not found in /resourcepacks.");
            return; 
        }

        string modDirectorPath = Path.Combine(selectedDirectory, "config", "mod-director");

        Console.WriteLine($"Listing files in {modDirectorPath}:");

        try
        {
            ListFilesAndDelete(modDirectorPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void ListFilesAndDelete(string path)
    {
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);

            if (files.Length == 0)
            {
                Console.WriteLine("No files found in the directory.");
            }
            else
            {
                foreach (var file in files)
                {
                    Console.WriteLine($"Deleting file: {Path.GetFileName(file)}");
                    File.Delete(file);
                }

                Console.WriteLine("All files deleted successfully.");
            }
        }
        else
        {
            Console.WriteLine($"The specified directory '{path}' does not exist.");
        }
    }

    static void CopyFilesFromModsDirectory(string destinationPath)
    {
        string modsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods");
        string destinationModsPath = Path.Combine(destinationPath, "mods");

        if (Directory.Exists(modsDirectory))
        {
            if (!Directory.Exists(destinationModsPath))
            {
                Directory.CreateDirectory(destinationModsPath);
            }

            string[] files = Directory.GetFiles(modsDirectory);

            if (files.Length == 0)
            {
                Console.WriteLine("No files found in the 'Mods' directory.");
            }
            else
            {
                foreach (var file in files)
                {
                    string destinationFile = Path.Combine(destinationModsPath, Path.GetFileName(file));
                    Console.WriteLine($"Copying file: {Path.GetFileName(file)} to {destinationModsPath}");
                    File.Copy(file, destinationFile, true); // The third parameter (true) allows overwriting existing files.
                }

                Console.WriteLine("All files copied successfully.");
            }
        }
        else
        {
            Console.WriteLine($"The 'Mods' directory in the application directory does not exist.");
        }
    }
}


