using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArcaeaWorldMapEditor
{
    public class CharacterItem : INotifyPropertyChanged
    {
        private bool _isVisible = true;
        private bool _isSelected;

        public int Key { get; set; }
        public string Value { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public CharacterItem(int key, string value)
        {
            Key = key;
            Value = value;
            IsSelected = false;
            IsVisible = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class CharacterData
    {
        public static List<CharacterItem> GetCharacters()
        {
            return new List<CharacterItem>
            {
                new CharacterItem(0, "Hikari"),
                new CharacterItem(1, "Tairitsu"),
                new CharacterItem(2, "Kou"),
                new CharacterItem(3, "Sapphire"),
                new CharacterItem(4, "Lethe"),
                new CharacterItem(5, "Hikari & Tairitsu (Reunion)"),
                new CharacterItem(6, "Tairitsu (Axium)"),
                new CharacterItem(7, "Tairitsu (Grievous Lady)"),
                new CharacterItem(8, "Stella"),
                new CharacterItem(9, "Hikari & Fisica"),
                new CharacterItem(10, "Ilith"),
                new CharacterItem(11, "Eto"),
                new CharacterItem(12, "Luna"),
                new CharacterItem(13, "Shirabe"),
                new CharacterItem(14, "Hikari (Zero)"),
                new CharacterItem(15, "Hikari (Fracture)"),
                new CharacterItem(16, "Hikari (Summer)"),
                new CharacterItem(17, "Tairitsu (Summer)"),
                new CharacterItem(18, "Tairitsu & Trin"),
                new CharacterItem(19, "Ayu"),
                new CharacterItem(20, "Eto & Luna"),
                new CharacterItem(21, "Yume"),
                new CharacterItem(22, "Hikari & Seine"),
                new CharacterItem(23, "Saya"),
                new CharacterItem(24, "Tairitsu & Chuni Penguin"),
                new CharacterItem(25, "Chuni Penguin"),
                new CharacterItem(26, "Haruna"),
                new CharacterItem(27, "Nono"),
                new CharacterItem(28, "Pandora Nemesis (MTA-XXX)"),
                new CharacterItem(29, "Regulus (MDA-21)"),
                new CharacterItem(30, "Kanae"),
                new CharacterItem(31, "Hikari (Fantasia)"),
                new CharacterItem(32, "Tairitsu (Sonata)"),
                new CharacterItem(33, "Sia"),
                new CharacterItem(34, "DORO*C"),
                new CharacterItem(35, "Tairitsu (Tempest)"),
                new CharacterItem(36, "Brillante (E/S Primera)"),
                new CharacterItem(37, "Ilith (Summer)"),
                new CharacterItem(38, "Saya (Etude)"),
                new CharacterItem(39, "Alice & Tenniel"),
                new CharacterItem(40, "Luna & Mia"),
                new CharacterItem(41, "Areus"),
                new CharacterItem(42, "Seele Haze"),
                new CharacterItem(43, "Isabelle Yagrush"),
                new CharacterItem(44, "Mir"),
                new CharacterItem(45, "Lagrange"),
                new CharacterItem(46, "Linka"),
                new CharacterItem(47, "Nami"),
                new CharacterItem(48, "Saya & Elizabeth"),
                new CharacterItem(49, "Lily"),
                new CharacterItem(50, "Kanae (Midsummer)"),
                new CharacterItem(51, "Alice & Tenniel (Minuet)"),
                new CharacterItem(52, "Tairitsu (Elegy)"),
                new CharacterItem(53, "Marija"),
                new CharacterItem(54, "Vita"),
                new CharacterItem(55, "Hikari (Fatalis)"),
                new CharacterItem(56, "Saki"),
                new CharacterItem(57, "Setsuna"),
                new CharacterItem(58, "Amane"),
                new CharacterItem(59, "Kou (Winter)"),
                new CharacterItem(60, "Lagrange (Aria)"),
                new CharacterItem(61, "Lethe (Apophenia)"),
                new CharacterItem(62, "Shama"),
                new CharacterItem(63, "Milk"),
                new CharacterItem(64, "Shikoku"),
                new CharacterItem(65, "Mika Yurisaki"),
                new CharacterItem(66, "Mithra Tercera"),
                new CharacterItem(67, "Toa Kozukata"),
                new CharacterItem(68, "Nami(Twilight)"),
                new CharacterItem(69, "Ilith & Ivy"),
                new CharacterItem(70, "Hikari & Vanessa"),
                new CharacterItem(71, "Maya"),
                new CharacterItem(72, "Lacrymira (Insight)"),
                new CharacterItem(73, "Luin"),
                new CharacterItem(74, "Vita"),
                new CharacterItem(75, "Ai-chan"),
                new CharacterItem(76, "Luna & Ilot"),
                new CharacterItem(77, "Eto & Hoppe"),
                new CharacterItem(78, "Forlorn (Compassion)"),
                new CharacterItem(79, "Chinatsu"),
                new CharacterItem(80, "Tsumugi"),
                new CharacterItem(81, "Nai"),
                new CharacterItem(82, "Selene Sheryl"),
                new CharacterItem(83, "Salt"),
                new CharacterItem(84, "Acid"),
                new CharacterItem(99, "Shirahime")
            };
        }
    }
}