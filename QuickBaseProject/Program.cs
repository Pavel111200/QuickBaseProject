using System.Net.Http.Headers;

string githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
string freshdeskToken = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN");

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", "Pavel111200");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {githubToken}");
client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", " 2022-11-28");

Console.WriteLine("Enter the username");
string username = Console.ReadLine();

await ProcessRepositoriesAsync(client, username);

static async Task ProcessRepositoriesAsync(HttpClient client, string username)
{
    var json = await client.GetStringAsync($"https://api.github.com/users/{username}");

    Console.WriteLine(json);
}