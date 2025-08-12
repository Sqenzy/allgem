using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class GitHubApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _token;

    public GitHubApiClient(string token)
    {
        _token = token;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
    }

    public async Task<string> CreateRepository(string repoName, string description = "", bool isPrivate = false)
    {
        var createRepoUrl = "https://api.github.com/user/repos";
        
        var repoData = new
        {
            name = repoName,
            description = description,
            @private = isPrivate,
            auto_init = true // This creates an initial commit
        };

        var json = JsonConvert.SerializeObject(repoData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(createRepoUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create repository: {responseContent}");
        }

        return responseContent;
    }
}