using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace FileSystemMonitor
{
    internal class Program
    {
        private static string _watchFilePath;
        private static bool _isIncludeSubdirectories;
        private static List<FileLog> _fileLogs;

        private static void Main(string[] args)
        {
            _watchFilePath = @"W:\Demo";
            _isIncludeSubdirectories = true;
            _fileLogs = new List<FileLog>();

            var watcher = new FileSystemWatcher
            {
                Filter = "*.*",
                Path = _watchFilePath + "\\",
                
                IncludeSubdirectories = _isIncludeSubdirectories,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;
            watcher.EnableRaisingEvents = true;

            var tmr = new Timer(1000 * 20);
            tmr.Elapsed += delegate { SendNotify(); };
            tmr.Start();

            var command = "";
            do
            {
                command = Console.ReadLine();
            } while (command != "exit");
        }

        public static void SendNotify()
        {
            if (!_fileLogs.Any()) return;

            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine("=============File Change Notify=============");

            //foreach (var model in _fileLogs)
            //{
            //    Console.WriteLine("=======================================");
            //    Console.WriteLine("Type => " + model.ChangeType);
            //    Console.WriteLine("FullPath => " + model.FullPatch);
            //    Console.WriteLine("Time => " + model.ChangeTime.ToString("yyyy/MM/dd HH:mm"));
            //    Console.WriteLine("=======================================");
            //}

            _fileLogs.Clear();

            var cmdHelper = new CmdHelper(true);
            cmdHelper.ExecNonCmd("W:&cd W:\\Demo&git reset --hard");
            Console.Clear();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name.StartsWith(".git\\")) return;
            if (_fileLogs.Any(x => x.FullPatch == e.FullPath)) return;

            var model = new FileLog
            {
                FullPatch = e.FullPath,
                ChangeType = e.ChangeType.ToString(),
                ChangeTime = DateTime.Now
            };
            _fileLogs.Add(model);

            Console.WriteLine("=======================================");
            Console.WriteLine("File Changed!!");
            Console.WriteLine("Type => " + model.ChangeType);
            Console.WriteLine("FullPath => " + model.FullPatch);
            Console.WriteLine("Time => " + model.ChangeTime.ToString("yyyy/MM/dd HH:mm"));
            Console.WriteLine("=======================================");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.Name.StartsWith(".git\\")) return;
            if (_fileLogs.Any(x => x.FullPatch == e.FullPath)) return;

            var model = new FileLog
            {
                FullPatch = e.OldFullPath,
                ChangeType = e.ChangeType.ToString(),
                ChangeTime = DateTime.Now
            };
            _fileLogs.Add(model);

            Console.WriteLine("=======================================");
            Console.WriteLine("File Changed!!");
            Console.WriteLine("Type => " + model.ChangeType);
            Console.WriteLine("FullPath => " + model.FullPatch);
            Console.WriteLine("Time => " + model.ChangeTime.ToString("yyyy/MM/dd HH:mm"));
            Console.WriteLine("=======================================");
        }
    }
    

    internal class FileLog
    {
        public string FullPatch { get; set; }

        public string ChangeType { get; set; }

        public DateTime ChangeTime { get; set; }
    }
}