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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; }

        public Customer(string first_name, string last_name, string city, string address, decimal payment,
            DateTime date, long account_number, string service)
        {
            this.FirstName = first_name;
            this.LastName = last_name;
            this.City = city;
            this.Address = address;
            this.Payment = payment;
            this.Date = date;
            this.AccountNumber = account_number;
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
