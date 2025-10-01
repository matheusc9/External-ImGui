using System.Numerics;
using System.Collections.Concurrent;
using ImGuiNET;

namespace ExternalImGui
{
    public static class MenuData
    {
        // -------- Menu --------
        public static Vector2 menuSize = new Vector2(600, 400);
        public static Vector4 menuColor = new Vector4(0f, 0.675f, 1f, 0.725f);

        // -------- Players --------
        public static bool enableEsp = false;
        public static bool enableBox = false;
        public static bool enableHealthbar = false;
        public static bool enableNames = false;
        public static bool enableLines = false;
        public static Vector4 boxColor = new Vector4(1f, 1f, 1f, 0.4f);
        public static Vector4 linesColor = new Vector4(1f, 1f, 1f, 0.4f);
        // custom box
        public static int boxMode = 0; // 0 = Desativado, 1 = Box Fill, 2 = Box Fade
        public static string[] boxModes = { "Default", "Box Fill", "Box Fade" };
        public static Vector4 customColor = new Vector4(1f, 1f, 1f, 0.1f);

        // -------- VISUALS --------
        // crosshair
        public static bool enableCrosshair = false;
        public static bool enableRageCross = false;
        public static float CrosshairSize = 5.0f;
        public static Vector4 crosshairColor = new Vector4(1f, 1f, 1f, 1f);
        // fov circle
        public static bool drawFovCircle = false;
        public static float fovRadius = 1.0f;
        public static Vector4 drawFovCircleColor = new Vector4(1f, 1f, 1f, 0.4f);
        // world modulation
        public static bool enableWM = false;
        public static Vector4 wmColor = new Vector4(1f, 1f, 1f, 0.2f);
        // night mode
        public static bool enableNightMode = false;

        // -------- MISC --------

        // -------- UI / MENU --------
        public static int currentTab = 0;
        public static bool showWindow = true;
        public static bool enableOverlay = true;
        public static bool keyWasPressed = false;

        // -------- POSIÇÕES / TAMANHO --------
        public static Vector2 screenSize = new Vector2(1920, 1080);
        public static Vector2 drawPosition = new Vector2(1920 / 2, 1080 / 2);
        public static Vector2 cantoSuperiorDireito = new Vector2(1820, 10);

        // -------- ENTIDADES --------
        //public static ConcurrentQueue<Entity> entities = new ConcurrentQueue<Entity>();
        //public static Entity localPlayer = new Entity();
        public static readonly object entityLock = new object();

        // -------- CORES --------
        public static readonly uint redColor = ImGui.ColorConvertFloat4ToU32(new Vector4(1, 0, 0, 1));
        public static readonly uint greenColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0, 1, 0, 1));
        public static readonly uint blueColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 1, 1));
        public static readonly uint whiteColor = ImGui.ColorConvertFloat4ToU32(new Vector4(1f, 1f, 1f, 1f));
        public static readonly uint blackColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0f, 0f, 0f, 1f));
        public static readonly uint purpleColor = ImGui.ColorConvertFloat4ToU32(new Vector4(128f, 0f, 128f, 1f));
        public static readonly uint fadeColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0f, 0f, 0f, 0.05f));
    }
}
