using QuickBaseProject;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

Environment.SetEnvironmentVariable("GITHUB_TOKEN", "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");
Environment.SetEnvironmentVariable("FRESHDESK_TOKEN", "abcdefghij1234567890");
Environment.SetEnvironmentVariable("FRESHDESK_DOMAIN", "thelocker");

string githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
string freshdeskToken = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN");


using HttpClient githubClient = new();
githubClient.DefaultRequestHeaders.Accept.Clear();
githubClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
githubClient.DefaultRequestHeaders.Add("User-Agent", "Pavel111200");
githubClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {githubToken}");
githubClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", " 2022-11-28");

using HttpClient freshdeskClient = new();
freshdeskClient.DefaultRequestHeaders.Accept.Clear();
freshdeskClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
freshdeskClient.DefaultRequestHeaders.Add("Authorization", $"Basic {freshdeskToken +":X"}");

Console.WriteLine("Enter the username");
string username = Console.ReadLine();

User user = await ProcessUserAsync(githubClient, username);
Console.WriteLine(user);

await CreateContact(freshdeskClient, user);

static async Task<User> ProcessUserAsync(HttpClient client, string username)
{
    await using Stream stream =
    await client.GetStreamAsync($"https://api.github.com/users/{username}");
    User user = await JsonSerializer.DeserializeAsync<User>(stream);

    return user;
}

static async Task CreateContact(HttpClient client, User user)
{
    string freshdeskDomain = Environment.GetEnvironmentVariable("FRESHDESK_DOMAIN");
    using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            name = user.Name,
            email = user.Email,
            address = user.Address,
            description = user.Description
        }),
        Encoding.UTF8,
        "application/json");

    using HttpResponseMessage response = await client.PostAsync(
        $"https://{freshdeskDomain}.freshdesk.com/api/v2/contacts", jsonContent);

    response.EnsureSuccessStatusCode();

    string jsonResponse = await response.Content.ReadAsStringAsync();
    Console.WriteLine(jsonResponse);
}