using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class FileManager
    {
        private List<Customer> cust = new List<Customer>();

        public string Path { get; set; }

        public void ReadFile(string source, Meta meta)
        {
            int skip = 0;
            if (source.EndsWith(".csv"))
            {
                skip = 1;
            }

            foreach (string line in System.IO.File.ReadLines(source).Skip(skip))
            {
                String[] split = Regex.Split(line, ", ");
                try
                {
                    string city = split[2].Substring(1);
                    string address = split[3] + ", " + split[4].Substring(0, split[4].Length - 1);
                    cust.Add(new Customer(split[0], split[1], city, address, Decimal.Parse(split[5], CultureInfo.InvariantCulture),
                    DateTime.ParseExact(split[6], "yyyy-dd-MM", CultureInfo.InvariantCulture), long.Parse(split[7]), split[8]));
                    meta.Parsed_lines++;
                }
                catch (Exception)
                {
                    meta.Found_errors++;
                }
            }
            meta.Parsed_files++;
            CreateFile();
        }

        public void CreateFile()
        {
            var res = cust.GroupBy(x => x.City)
                .Select(s => new CitySummary()
                {
                    City = s.Key,
                    Services = s.GroupBy(b => b.Service)
                    .Select(b => new Service()
                    {
                        Name = b.Key,
                        Payers = b.Select(p => new Payer()
                        {
                            Name = p.First_name + " " + p.Last_name,
                            Payment = p.Payment,
                            Date = p.Date,
                            Account_number = p.Account_number

                        }).ToArray(),
                        Total = b.Sum(b => b.Payment),
                    }).ToArray(),
                    Total = s.Sum(s => s.Payment)
                });

            string json = JsonSerializer.Serialize(res);
            SaveFile(json);
        }

        public void SaveFile(string json)
        {
            string folderC = DateTime.Now.ToString("MM/dd/yyyy");
            string savePath = Config.WritePath + "\\" + folderC;
            Directory.CreateDirectory(savePath);
            int countFiles = Directory.GetFiles(savePath, "*.json").Count() + 1;
            string fileName = savePath + "\\output" + countFiles + ".json";
            File.WriteAllText(fileName, json); 
        }
    }
}
