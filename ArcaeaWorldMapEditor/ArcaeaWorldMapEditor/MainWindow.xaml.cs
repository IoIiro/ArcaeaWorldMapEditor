using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace ArcaeaWorldMapEditor;
public partial class MainWindow : Window
{
    private bool _updatingCheckboxes = false;
    private static readonly Regex _numberRegex = new Regex("[^0-9]+");
    private static readonly Regex _multiplierRegex = new Regex(@"^[0-9]*\.?[0-9]*$");
    private List<CharacterItem> _characters;
    private bool _isSearching = false;

    public MainWindow()
    {
        InitializeComponent();
        UpdateBeyondHealthVisibility();
        InitializeCharacterAffinity();
        InitializeChapterSelection();
    }

    private void InitializeCharacterAffinity()
    {
        _characters = CharacterData.GetCharacters();
        CharacterAffinityComboBox.ItemsSource = _characters;
        CharacterAffinityComboBox.Text = "Select character...";
    }

    private void CharacterAffinity_TextSearch(object sender, TextChangedEventArgs e)
    {
        if (_isSearching) return;

        _isSearching = true;
        try
        {
            var searchText = CharacterAffinityComboBox.Text?.ToLower() ?? string.Empty;
            if (string.IsNullOrEmpty(searchText))
            {
                CharacterAffinityComboBox.Text = "Select character...";
                foreach (var character in _characters)
                {
                    character.IsVisible = true;
                }
            }
            else if (searchText == "select character...")
            {
                foreach (var character in _characters)
                {
                    character.IsVisible = true;
                }
            }
            else
            {
                foreach (var character in _characters)
                {
                    character.IsVisible = character.Value?.ToLower().Contains(searchText) ?? false;
                }
                CharacterAffinityComboBox.IsDropDownOpen = true;
            }
        }
        finally
        {
            _isSearching = false;
        }
    }

    private void CharacterAffinity_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (CharacterAffinityComboBox.Text == "Select character...")
        {
            if (e.Key != Key.Tab && e.Key != Key.Enter)
            {
                CharacterAffinityComboBox.Text = string.Empty;
            }
            return;
        }

