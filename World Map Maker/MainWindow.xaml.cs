using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _updatingCheckboxes = false;
    private static readonly Regex _numberRegex = new Regex("[^0-9]+");
    private List<CharacterItem> _characters;
    private bool _isSearching = false;

    public MainWindow()
    {
        InitializeComponent();
        UpdateBeyondHealthVisibility();
        InitializeCharacterAffinity();
    }

    private void InitializeCharacterAffinity()
    {
        _characters = CharacterData.GetCharacters();
        CharacterAffinityComboBox.ItemsSource = _characters;
        CharacterAffinityComboBox.Text = "Search Characters...";
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
                CharacterAffinityComboBox.Text = "Search Characters...";
                foreach (var character in _characters)
                {
                    character.IsVisible = true;
                }
            }
            else if (searchText == "search characters...")
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
        if (CharacterAffinityComboBox.Text == "Search Characters...")
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
            CharacterAffinityComboBox.Text = "Search Characters...";
            foreach (var character in _characters)
            {
                character.IsVisible = true;
            }
            _isSearching = false;
        }
    }

    private void CharacterAffinity_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (CharacterAffinityComboBox.Text == "Search Characters...")
        {
            CharacterAffinityComboBox.Text = string.Empty;
        }
    }

    private void CharacterAffinity_DropDownOpened(object sender, EventArgs e)
    {
        if (CharacterAffinityComboBox.Text == "Search Characters...")
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

            if (isChecked)
            {
                MapData.AddCharacterAffinity(characterId);
                CharacterAffinityComboBox.Text = string.Empty;
            }
            else
            {
                MapData.RemoveCharacterAffinity(characterId);
                if (!MapData.GetCharacterAffinity().Any())
                {
                    CharacterAffinityComboBox.Text = "Search Characters...";
                }
            }

            UpdatePreview();
        }
    }

    private void CharacterAffinity_Click(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
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
        }
    }

    private void DateRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (Preview != null)  // Ensure the control is initialized
        {
            UpdatePreview();
        }
    }

    private void UpdatePreview()
    {
        if (DefaultDateRadio == null || Preview == null) return;  // Guard against null controls

        var mapData = MapData.GetMapData();
        if (DefaultDateRadio.IsChecked != true)  // If Unix time is selected
        {
            var dates = MapData.GetDate();
            mapData["available_from"] = dates[1]["available_from"];
            mapData["available_to"] = dates[1]["available_to"];
        }

        Preview.UpdateContent(mapData);
    }
}