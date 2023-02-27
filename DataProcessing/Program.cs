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

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
CancellationToken cancellationToken = cancellationTokenSource.Token;

Console.WriteLine("Write: \nstart to Start\nreset to Reset\nstop to Stop");

do
{
    string command = Console.ReadLine();
    if (command == Commands.Start)
    {
        Task runTask = Task.Factory.StartNew(() => watcher.Run());
    }
    if (command == Commands.Reset)
    {
        watcher.Reset();
        cancellationTokenSource.Cancel();
    }
    if (command == Commands.Stop)
    {
        watcher.SaveMeta();
        cancellationTokenSource.Cancel();
        break;
    }
} while (true);

