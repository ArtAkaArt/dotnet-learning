using System.Runtime.InteropServices;
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
                    buffer = sr.ReadToEnd();
            }
            List<Deal> DealArray = JsonConvert.DeserializeObject<List<Deal>>(buffer);
            IList<string> selectedDeals = GetNumbersOfDeals(DealArray);
            Console.Write($"Количество значений: {selectedDeals.Count}");
            foreach (string item in selectedDeals)
            {
                Console.Write($", {item}");
            }
            Console.WriteLine();
            IList<SumByMonth> SumByMonth = GetSumsByMonth(DealArray);
            foreach (var t in SumByMonth){
                Console.WriteLine($"Номер месяца: {t.Month.Month}, общая сумма сделок: {t.Sum}");
            }
        }
        static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals) // изменил сигнатуру с IList<int> на IList<string> т.к. Id представлены как string
        {
            
            IList<string> result = deals.Where(t=> t.Sum>100).OrderBy(t => t.Date).Take(5).OrderByDescending(t=>t.Sum).Select(t => t.Id).ToList();
            return result;
        }
        static IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
        {
            IList<SumByMonth> result = new List<SumByMonth>();

            var dealGroups = deals.GroupBy(t => t.Date.Month);

            foreach (var groups in dealGroups){
                result.Add(new SumByMonth(groups.First().Date, groups.Sum(c => c.Sum)));
            }
            return result;
/*
Тут я пытался все в одном запросе сделать, но как-то не вышло
IList<SumByMonth> result = deals.GroupBy(l => l.Date.Month)
                                        .Select(cl => new SumByMonth
                                        {
                                        Month = cl.First().Date,
                                        Sum = cl.Sum( c => c.Sum),
                                        }).ToList();
*/

        }
    }
    class Deal
    {
        public int Sum {get;set;}
        public string Id {get;set;}
        public DateTime Date {get;set;}
    }
    record SumByMonth(DateTime Month, int Sum);
}
