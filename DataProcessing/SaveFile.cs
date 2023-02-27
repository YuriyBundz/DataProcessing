using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataProcessing
{
    public static class SaveFile
    {
        public static string SaveToFile(IEnumerable<CitySummary> source)
        {
            string json = JsonSerializer.Serialize(source);
            string folderC = DateTime.Now.ToString("MM/dd/yyyy");
            string savePath = Config.WritePath + "\\" + folderC;
            Directory.CreateDirectory(savePath);
            int countFiles = Directory.GetFiles(savePath, "*.json").Count() + 1;
            string fileName = savePath + "\\output" + countFiles + ".json";
            File.WriteAllText(fileName, json);
            return fileName;
        }
    }
}
