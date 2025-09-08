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
        public string gameid { get; set; }

        public static GameLoad LoadById(string gameId)
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Components", "Pages", "Games", "games.json");
            if (!File.Exists(jsonPath))
                return null;

            var json = File.ReadAllText(jsonPath);
            var games = JsonSerializer.Deserialize<List<GameLoad>>(json);
            return games?.FirstOrDefault(g => g.id == gameId);
        }
    }
}