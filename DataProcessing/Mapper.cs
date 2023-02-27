using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataProcessing
{
    public static class Mapper
    {
        public static IEnumerable<CitySummary> MapToCitySummary(List<Customer> cust)
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
                            Name = p.FirstName + " " + p.LastName,
                            Payment = p.Payment,
                            Date = p.Date,
                            AccountNumber = p.AccountNumber

                        }).ToArray(),
                        Total = b.Sum(b => b.Payment),
                    }).ToArray(),
                    Total = s.Sum(s => s.Payment)
                });
            return res;
        }
    }
}
