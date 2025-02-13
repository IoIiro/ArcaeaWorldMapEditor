using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public static class GameData
    {
        public static readonly List<string> CoreIds = new()
        {
            "core_hollow",
            "core_desolate",
            "core_chunithm",
            "core_crimson",
            "core_ambivalent",
            "core_scarlet",
            "core_groove",
            "core_binary",
            "core_colorful",
            "core_umbral",
            "core_wacca",
            "core_sunset",
            "core_tanoc",
            "core_serene",
            "core_maimai",
            "core_generic",
            "core_course_skip_purchase"
        };

        public static readonly long UnixMonth = 2592000000;

        public static readonly List<string> RestrictTypes = new()
        {
            "song_id",
            "pack_id"
        };

        public static readonly Dictionary<int, string> Chapters = new()
        {
            { 0, "Event" },
            { 1, "Lost World" },
            { 2, "Outer Reaches" },
            { 3, "Spire of Convergence" },
            { 4, "Dormant Echoes" },
            { 5, "Boundless Divide" },
            { 6, "Forgotten Construct" },
            { 7, "Horizon of Anamnesis" },
            { 8, "Evanescent Domain" },
            { 9, "Spire of the Gods" },
            { 1001, "Beyond" },
            { 2001, "Lost World (Breached)" }
        };

        public static class ItemTypes
        {
            public static readonly List<string> IdTypes = new()
            {
                "character",
                "world_song",
                "single",
                "pack",
                "core"
            };

            public static readonly List<string> AmountTypes = new()
            {
                "fragment",
                "memory",
                "stamina",
                "anni5tix",
                "pick_ticket",
                "core"
            };
        }

        public static readonly List<string> StepTypes = new()
        {
            "plusstamina",
            "speedlimit",
            "randomsong",
            "wall_nell",
            "wall_impossible"
        };
    }
}