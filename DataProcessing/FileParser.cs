using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class FileParser
    {
        private const int firstNameInd = 0;
        private const int lastNameInd = 1;
        private const int cityInd = 2;
        private const int streetInd = 3;
        private const int houseNumInd = 4;
        private const int paymentInd = 5;
        private const int dateInd = 6;
        private const int accountInd = 7;
        private const int serviceInd = 8;

        public ParsingResult Parse(string source)
        {
            
            int skip = 0;
            if (source.EndsWith(".csv"))
            {
                skip = 1;
            }
            ParsingResult parsResult = new ParsingResult();
            foreach (string line in System.IO.File.ReadLines(source).Skip(skip))
            {
                String[] split = Regex.Split(line, ", ");
                try
                {
                    string city = split[cityInd].Substring(1);
                    string address = split[streetInd] + ", " + split[houseNumInd].Substring(0, split[houseNumInd].Length - 1);
                    parsResult.CustomerList.Add(new Customer(split[firstNameInd], split[lastNameInd], city, address, Decimal.Parse(split[paymentInd]
                        ,CultureInfo.InvariantCulture), DateTime.ParseExact(split[dateInd], "yyyy-dd-MM"
                        ,CultureInfo.InvariantCulture), long.Parse(split[accountInd]), split[serviceInd]));
                   parsResult.ParsedLines++;
                }
                catch (Exception)
                {
                   parsResult.FoundErrors++;
                }
            }
            parsResult.ParsedFiles++;
            return parsResult;
        }
    }
}
