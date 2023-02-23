using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class Customer
    {
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long Account_number { get; set; }
        public string Service { get; set; }

        public Customer(string first_name, string last_name, string city, string address, decimal payment,
            DateTime date, long account_number, string service)
        {
            this.First_name = first_name;
            this.Last_name = last_name;
            this.City = city;
            this.Address = address;
            this.Payment = payment;
            this.Date = date;
            this.Account_number = account_number;
            this.Service = service;
        }
        public string Serialize()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
        public static Customer Deserialize(string source)
        {
            return JsonSerializer.Deserialize<Customer>(source);
        }
    }
}
