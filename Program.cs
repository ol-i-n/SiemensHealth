using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DirectoryProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            // načítanie configu
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // získať cestu z configu
            string? relativePath = config["DirectorySettings:BaseDirectory"];

            // kontrola, či je cesta null
            if (string.IsNullOrEmpty(relativePath))
            {
                Console.WriteLine("Chyba: Chýba BaseDirectory v konfigurácii.");
                relativePath = "defaultDirectory"; // len tak
            }

            // zlož si celú cestu
            string basePath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(basePath, relativePath);

            Console.WriteLine("Full Path: " + fullPath);

            // skontrolovať, či cesta existuje
            if (Directory.Exists(fullPath))
            {
                // načítať info o adresári
                var directoryInfo = GetDirectoryInfo(fullPath);

                // vypísať unikátne prípony
                PrintUniqueExtensions(directoryInfo);

                // serializácia do JSON
                string json = JsonConvert.SerializeObject(directoryInfo, Formatting.Indented);
                File.WriteAllText("directoryInfo.json", json);

                // deserializácia z JSON
                var deserializedInfo = JsonConvert.DeserializeObject<DirectoryData>(File.ReadAllText("directoryInfo.json"));

                // ak je deserializácia ok, vypíš štruktúru
                if (deserializedInfo != null)
                {
                    Console.WriteLine("\nDeserialized Directory Information:");
                    PrintDirectoryStructure(deserializedInfo);
                }
                else
                {
                    Console.WriteLine("Chyba: Deserializácia JSON zlyhala.");
                }
            }
            else
            {
                // cesta neexistuje, vypíš chybu
                Console.WriteLine($"Chyba: Adresár '{fullPath}' neexistuje.");
            }
        }

        // získa info o adresári
        public static DirectoryData GetDirectoryInfo(string path)
        {
            var directoryData = new DirectoryData
            {
                Name = Path.GetFileName(path),
                Files = new List<FileData>(),
                Directories = new List<DirectoryData>()
            };

            // získať súbory
            foreach (var file in Directory.GetFiles(path))
            {
                directoryData.Files.Add(new FileData
                {
                    Name = Path.GetFileName(file),
                    Extension = Path.GetExtension(file)
                });
            }

            // získať podadresáre
            foreach (var dir in Directory.GetDirectories(path))
            {
                directoryData.Directories.Add(GetDirectoryInfo(dir));
            }

            return directoryData;
        }

        // vypíše unikátne prípony
        public static void PrintUniqueExtensions(DirectoryData directory)
        {
            var extensions = new HashSet<string>();
            GetExtensions(directory, extensions);
            Console.WriteLine("Unique file extensions:");
            foreach (var ext in extensions)
            {
                Console.WriteLine(ext);
            }
        }

        // získa všetky prípony
        public static void GetExtensions(DirectoryData directory, HashSet<string> extensions)
        {
            foreach (var file in directory.Files)
            {
                extensions.Add(file.Extension);
            }

            // rekurzívne získa prípony aj z podadresárov
            foreach (var subDir in directory.Directories)
            {
                GetExtensions(subDir, extensions);
            }
        }

        // vypíše štruktúru adresára
        public static void PrintDirectoryStructure(DirectoryData directory, string indent = "")
        {
            Console.WriteLine($"{indent}Directory: {directory.Name}");

            foreach (var file in directory.Files)
            {
                Console.WriteLine($"{indent}  File: {file.Name} ({file.Extension})");
            }

            // rekurzívne vypíše podadresáre
            foreach (var subDir in directory.Directories)
            {
                PrintDirectoryStructure(subDir, indent + "  ");
            }
        }
    }

    // ukladá info o adresári
    public class DirectoryData
    {
        public string Name { get; set; } = string.Empty;
        public List<FileData> Files { get; set; } = new List<FileData>();
        public List<DirectoryData> Directories { get; set; } = new List<DirectoryData>();
    }

    // ukladá info o súbore
    public class FileData
    {
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }
}
