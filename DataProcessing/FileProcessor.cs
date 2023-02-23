using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;

namespace DataProcessing
{
    public class FileProcessor
    {
        FileManager fileManager = new FileManager();
        private Meta meta = new Meta();

        //c# timmer

        public void SaveMeta()
        {
            meta.SaveMetaLog();
        }

        public void Run(object parameter)
        {
            string path = (string)parameter;

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (IsFileMatchingFilter(file))
                {
                    fileManager.ReadFile(file, meta);
                }
                else
                {
                    meta.Invalid_files.Add(file);
                }
            }
            
            FileSystemWatcher watcher = new FileSystemWatcher(path);
            watcher.Filter = "*.*";
            watcher.NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
            watcher.EnableRaisingEvents = true;

        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            if (IsFileMatchingFilter(e.FullPath))
            {
                Console.WriteLine($"Changed: {e.FullPath}");
                fileManager.ReadFile(e.FullPath, meta);
            }
            else
            {
                meta.Invalid_files.Add(e.FullPath);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (IsFileMatchingFilter(e.FullPath))
            {
                string value = $"Created: {e.FullPath}";
                Console.WriteLine(value);
                fileManager.ReadFile(e.FullPath, meta);
            }
            else
            {
                meta.Invalid_files.Add(e.FullPath);
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
        private bool IsFileMatchingFilter(string fullPath)
        {
            string fileName = Path.GetFileName(fullPath);
            return fileName.EndsWith(".txt") || fileName.EndsWith(".csv");
        }
    }
}