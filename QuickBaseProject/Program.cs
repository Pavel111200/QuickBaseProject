using QuickBaseProject;

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

User user = await ProcessUserAsync(client, username);

static async Task<User> ProcessUserAsync(HttpClient client, string username)
{
    await using Stream stream =
    await client.GetStreamAsync($"https://api.github.com/users/{username}");
    var user = await JsonSerializer.DeserializeAsync<User>(stream);

    return user;
}