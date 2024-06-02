using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;
using File = System.IO.File;
using HtmlAgilityPack;
using System.Text.Json;
using System.Net.Http;
using RestSharp;


namespace GUIbot
{
    internal class Host
    {

        TelegramBotClient botClient = new TelegramBotClient("7059787665:AAG08d9PF4z7-1HDVJBo8lezpytgctk38II");

        //мой апи ключ из сайта базы фильмов
        private const string TmdbApiKey = "f66dc291d524e858d5ac9797c967a780";

        CancellationTokenSource cts = new ();

        public Host()
        {
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            
        }


        public async Task StartAsync()
        {
            var me = await botClient.GetMeAsync();

            Debug.WriteLine($"Start listening for @{me.Username}");
        }


        public void Cancel() 
        {
            cts.Cancel();
        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Debug.WriteLine($"Received a '{messageText}' message in chat {chatId} from {message.Chat.FirstName}.");

            if (messageText.StartsWith("/"))
            {
                switch (messageText.ToLower())
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Вітаю!",
                            cancellationToken: cancellationToken);
                        break;
                    case "/info":
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Бот було створено як навчальний проєкт",
                            cancellationToken: cancellationToken);
                        break;

                    case "/randomfilm":
                        var movie = await GetRandomMovieAsync();
                       
                        if (movie != null)
                        {
                            string imageUrl = $"https://image.tmdb.org/t/p/w500/{movie.Poster_path}";

                            Message message1 = await botClient.SendPhotoAsync(
                                chatId: chatId,
                                photo: InputFile.FromUri(imageUrl),
                                caption: await FormatMovieInfoAsync(movie),
                                parseMode: ParseMode.Html,
                                cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId, "Помилка, не вийшло знайти фільм", cancellationToken: cancellationToken);
                        }
                        break;

                    default:
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Невідома команда, оберіть пропоновані з меню",
                            cancellationToken: cancellationToken);
                        break;
                }
            }

        }


        async Task<Movie> GetRandomMovieAsync()
        {
            var client = new RestClient("https://api.themoviedb.org/3");
            var request = new RestRequest("movie/popular", Method.Get);
            request.AddParameter("api_key", TmdbApiKey);
            request.AddParameter("language", "uk-UA,en-US");
            request.AddParameter("append_to_response", "videos,images,genres");

            Random rnd = new Random();
            int numPage = rnd.Next(1, 16);

            request.AddParameter("page", numPage);

            var response = await client.ExecuteAsync<MovieList>(request);

            if (response.IsSuccessful)
            {
                var movieList = response.Data;
                if (movieList?.Results?.Count > 0)
                {
                    var random = new Random();
                    var randomIndex = random.Next(0, movieList.Results.Count);
                    var movieId = movieList.Results[randomIndex].Id;
                    return await GetMovieDetailsAsync(movieId);
                }
            }
            else
            {
                Debug.WriteLine($"Failed to get a successful response. Status code: {response.StatusCode}");
            }
            return null;
        }


        async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            var client = new RestClient("https://api.themoviedb.org/3");
            var request = new RestRequest($"movie/{movieId}", Method.Get);
            request.AddParameter("api_key", TmdbApiKey);
            request.AddParameter("language", "uk-UA,en-US"); 
            request.AddParameter("append_to_response", "videos,images,genres"); // Append extra requests for videos and images
            var response = await client.ExecuteAsync<Movie>(request);

            if (response.IsSuccessful)
            {
                var movie = response.Data;

                //проверяет есть ли описание на украинском языке
                if (!string.IsNullOrEmpty(movie.Overview))
                {
                    // если описание на украинском языке доступно, то возвращаем его
                    return movie;
                }
                else
                {
                    // если описание на украинском языке отсутствует, то пытаемся получить описание на английском языке
                    request.AddParameter("language", "en-US");
                    response = await client.ExecuteAsync<Movie>(request);
                    if (response.IsSuccessful)
                    {
                        // возвращает описание на английском языке
                        return response.Data;
                    }
                }
            }
            else
            {
                Debug.WriteLine($"Failed to get a successful response. Status code: {response.StatusCode}");
            }
            return null;
        }


        async Task<string> FormatMovieInfoAsync(Movie movie)
        {
            string adultOrNo = movie.Adult ? "Так" : "Ні";

            // Получаем только названия жанров фильма
            var genreNames = movie.Genres.Select(g => g.Name).ToList();

            string genres = string.Join(", ", genreNames);

            // Формирую информацию о фильме
            string formattedInfo = $"• Назва: \"{movie.Title}\". \n• Опис: {movie.Overview} \n• Жанр/и: {genres} \n• Дата випуску: {movie.Release_date} \n• Оцінка: {Math.Round(movie.Vote_average, 1)}/10 ({movie.Vote_Count} голосів) \n• Для дорослих?: {adultOrNo}";

            return formattedInfo;
        } 


        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Debug.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }

    public class MovieList
    {
        public List<Movie> Results { get; set; }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Poster_path { get; set; }
        public string Release_date { get; set; }
        public bool Adult { get; set; }
        public int Vote_Count { get; set; }
        public double Vote_average { get; set; }
        public List<Genres> Genres { get; set; }
    }

    public class Genres
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
