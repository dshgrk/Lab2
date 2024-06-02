using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;


namespace Lab2
{
    internal class Program
    {
        static string token = "7084049064:AAE4-mV8UzF5Z0W6ZZsloV4aZDW5L_gnZPs";

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Host mybot = new Host(token);

            mybot.Start();
            //mybot.OnMessage += OnMessage;

            Console.ReadLine();
        }

        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            //try
            //{

                string messageText = update.Message?.Text;
                long chatId = update.Message?.Chat.Id ?? 0;


                //switch (messageText)
                //{
                //    case "/start":
                //        await client.SendTextMessageAsync(chatId, "Вітаю!");
                //        break;
                //    case "/info":
                //        await client.SendTextMessageAsync(chatId, "Цей бот створено для пошуку знижок в магазині \"Аврора\"!");
                //        break;
                //    //case "/catfact":
                //    //    string catFact = await GetRandomCatFactAsync(); // Получаем случайный факт о кошке
                //    //    await client.SendTextMessageAsync(chatId, catFact); // Отправляем факт о кошке пользователю
                //    //    break;
                //    default:
                //        await client.SendTextMessageAsync(chatId, "Доступні команди: \n/start\n/info\n/catfact"); // Добавляем команду /catfact в список доступных команд
                //        break;
                //}
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Відбулась помилка: {ex.Message}");
            //}
        }



        //// Метод для получения случайного факта о кошке из API
        //private static async Task<string> GetRandomCatFactAsync()
        //{
        //    string apiUrl = "https://catfact.ninja/fact";
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            JObject jsonResponse = JObject.Parse(responseBody);
        //            return jsonResponse["fact"].ToString(); // Получаем факт о кошке из JSON-ответа
        //        }
        //        else
        //        {
        //            return "Не удалось получить факт о кошке :(";
        //        }
        //    }
        //}
    }
}
