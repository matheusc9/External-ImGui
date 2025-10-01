using ImGuiNET;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ExternalImGui; // Program.cs

namespace ExternalImGui.Features
{
    internal class ESP
    {
        private Program _program;

        public ESP(Program program)
        {
            _program = program;
        }

        public static void DrawBox(ImDrawListPtr drawList, Vector2 topLeft, Vector2 bottomRight, Vector4 Color, Vector2 screenSize)
        {
            uint color = ImGui.ColorConvertFloat4ToU32(Color);

            // draw the box (border)
            drawList.AddRect(topLeft, bottomRight, color, 0f, 0, 1.0f);
        }

        public static void DrawBoxFill(ImDrawListPtr drawList, Vector2 topLeft, Vector2 bottomRight, Vector4 Color, Vector2 screenSize)
        {
            uint color = ImGui.ColorConvertFloat4ToU32(Color);

            // draw the fill in box (internal)
            drawList.AddRectFilled(topLeft, bottomRight, color);
        }

        public static void DrawBoxFade(ImDrawListPtr drawList, Vector2 topLeft, Vector2 bottomRight, Vector4 baseColor, uint fadeColor, Vector2 screenSize)
        {
            uint baseC = ImGui.ColorConvertFloat4ToU32(baseColor);

            drawList.AddRectFilledMultiColor(topLeft, bottomRight, fadeColor, fadeColor, baseC, baseC);
        }

        public static void DrawName(ImDrawListPtr drawList, Vector2 topLeft, Vector2 bottomRight, string name, uint color)
        {
            uint nameColor = color;
            Vector2 textSize = ImGui.CalcTextSize(name);
            Vector2 namePos = new Vector2((topLeft.X + bottomRight.X) / 2 - textSize.X / 2, topLeft.Y - textSize.Y - 2);
            drawList.AddText(namePos, nameColor, name);
        }

        public static void DrawHealthbar(ImDrawListPtr drawList, Vector2 topLeft, Vector2 bottomRight, float health)
        {
            float barWidth = 1f;
            float offsetX = 8f;
            float paddingY = 1f; // padding vertical
            float paddingX = 1f; // padding horizontal

            // Fundo maior, com padding em todos os lados
            Vector2 bgTopLeft = new Vector2(topLeft.X - offsetX - paddingX, topLeft.Y - paddingY);
            Vector2 bgBottomRight = new Vector2(topLeft.X - offsetX + barWidth + paddingX, bottomRight.Y + paddingY);

            // Altura disponível para a barra dentro do fundo (diminuída pelo padding superior e inferior)
            float availableHeight = (bottomRight.Y - topLeft.Y) + 2 * paddingY;

            // Altura da barra proporcional à vida, limitada ao espaço disponível (sem "crescer" para além do fundo)
            float healthHeight = availableHeight * Math.Clamp(health, 0f, 1f);

            // A barra começa em bgBottomRight.Y (fundo) e sobe healthHeight para cima
            Vector2 hpTop = new Vector2(bgTopLeft.X + paddingX, bgBottomRight.Y - healthHeight - paddingY);
            Vector2 hpBottom = new Vector2(bgBottomRight.X - paddingX, bgBottomRight.Y - paddingY);

            // Cores
            Vector4 bgColor = new Vector4(0.3f, 0.3f, 0.3f, 1f);
            Vector4 hpColor = health > 0.66f ? new Vector4(0f, 1f, 0f, 1f) :
                               health > 0.33f ? new Vector4(1f, 1f, 0f, 1f) :
                                                new Vector4(1f, 0f, 0f, 1f);

            uint bgColorU32 = ImGui.ColorConvertFloat4ToU32(bgColor);
            uint hpColorU32 = ImGui.ColorConvertFloat4ToU32(hpColor);

            // Desenha fundo e barra
            drawList.AddRectFilled(bgTopLeft, bgBottomRight, bgColorU32);
            drawList.AddRectFilled(hpTop, hpBottom, hpColorU32);
        }

        public static void DrawLines(ImDrawListPtr drawList, Vector2 topLeft, Vector2 bottomRight, Vector2 screenSize, Vector4 colorV4)
        {
            uint color = ImGui.ColorConvertFloat4ToU32(colorV4);
            drawList.AddLine(new Vector2(topLeft.X + 50, bottomRight.Y), new Vector2(screenSize.X / 2, screenSize.Y), color);
        }

        public static void DrawPreview()
        {
            var drawList = ImGui.GetWindowDrawList();
            Vector2 canvasPos = ImGui.GetCursorScreenPos(); // posição absoluta
            Vector2 canvasSize = ImGui.GetContentRegionAvail(); // tamanho disponível
            Vector2 boxSize = new Vector2(100, 200); // ou 900-800, 600-400

            // Centraliza dentro do preview area (child)
            Vector2 topLeft = new Vector2(
                canvasPos.X + (canvasSize.X - boxSize.X) / 2f,
                canvasPos.Y + (canvasSize.Y - boxSize.Y) / 2f
            );
            Vector2 bottomRight = topLeft + boxSize;

            drawList.AddRectFilled(canvasPos, canvasPos + canvasSize, ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1f))); // background

            if (MenuData.enableBox)
                ESP.DrawBox(drawList, topLeft, bottomRight, MenuData.boxColor, MenuData.screenSize);

            if (MenuData.boxMode == 1 && MenuData.enableBox)
                ESP.DrawBoxFill(drawList, topLeft, bottomRight, MenuData.customColor, MenuData.screenSize);

            if (MenuData.boxMode == 2 && MenuData.enableBox)
                ESP.DrawBoxFade(drawList, topLeft, bottomRight, MenuData.customColor, MenuData.fadeColor, MenuData.screenSize);

            if (MenuData.enableNames)
                ESP.DrawName(drawList, topLeft, bottomRight, "Player", MenuData.whiteColor);

            if (MenuData.enableHealthbar)
                ESP.DrawHealthbar(drawList, topLeft, bottomRight, 1f);

            if (MenuData.enableLines)
                ESP.DrawLines(drawList, topLeft, bottomRight, MenuData.screenSize, MenuData.linesColor);
        }
    }
}
