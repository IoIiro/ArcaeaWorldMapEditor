using System.Text.Json;
using System.Windows.Controls;

namespace ArcaeaWorldMapEditor
{
    public partial class JsonPreview : UserControl
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonPreview()
        {
            InitializeComponent();
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public void UpdateContent(object content)
        {
            JsonContent.Text = JsonSerializer.Serialize(content, _jsonOptions);
        }
    }
} 