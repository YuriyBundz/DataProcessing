using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using DataProcessing;
using Newtonsoft.Json;

string pathToJsonFile = @"Config.json";
string jsonContent = File.ReadAllText(pathToJsonFile);

var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
Config.ReadPath = config["ReadPath"];
Config.WritePath = config["WritePath"];

FileProcessor watcher = new FileProcessor();

Thread fileProcessorThread = new Thread(new ParameterizedThreadStart(watcher.Run));
Thread timeThrad = new Thread(watcher.TimeTracker);
Console.WriteLine("Write: start");

do
{
    string command = Console.ReadLine();
    if (command == "start")
    {
        timeThrad.Start();
        fileProcessorThread.Start(Config.ReadPath);
    }
    if (command == "stop")
    {
        watcher.SaveMeta();
        watcher.StopTimeTracker = true;
        break;
    }
} while (true);

