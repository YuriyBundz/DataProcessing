using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class Meta
    {
        public int Parsed_files { get; set; }
        public int Parsed_lines { get; set; }
        public int Found_errors { get; set; }
        public List<String> Invalid_files { get; set; } = new List<String>();

        public void SaveMetaLog()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("parsed_files: " + Parsed_files);
            sb.AppendLine("parsed_lines: " + Parsed_lines);
            sb.AppendLine("found_errors: " + Found_errors);
            sb.Append("invalid_files: [");
            sb.Append(string.Join(", ", Invalid_files));
            sb.Append("]");

            string folderC = DateTime.Now.ToString("MM/dd/yyyy");

            Directory.CreateDirectory(Config.WritePath + "\\" + folderC);

            string fileContent = sb.ToString();
            string filePath = Config.WritePath + "\\" + folderC + "\\meta.log";
            File.WriteAllText(filePath, fileContent);
        }
    }
}
