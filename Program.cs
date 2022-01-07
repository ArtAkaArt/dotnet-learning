using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string buffer = "";
            using (StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\JSON_sample_1.json"))
            {
                    buffer += sr.ReadToEnd();
            }
            List<Deal> DealArray = JsonConvert.DeserializeObject<List<Deal>>(buffer);
            IList<string> selectedDeals =  GetNumbersOfDeals(DealArray);
            Console.Write($"Количество значений: {selectedDeals.Count}");
            foreach (string item in selectedDeals)
            {
                Console.Write($", {item}");
            }

        }
      static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals) // изменил сигнатуру с IList<int> на IList<string> т.к. Id представлены как string
        {
            var selectedDeals = from t in deals
                                orderby t.Date
                                where t.Sum > 100
                                select t;

            var sortedDeals = selectedDeals.OrderByDescending(u=>u.Sum).Take(3);
            IList<string> result = new List<string>();
            foreach (Deal deal in sortedDeals){
                result.Add(deal.Id);
            }  
            return result;
        }
    }
    class Deal
    {
        public int Sum {get;set;}
        public string Id {get;set;}
        public DateTime Date {get;set;}
    }


}
