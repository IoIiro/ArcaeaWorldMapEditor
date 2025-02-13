using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1
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
                // In a production environment, you might want to log this error
                Console.WriteLine($"Error loading song list: {ex.Message}");
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
                // In a production environment, you might want to log this error
                Console.WriteLine($"Error loading song list: {ex.Message}");
                return new SongList();
            }
        }
    }
} 