        if (e.Key == Key.Back && CharacterAffinityComboBox.Text.Length <= 1)
        {
            e.Handled = true;
            _isSearching = true;
            CharacterAffinityComboBox.Text = "Select character...";
            foreach (var character in _characters)
            {
                character.IsVisible = true;
            }
            _isSearching = false;
        }
    }

    private void CharacterAffinity_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (CharacterAffinityComboBox.Text == "Select character...")
        {
            CharacterAffinityComboBox.Text = string.Empty;
        }
    }

    private void CharacterAffinity_DropDownOpened(object sender, EventArgs e)
    {
        if (CharacterAffinityComboBox.Text == "Select character...")
        {
            CharacterAffinityComboBox.Text = string.Empty;
        }
    }

    private void CharacterAffinity_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox)
        {
            int characterId = (int)checkBox.Tag;
            bool isChecked = checkBox.IsChecked ?? false;
            var currentText = CharacterAffinityComboBox.Text;

            _isSearching = true;
            try
            {
                if (isChecked)
                {
                    MapData.AddCharacterAffinity(characterId);
                }
                else
                {
                    MapData.RemoveCharacterAffinity(characterId);
                    if (!MapData.GetCharacterAffinity().Any() && string.IsNullOrEmpty(currentText))
                    {
                        CharacterAffinityComboBox.Text = "Select character...";
                    }
                }

                UpdateMultipliersList();
                UpdatePreview();
                CharacterAffinityComboBox.IsDropDownOpen = true;
            }
            finally
            {
                _isSearching = false;
            }
        }
    }

    private void CharacterAffinity_Click(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
        if (sender is CheckBox)
        {
            CharacterAffinityComboBox.IsDropDownOpen = true;
        }
    }

    private void CharacterAffinity_LostFocus(object sender, RoutedEventArgs e)
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            if (CharacterAffinityComboBox.IsKeyboardFocusWithin ||
                CharacterAffinityComboBox.IsMouseOver ||
                Mouse.DirectlyOver is CheckBox ||
                (Mouse.DirectlyOver is FrameworkElement element &&
                 (element.Parent is CheckBox || element.TemplatedParent is CheckBox)))
            {
                CharacterAffinityComboBox.IsDropDownOpen = true;
            }
        }));
    }

    private void CharacterAffinity_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        e.Handled = true;
        if (!_isSearching)
        {
            CharacterAffinityComboBox.IsDropDownOpen = true;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        UpdatePreview();
    }

    private void MapIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (MapIdTextBox != null)
        {
            MapData.SetMapId(MapIdTextBox.Text);
            UpdatePreview();
        }
    }

    private void IsLockedCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (IsLockedCheckBox != null)
        {
            MapData.SetIsLocked(IsLockedCheckBox.IsChecked ?? false);
            UpdatePreview();
        }
    }

    private void IsLegacyCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (IsLegacyCheckBox != null)
        {
            MapData.SetIsLegacy(IsLegacyCheckBox.IsChecked ?? false);
            UpdatePreview();
        }
    }

    private void IsBeyondCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (_updatingCheckboxes || IsBeyondCheckBox == null) return;

        _updatingCheckboxes = true;
        try
        {
            var isChecked = IsBeyondCheckBox.IsChecked ?? false;
            MapData.SetIsBeyond(isChecked);
            if (isChecked && IsBreachedCheckBox != null)
            {
                IsBreachedCheckBox.IsChecked = false;
            }
            UpdateBeyondHealthVisibility();
            UpdatePreview();
        }
        finally
        {
            _updatingCheckboxes = false;
        }
    }

    private void IsBreachedCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (_updatingCheckboxes || IsBreachedCheckBox == null) return;

        _updatingCheckboxes = true;
        try
        {
            var isChecked = IsBreachedCheckBox.IsChecked ?? false;
            MapData.SetIsBreached(isChecked);
            if (isChecked && IsBeyondCheckBox != null)
            {
                IsBeyondCheckBox.IsChecked = false;
            }
            UpdateBeyondHealthVisibility();
            UpdatePreview();
        }
        finally
        {
            _updatingCheckboxes = false;
        }
    }

    private void IsRepeatableCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (IsRepeatableCheckBox != null)
        {
            MapData.SetIsRepeatable(IsRepeatableCheckBox.IsChecked ?? false);
            UpdatePreview();
        }
    }

    private void BeyondHealthTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BeyondHealthTextBox != null)
        {
            MapData.SetBeyondHealth(BeyondHealthTextBox.Text);
            UpdatePreview();
        }
    }

    private void BeyondHealthTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = _numberRegex.IsMatch(e.Text);
    }

    private void UpdateBeyondHealthVisibility()
    {
        if (BeyondHealthPanel != null)
        {
            bool shouldShow = MapData.ShouldShowBeyondHealth();
            BeyondHealthPanel.Visibility = shouldShow ? Visibility.Visible : Visibility.Collapsed;
            if (!shouldShow && BeyondHealthTextBox != null)
            {
                BeyondHealthTextBox.Text = "0";
            }


            if (LawsPanel != null)
            {
                LawsPanel.Visibility = MapData.GetMapData()["is_breached"].Equals(true)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }
    }

    private void DateRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (Preview != null)
        {
            UpdatePreview();
        }
    }

    private void UpdatePreview()
    {
        if (DefaultDateRadio == null || Preview == null) return;

        var mapData = MapData.GetMapData();
        if (DefaultDateRadio.IsChecked != true)
        {
            var dates = MapData.GetDate();
            mapData["available_from"] = dates[1]["available_from"];
            mapData["available_to"] = dates[1]["available_to"];
        }

        Preview.UpdateContent(mapData);
    }

    private void UpdateMultipliersList()
    {
        var selectedCharacters = MapData.GetCharacterAffinity();
        if (selectedCharacters.Any())
        {
            var multiplierItems = selectedCharacters.OrderBy(id => id).Select(id =>
            {
                var character = _characters.First(c => c.Key == id);
                return new MultiplierItem
                {
                    CharacterName = character.Value,
                    CharacterId = id,
                    Multiplier = MapData.GetAffinityMultiplier(id).ToString("0.0")
                };
            }).ToList();

            MultipliersList.ItemsSource = multiplierItems;
            MultipliersPanel.Visibility = Visibility.Visible;
        }
        else
        {
            MultipliersList.ItemsSource = null;
            MultipliersPanel.Visibility = Visibility.Collapsed;
        }
    }

    private void Multiplier_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox && float.TryParse(textBox.Text, out float value))
        {
            int characterId = (int)textBox.Tag;
            MapData.SetAffinityMultiplier(characterId, value);
            UpdatePreview();
        }
    }

    private void Multiplier_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !_multiplierRegex.IsMatch(e.Text);
    }

    private void Law_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked == true)
        {
            string law = radioButton.Tag.ToString();
            MapData.AddLaw(law);
            UpdatePreview();
        }
    }

    private void ChapterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ChapterComboBox.SelectedItem is KeyValuePair<int, string> chapter)
        {
            MapData.SetChapter(chapter.Key);
            UpdatePreview();
        }
    }

    private void ChapterComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {

        e.Handled = true;
    }

    private void InitializeChapterSelection()
    {
        ChapterComboBox.SelectedItem = null;
        ChapterComboBox.Text = "Select chapter...";
    }

    public class MultiplierItem
    {
        public string CharacterName { get; set; }
        public int CharacterId { get; set; }
        public string Multiplier { get; set; }
    }
}