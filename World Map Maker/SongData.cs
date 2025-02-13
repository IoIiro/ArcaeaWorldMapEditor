using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WpfApp1
{
    public class SongList
    {
        [JsonPropertyName("songs")]
        public List<Song> Songs { get; set; } = new List<Song>();
    }

    public class Song
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
} 