using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExternalImGui.Features
{
    public class Crosshair
    {
        public static void Draw(ImDrawListPtr drawList, Vector2 center, float size, Vector4 color)
        {
            uint colorU32 = ImGui.ColorConvertFloat4ToU32(color);
            drawList.AddLine(new Vector2(center.X - size, center.Y), new Vector2(center.X + size, center.Y), colorU32, 1.0f);
            drawList.AddLine(new Vector2(center.X, center.Y - size), new Vector2(center.X, center.Y + size), colorU32, 1.0f);
        }
    }
}
