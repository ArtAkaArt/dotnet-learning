using Dadata;

namespace Solution {
    class Programm
    {
        public static async Task Main(string[] args)
        {

            Console.WriteLine("Введите ИНН");
            string INN = Console.ReadLine();
            string companyName = "";
            if (INN != null) companyName = await GetCompanyName(INN);
            Console.WriteLine(companyName);

            async Task<string> GetCompanyName(string INN) // ИНН должен быть десятизначным числом
            {
                try
                {
                    if (INN?.Length != 10) throw new Exception("Размер ИНН компании должен быть 10 символов");
                    if (!Int64.TryParse(INN, out long number)) throw new Exception("Введено не число");
                    if (number < 0) throw new Exception("ИНН должен быть положительным числом");

                    var token = "045e1fcb358827b660f924397ff72edf1fa7bda1";
                    var api = new SuggestClientAsync(token);
                    var response = await api.FindParty(INN);
                    var party = response.suggestions[0].data;

                    return party.name.full;
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Произошла ошибка, повторите ввод ИНН или введите \"-\" без кавычек для отмены");
                    var INN2 = Console.ReadLine();
                    if (INN2 != "-" && INN2 != null) return await GetCompanyName(INN2);
                    return "Выход";
                }
            }
        }
    }
}
