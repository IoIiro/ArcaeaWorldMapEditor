using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1
{
    public static class MapData
    {
        private static string _mapId = string.Empty;
        private static bool _isLocked = false;
        private static bool _isLegacy = false;
        private static bool _isBeyond = false;
        private static bool _isBreached = false;
        private static bool _isRepeatable = false;
        private static int? _beyondHealth = null;
        private static readonly HashSet<int> _characterAffinity = new();

        public static void SetMapId(string mapId)
        {
            _mapId = mapId;
        }

        public static void SetIsLocked(bool isLocked)
        {
            _isLocked = isLocked;
        }

        public static void SetIsLegacy(bool isLegacy)
        {
            _isLegacy = isLegacy;
        }

        public static void SetIsBeyond(bool isBeyond)
        {
            _isBeyond = isBeyond;
            if (isBeyond) _isBreached = false;
        }

        public static void SetIsBreached(bool isBreached)
        {
            _isBreached = isBreached;
            if (isBreached) _isBeyond = false;
        }

        public static void SetIsRepeatable(bool isRepeatable)
        {
            _isRepeatable = isRepeatable;
        }

        public static void SetBeyondHealth(string healthStr)
        {

            _beyondHealth = null;


            if (!_isBeyond && !_isBreached)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(healthStr) && int.TryParse(healthStr, out int health))
            {
                _beyondHealth = health;
            }
        }

        public static bool ShouldShowBeyondHealth()
        {
            return _isBeyond || _isBreached;
        }

        public static void AddCharacterAffinity(int characterId)
        {
            _characterAffinity.Add(characterId);
        }

        public static void RemoveCharacterAffinity(int characterId)
        {
            _characterAffinity.Remove(characterId);
        }

        public static HashSet<int> GetCharacterAffinity()
        {
            return _characterAffinity;
        }

        public static Dictionary<string, object> GetMapData()
        {
            var dates = GetDate();
            var selectedDates = dates[0];

            var mapData = new Dictionary<string, object>
            {
                { "map_id", _mapId },
                { "is_legacy", _isLegacy },
                { "character_affinity", _characterAffinity.OrderBy(x => x).ToList() },
                { "available_from", selectedDates["available_from"] },
                { "available_to", selectedDates["available_to"] },
                { "is_repeatable", _isRepeatable },
                { "is_beyond", _isBeyond }
            };

            if (ShouldShowBeyondHealth())
            {
                mapData["beyond_health"] = _beyondHealth ?? 0;
            }

            mapData["is_breached"] = _isBreached;
            mapData["is_locked"] = _isLocked;

            return mapData;
        }

        public static List<Dictionary<string, long>> GetDate()
        {
            var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return new List<Dictionary<string, long>>
            {
                new Dictionary<string, long>
                {
                    { "available_from", -1 },
                    { "available_to", 9999999999999 }
                },
                new Dictionary<string, long>
                {
                    { "available_from", currentUnixTime },
                    { "available_to", currentUnixTime + GameData.UnixMonth }
                }
            };
        }
    }
}