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
            var buffer = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\JSON_sample_1.json");
            var dealsArray = JsonConvert.DeserializeObject<List<Deal>>(buffer);
            var selectedDeals = GetNumbersOfDeals(dealsArray);
            Console.Write($"Количество значений: {selectedDeals.Count}");
            foreach (var item in selectedDeals)
            {
                Console.Write($", {item}");
            }
            Console.WriteLine();
            var sumByMonth = GetSumsByMonth(dealsArray);
            foreach (var t in sumByMonth)
            {
                Console.WriteLine($"Номер месяца: {t.Month.Month}, общая сумма сделок: {t.Sum}");
            }

            IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
            { 
                return deals.Where(t=> t.Sum>100)
                            .OrderBy(t => t.Date)
                            .Take(5)
                            .OrderByDescending(t=>t.Sum)
                            .Select(t => t.Id)
                            .ToList();
            }
            
            IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
            {
                return deals.GroupBy(t => t.Date.Month)
                            .Select(cl => new SumByMonth{Month = cl.First().Date, Sum = cl.Sum(c => c.Sum)})
                            .ToList();
            }
        }
    }
}