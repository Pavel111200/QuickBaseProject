using System.Text.Json.Serialization;

namespace QuickBaseProject
{
    public record class User(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("location")] string Address,
        [property: JsonPropertyName("bio")] string Description,
        [property: JsonPropertyName("created_at")] DateTime CreationDateUtc)
    {
        public DateTime CreationDate => CreationDateUtc.ToLocalTime();
    }
}
