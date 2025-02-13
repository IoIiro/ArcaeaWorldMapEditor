using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

// UNUSED

namespace ArcaeaWorldMapEditor
{
    public class SongListLoader
    {
        private readonly string _filePath;

        public SongListLoader(string filePath = "songlist")
        {
            _filePath = filePath;
        }

        public SongList LoadSongList()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new SongList();
                }

                string jsonContent = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<SongList>(jsonContent, options) ?? new SongList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error loading songlist: {ex.Message}");
                return new SongList();
            }
        }

        public async Task<SongList> LoadSongListAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new SongList();
                }

                string jsonContent = await File.ReadAllTextAsync(_filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<SongList>(jsonContent, options) ?? new SongList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error loading songlist: {ex.Message}");
                return new SongList();
            }
        }
    }
} 