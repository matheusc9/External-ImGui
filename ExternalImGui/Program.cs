using ClickableTransparentOverlay;
using ExternalImGui;
using ExternalImGui.Features; // Features aqui
using ImGuiNET;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using Veldrid;
using static System.Net.Mime.MediaTypeNames;

namespace ExternalImGui
{
    public class Program : Overlay
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        //
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        //private string targetProcName = "cs2";
        private string targetProcName = "OAR-Win64-Shipping";

        public static string GetActiveProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out uint pid);
            try
            {
                var proc = System.Diagnostics.Process.GetProcessById((int)pid);
                return proc.ProcessName; // sem extensão
            }
            catch
            {
                return null;
            }
        }

        protected override void Render()
        {
            string activeProcess = GetActiveProcessName();
            string currentProcess = Process.GetCurrentProcess().ProcessName;

            bool isTarget = activeProcess != null &&
                (activeProcess.Equals(targetProcName, StringComparison.OrdinalIgnoreCase) ||
                 activeProcess.Equals(currentProcess, StringComparison.OrdinalIgnoreCase));

            if (!isTarget)
                return;


            if (GetAsyncKeyState(0x2D) < 0)
            {
                if (!MenuData.keyWasPressed)
                {
                    MenuData.showWindow = !MenuData.showWindow;
                    MenuData.keyWasPressed = true;
                }
            }
            else
            {
                MenuData.keyWasPressed = false;
            }

            if (MenuData.showWindow)
            {
                ImGuiStyle.Apply();
                Menu.DrawMenu();
            }

            DrawOverlay();
        }


        void DrawOverlay()
        {
            if (!MenuData.enableOverlay) return;

            ImGui.SetNextWindowSize(MenuData.screenSize);
            ImGui.SetNextWindowPos(Vector2.Zero);

            ImGui.Begin("Overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollWithMouse
            );

            ImDrawListPtr drawList = ImGui.GetWindowDrawList();

            // watermark
            DrawWatermark(drawList);

            // -------- Players --------
            // esp
            if (MenuData.enableEsp)
            {
                foreach (var entity in FakeEntityList())
                {
                    if (MenuData.enableBox)
                        ESP.DrawBox(drawList, entity.PosTopLeft, entity.PosBottomRight, MenuData.boxColor, MenuData.screenSize);

                    if (MenuData.boxMode == 1 && MenuData.enableBox)
                        ESP.DrawBoxFill(drawList, entity.PosTopLeft, entity.PosBottomRight, MenuData.customColor, MenuData.screenSize);

                    if (MenuData.boxMode == 2 && MenuData.enableBox)
                        ESP.DrawBoxFade(drawList, entity.PosTopLeft, entity.PosBottomRight, MenuData.customColor, MenuData.fadeColor, MenuData.screenSize);

                    if (MenuData.enableNames)
                        ESP.DrawName(drawList, entity.PosTopLeft, entity.PosBottomRight, entity.Name, MenuData.whiteColor);

                    if (MenuData.enableHealthbar)
                        ESP.DrawHealthbar(drawList, entity.PosTopLeft, entity.PosBottomRight, entity.Health);

                    if (MenuData.enableLines)
                        ESP.DrawLines(drawList, entity.PosTopLeft, entity.PosBottomRight, MenuData.screenSize, MenuData.linesColor);
                }
            }

            // -------- VISUALS --------
            // crosshair
            if (MenuData.enableCrosshair)
                Crosshair.Draw(drawList, MenuData.drawPosition, MenuData.CrosshairSize, MenuData.crosshairColor);
            if (MenuData.enableRageCross)
            {
                Crosshair.Draw(drawList, MenuData.drawPosition, 970, new Vector4(0f, 0f, 0f, 0.40f));
                Crosshair.Draw(drawList, MenuData.drawPosition, 5f, new Vector4(1f, 1f, 1f, 0.40f));
                DrawFovCircle(drawList, 1.5f, new Vector4(0f, 0f, 0f, 0.1f));
            }
            // world modulation
            if (MenuData.enableWM)
            {
                Vector4 c = MenuData.wmColor;
                uint topLeft = ImGui.ColorConvertFloat4ToU32(c);
                uint topRight = ImGui.ColorConvertFloat4ToU32(new Vector4(c.X, c.Y, c.Z, c.W * 0.8f));
                uint bottomLeft = ImGui.ColorConvertFloat4ToU32(new Vector4(c.X, c.Y, c.Z, c.W * 0.8f));
                uint bottomRight = ImGui.ColorConvertFloat4ToU32(new Vector4(c.X, c.Y, c.Z, c.W * 0.6f));

                drawList.AddRectFilledMultiColor(
                    new Vector2(0, 0),
                    MenuData.screenSize,
                    topLeft, topRight, bottomRight, bottomLeft
                );
            }
            if (MenuData.enableNightMode)
            {
                Vector4 c = new Vector4(0f, 0f, 0f, 0.5f);
                uint topLeft = ImGui.ColorConvertFloat4ToU32(c);
                uint topRight = ImGui.ColorConvertFloat4ToU32(new Vector4(c.X, c.Y, c.Z, c.W * 0.8f));
                uint bottomLeft = ImGui.ColorConvertFloat4ToU32(new Vector4(c.X, c.Y, c.Z, c.W * 0.8f));
                uint bottomRight = ImGui.ColorConvertFloat4ToU32(new Vector4(c.X, c.Y, c.Z, c.W * 0.6f));

                drawList.AddRectFilledMultiColor(
                    new Vector2(0, 0),
                    MenuData.screenSize,
                    topLeft, topRight, bottomRight, bottomLeft
                );
            }


            // -------- AIMBOT --------
            // fov
            if (MenuData.drawFovCircle)
                DrawFovCircle(drawList, MenuData.fovRadius, MenuData.drawFovCircleColor);

            ImGui.End();
        }

        void DrawFovCircle(ImDrawListPtr drawList, float Radius, Vector4 Color)
        {
            Vector2 center = new Vector2(MenuData.screenSize.X / 2, MenuData.screenSize.Y / 2);
            float radius = Radius * 10f;
            uint color = ImGui.ColorConvertFloat4ToU32(Color);

            drawList.AddCircle(
                center,
                radius,
                color,
                64,
                1.5f
            );
        }

        void DrawWatermark(ImDrawListPtr drawList)
        {
            ImGui.SetCursorPosX(MenuData.screenSize.X - 100);
            ImGui.TextColored(MenuData.menuColor, "gamelock.gg");
            drawList.AddRectFilled(new Vector2(1100, 1600), new Vector2(1100, 1100), ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 0, 1)));
        }

        IEnumerable<dynamic> FakeEntityList()
        {
            return new[]
            {
                new { Name = "Enemy1", PosTopLeft = new Vector2(800, 400), PosBottomRight = new Vector2(900, 600), Health = 1.0f },
                //new { Name = "Enemy2", PosTopLeft = new Vector2(950, 450), PosBottomRight = new Vector2(1050, 650), Health = 0.5f },
                //new { Name = "Enemy3", PosTopLeft = new Vector2(1100, 500), PosBottomRight = new Vector2(1200, 700), Health = 0.1f },
            };
        }


        public static void Main(string[] args)
        {
            Program program = new Program();
            program.Start().Wait();
        }
    }
}
