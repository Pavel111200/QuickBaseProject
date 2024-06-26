﻿using Microsoft.VisualBasic;
using QuickBaseProject;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;



Environment.SetEnvironmentVariable("GITHUB_TOKEN", "mysecrettoken");
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

string connectionString = @"mysecretconnectionstring";
SqlConnection connection = new SqlConnection(connectionString);


Console.WriteLine("Enter the username");
string username = Console.ReadLine();

User user = await ProcessUserAsync(githubClient, username);
Console.WriteLine(user.CreationDate);

try
{
    connection.Open();

    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("INSERT INTO Users (Login,Name,CreatedOn)");
    stringBuilder.AppendLine($"VALUES ('{user.Login}', '{user.Name}', '{user.CreationDate.ToString("yyyy-MM-dd hh:MM:ss")}')");

    using (SqlCommand command = new SqlCommand(stringBuilder.ToString(), connection))
    {
        command.ExecuteNonQuery();
        Console.WriteLine("User successfully added in the database");
    }
}
catch (Exception e)
{
    Console.WriteLine("Error" + e.Message);
}

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