using System.Text.Json;
using System.Text.RegularExpressions;

namespace Allgem01.Logic;

public class GameService
{
    private readonly string _gamesJsonPath;
    private readonly IWebHostEnvironment _environment;

    public GameService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _gamesJsonPath = Path.Combine(_environment.WebRootPath, "..", "Components", "Pages", "Games", "games.json");
    }

    public async Task<List<Game>> GetGamesAsync()
    {
        try
        {
            if (!File.Exists(_gamesJsonPath))
                return new List<Game>();

            var jsonContent = await File.ReadAllTextAsync(_gamesJsonPath);
            var games = JsonSerializer.Deserialize<List<Game>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return games ?? new List<Game>();
        }
        catch (Exception ex)
        {
            // Log the exception if you have logging configured
            Console.WriteLine($"Error reading games: {ex.Message}");
            return new List<Game>();
        }
    }

    public async Task<bool> AddGameAsync(Game newGame)
    {
        try
        {
            var games = await GetGamesAsync();
            
            // Generate ID and IDN
            newGame.Id = GenerateId(newGame.Name);
            newGame.Idn = (games.Count + 1).ToString();
            
            // Set default image URL if not provided
            if (string.IsNullOrEmpty(newGame.ImageUrl))
            {
                newGame.ImageUrl = $"gameimages/{newGame.Id}.webp";
            }

            games.Add(newGame);
            
            var jsonContent = JsonSerializer.Serialize(games, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(_gamesJsonPath, jsonContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding game: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateGameAsync(string gameId, Game updatedGame)
    {
        try
        {
            var games = await GetGamesAsync();
            var gameIndex = games.FindIndex(g => g.Id == gameId);
            
            if (gameIndex == -1)
                return false;

            updatedGame.Id = gameId;
            updatedGame.Idn = games[gameIndex].Idn; // Preserve original IDN
            
            games[gameIndex] = updatedGame;
            
            var jsonContent = JsonSerializer.Serialize(games, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(_gamesJsonPath, jsonContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteGameAsync(string gameId)
    {
        try
        {
            var games = await GetGamesAsync();
            var gameToRemove = games.FirstOrDefault(g => g.Id == gameId);
            
            if (gameToRemove == null)
                return false;

            games.Remove(gameToRemove);
            
            // Update IDN values to maintain sequential numbering
            for (int i = 0; i < games.Count; i++)
            {
                games[i].Idn = (i + 1).ToString();
            }
            
            var jsonContent = JsonSerializer.Serialize(games, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(_gamesJsonPath, jsonContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting game: {ex.Message}");
            return false;
        }
    }

    private string GenerateId(string gameName)
    {
        // Convert game name to a URL-friendly ID
        var id = Regex.Replace(gameName.ToLower(), @"[^a-z0-9\s-]", "");
        id = Regex.Replace(id, @"\s+", "");
        id = id.Replace("-", "");
        
        // Ensure it's not empty and add a timestamp to make it unique
        if (string.IsNullOrEmpty(id))
        {
            id = "game";
        }
        
        return $"{id}{DateTime.Now:yyyyMMddHHmmss}";
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile, string gameId)
    {
        try
        {
            var imagesDirectory = Path.Combine(_environment.WebRootPath, "gameimages");
            
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            var fileName = $"{gameId}.webp";
            var filePath = Path.Combine(imagesDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"gameimages/{fileName}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving image: {ex.Message}");
            return string.Empty;
        }
    }
}
