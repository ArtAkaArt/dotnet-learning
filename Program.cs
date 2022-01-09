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
            var DealArray = JsonConvert.DeserializeObject<List<Deal>>(buffer);
            var selectedDeals = GetNumbersOfDeals(DealArray);
            Console.Write($"Количество значений: {selectedDeals.Count}");
            foreach (string item in selectedDeals)
            {
                Console.Write($", {item}");
            }
            Console.WriteLine();
            var SumByMonth = GetSumsByMonth(DealArray);
            foreach (var t in SumByMonth){
                Console.WriteLine($"Номер месяца: {t.Month.Month}, общая сумма сделок: {t.Sum}");
            }
        }
        static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
        {
            
            IList<string> result = deals.Where(t=> t.Sum>100)
                                    .OrderBy(t => t.Date).Take(5)
                                    .OrderByDescending(t=>t.Sum)
                                    .Select(t => t.Id)
                                    .ToList();
            return result;
        }
        static IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
        {
            IList<SumByMonth> result = new List<SumByMonth>();

            result = deals.GroupBy(t => t.Date.Month)
                        .Select(cl => new SumByMonth{Month = cl.First().Date, Sum = cl.Sum(c => c.Sum)})
                        .ToList();

            return result;
        }
    }
    class Deal
    {
        public int Sum {get;set;}
        public string Id {get;set;}
        public DateTime Date {get;set;}
    }
    class SumByMonth{ 
        public int Sum {get;set;}
        public DateTime Month {get;set;}
    }
    //record SumByMonth(DateTime Month, int Sum); не получалось инициализировать рекорд в ст. 43, переделал в класс. Не было конструктора без параметров?
}
