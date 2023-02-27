using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class Meta
    {
        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }
        public List<String> InvalidFiles { get; set; } = new List<String>();

        public void SaveMetaLog()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("parsed_files: " + ParsedFiles);
            sb.AppendLine("parsed_lines: " + ParsedLines);
            sb.AppendLine("found_errors: " + FoundErrors);
            sb.Append("invalid_files: [");
            sb.Append(string.Join(", ", InvalidFiles));
            sb.Append("]");

            string folderC = DateTime.Now.ToString("MM/dd/yyyy");

            Directory.CreateDirectory(Config.WritePath + "\\" + folderC);

            string fileContent = sb.ToString();
            string filePath = Config.WritePath + "\\" + folderC + "\\meta.log";
            File.WriteAllText(filePath, fileContent);
        }
    }
}
