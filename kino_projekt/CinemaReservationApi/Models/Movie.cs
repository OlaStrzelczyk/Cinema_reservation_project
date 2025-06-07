using System.Text.Json.Serialization;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public string PosterUrl { get; set; }

    [JsonIgnore] // 🛡️ zapobiega pętli serializacji
    public List<Screening> Screenings { get; set; }
}
