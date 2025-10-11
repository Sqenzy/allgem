using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace Allgem01.Components.Pages.Games
{
    public class GameLoad
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }

        public string lgpid { get; set; }
        public string lgpimgurl { get; set; }

        public static GameLoad LoadById(string gameId)
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Components", "Pages", "Games", "games.json");
            if (!File.Exists(jsonPath))
                return null;

            var json = File.ReadAllText(jsonPath);
            var games = JsonSerializer.Deserialize<List<GameLoad>>(json);
            var game = games?.FirstOrDefault(g => g.id == gameId);

            if (game != null)
            {
                game.lgpid = game.id;
                game.lgpimgurl = string.IsNullOrEmpty(game.imageUrl) ? "/defaultimg.png" : game.imageUrl;
            }

            return game;
        }
    }
}