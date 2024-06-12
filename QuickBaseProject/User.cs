using System.Text.Json.Serialization;

namespace QuickBaseProject
{
    public record class User(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("location")] string Address,
        [property: JsonPropertyName("bio")] string Description,
        [property: JsonPropertyName("created_at")] DateTime CreationDateUtc,
        [property: JsonPropertyName("login")] string Login)
    {
        public DateTime CreationDate => CreationDateUtc.ToLocalTime();
    }
}
