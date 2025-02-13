using System.Text.Json;
using System.Windows.Controls;

namespace WpfApp1
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