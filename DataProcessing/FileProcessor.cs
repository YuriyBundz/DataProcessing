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
using System.Timers;
using Timer = System.Timers.Timer;

namespace DataProcessing
{
    public class FileProcessor
    {
        FileParser fileManager = new FileParser();
        private static Timer _timer;
        Meta meta = new Meta();
        Reset reset = new Reset();

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.TimeOfDay == new TimeSpan(0, 0, 0))
            {
                meta.SaveMetaLog();
                meta = new Meta();
                reset = new Reset();
            }
        }

        public void Run()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Enabled = true;

            string[] files = Directory.GetFiles(Config.ReadPath);

            foreach (string file in files)
            {
                if (IsFileMatchingFilter(file))
                {
                    Console.WriteLine(file);
                    Process(file);
                }
                else
                {
                    meta.InvalidFiles.Add(file);
                }
            }

            FileSystemWatcher watcher = new FileSystemWatcher(Config.ReadPath);
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
        public void SaveMeta()
        {
            meta.SaveMetaLog();
        }
        public void Reset()
        {
            foreach (var item in reset.CreatedFiles)
            {
                File.Delete(item);
            }
            meta = new Meta();
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
                Process(e.FullPath);
            }
            else
            {
                meta.InvalidFiles.Add(e.FullPath);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (IsFileMatchingFilter(e.FullPath))
            {
                string value = $"Created: {e.FullPath}";
                Console.WriteLine(value);
                Process(e.FullPath);
            }
            else
            {
                meta.InvalidFiles.Add(e.FullPath);
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
        private void Process(string fullPath)
        {
            ParsingResult readFile = fileManager.Parse(fullPath);
            meta.ParsedLines += readFile.ParsedLines;
            meta.FoundErrors += readFile.FoundErrors;
            meta.ParsedFiles += readFile.ParsedFiles;
            IEnumerable<CitySummary> citySummary = Mapper.MapToCitySummary(readFile.CustomerList);
            string filePath = (SaveFile.SaveToFile(citySummary));
            reset.CreatedFiles.Add(filePath);
        }
    }
}