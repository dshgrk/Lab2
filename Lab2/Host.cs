using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Lab2
{
    public class Host
    {
        public Action<ITelegramBotClient, Update>? OnMessage;

        private TelegramBotClient bot;

        public Host(string token)
        {
            bot = new TelegramBotClient(token);
        }

        public void Start()
        {
            bot.StartReceiving(UpdateHandler, ErrorHandler);
            Console.WriteLine("Бот запущен");
        }

        private Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            //Console.WriteLine($"Боту что-то пришло: {update.Message?.Text ?? "[не текст]"}");
            //OnMessage?.Invoke(client, update);
            //await Task.CompletedTask;

            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from {message?.Chat.FirstName}.");

            // Echo received message text
            Message sentMessage = await client.SendTextMessageAsync(
                chatId: chatId,
                text: "> " + messageText,
                cancellationToken: token);
        }

    }
}
