using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExternalImGui
{
    public class ConfigData
    {
        [JsonConverter(typeof(Vector4Converter))]
        public Vector4 menuColor { get; set; }
        public bool enableEsp { get; set; }
        public bool enableBox { get; set; }
        public bool enableHealthbar { get; set; }
        public bool enableNames { get; set; }
        public bool enableLines { get; set; }

        [JsonConverter(typeof(Vector4Converter))]
        public Vector4 boxColor { get; set; }

        [JsonConverter(typeof(Vector4Converter))]
        public Vector4 linesColor { get; set; }

        public int boxMode { get; set; }

        [JsonConverter(typeof(Vector4Converter))]
        public Vector4 customColor { get; set; }

        public bool enableCrosshair { get; set; }
        public bool enableRageCross { get; set; }
        public float CrosshairSize { get; set; }

        [JsonConverter(typeof(Vector4Converter))]
        public Vector4 crosshairColor { get; set; }

        public bool enableWM { get; set; }

        [JsonConverter(typeof(Vector4Converter))]
        public Vector4 wmColor { get; set; }

        public bool enableNightMode { get; set; }
    }


    public static class ConfigManager
    {
        private static readonly string configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs");

        public static void Save(string name)
        {
            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);

            ConfigData data = new ConfigData
            {
                menuColor = MenuData.menuColor,
                enableEsp = MenuData.enableEsp,
                enableBox = MenuData.enableBox,
                enableHealthbar = MenuData.enableHealthbar,
                enableNames = MenuData.enableNames,
                enableLines = MenuData.enableLines,
                boxColor = MenuData.boxColor,
                linesColor = MenuData.linesColor,
                boxMode = MenuData.boxMode,
                customColor = MenuData.customColor,

                enableCrosshair = MenuData.enableCrosshair,
                enableRageCross = MenuData.enableRageCross,
                CrosshairSize = MenuData.CrosshairSize,
                crosshairColor = MenuData.crosshairColor,
                enableWM = MenuData.enableWM,
                wmColor = MenuData.wmColor,
                enableNightMode = MenuData.enableNightMode,
            };

            string path = Path.Combine(configDir, name + ".json");
            File.WriteAllText(path, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void Load(string name)
        {
            string path = Path.Combine(configDir, name + ".json");
            if (!File.Exists(path)) return;

            ConfigData? data = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(path));
            if (data == null) return;

            MenuData.menuColor = data.menuColor;
            MenuData.enableEsp = data.enableEsp;
            MenuData.enableBox = data.enableBox;
            MenuData.enableHealthbar = data.enableHealthbar;
            MenuData.enableNames = data.enableNames;
            MenuData.enableLines = data.enableLines;
            MenuData.boxColor = data.boxColor;
            MenuData.linesColor = data.linesColor;
            MenuData.boxMode = data.boxMode;
            MenuData.customColor = data.customColor;

            MenuData.enableCrosshair = data.enableCrosshair;
            MenuData.enableRageCross = data.enableRageCross;
            MenuData.CrosshairSize = data.CrosshairSize;
            MenuData.crosshairColor = data.crosshairColor;
            MenuData.enableWM = data.enableWM;
            MenuData.wmColor = data.wmColor;
            MenuData.enableNightMode = data.enableNightMode;
        }

        public static void ResetToDefault()
        {
            MenuData.menuColor = new Vector4(0f, 0.675f, 1f, 0.725f);
            MenuData.enableEsp = false;
            MenuData.enableBox = false;
            MenuData.enableHealthbar = false;
            MenuData.enableNames = false;
            MenuData.enableLines = false;
            MenuData.boxColor = new Vector4(1f, 1f, 1f, 0.4f);
            MenuData.linesColor = new Vector4(1f, 1f, 1f, 0.4f);
            MenuData.boxMode = 0;
            MenuData.customColor = new Vector4(1f, 1f, 1f, 0.1f);

            MenuData.enableCrosshair = false;
            MenuData.enableRageCross = false;
            MenuData.CrosshairSize = 5.0f;
            MenuData.crosshairColor = new Vector4(1f, 1f, 1f, 1f);
            MenuData.enableWM = false;
            MenuData.wmColor = new Vector4(1f, 1f, 1f, 0.2f);
            MenuData.enableNightMode = false;
        }

        public static void Delete(string name)
        {
            string path = Path.Combine(configDir, name + ".json");
            if (File.Exists(path))
                File.Delete(path);
        }

        public static List<string> GetAvailableConfigs()
        {
            if (!Directory.Exists(configDir)) return new List<string>();
            return Directory.GetFiles(configDir, "*.json").Select(Path.GetFileNameWithoutExtension).ToList();
        }
    }
}