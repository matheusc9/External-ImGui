using System.Numerics;
using ImGuiNET;

namespace ExternalImGui
{
    public static class ImGuiStyle
    {
        public static void Apply()
        {
            ImGuiStylePtr style = ImGui.GetStyle();
            var colors = style.Colors;

            // CONFIGURAÇÕES GERAIS
            // BORDER RADIUS
            style.WindowRounding = 4.0f;
            style.FrameRounding = 2.0f;
            style.GrabRounding = 2.0f;
            style.ScrollbarRounding = 5.0f;
            style.TabRounding = 0f;

            style.WindowPadding = new Vector2(10, 10);
            style.FramePadding = new Vector2(6, 4);
            style.ItemSpacing = new Vector2(6, 6);
            style.ItemInnerSpacing = new Vector2(10, 10);

            // CORES BÁSICAS
            var white = new Vector4(1f, 1f, 1f, 1f);
            var grey = new Vector4(0.3f, 0.3f, 0.3f, 1f);
            var background = new Vector4(0.05f, 0.05f, 0.06f, 1f);
            var childBg = new Vector4(0.07f, 0.07f, 0.08f, 1f);
            var border = new Vector4(0.15f, 0.15f, 0.15f, 1f);
            var tabColor = new Vector4(0.12f, 0.12f, 0.15f, 1f);
            var mainAccentColor = MenuData.menuColor; // corrigido para range 0-1

            // TEXTOS
            colors[(int)ImGuiCol.Text] = white;
            colors[(int)ImGuiCol.TextDisabled] = grey;

            // BACKGROUND / BORDAS
            colors[(int)ImGuiCol.WindowBg] = background;
            colors[(int)ImGuiCol.ChildBg] = childBg;
            colors[(int)ImGuiCol.PopupBg] = background;
            colors[(int)ImGuiCol.Border] = border;

            // INPUTS
            colors[(int)ImGuiCol.FrameBg] = tabColor;
            colors[(int)ImGuiCol.FrameBgHovered] = mainAccentColor;
            colors[(int)ImGuiCol.FrameBgActive] = mainAccentColor;
            colors[(int)ImGuiCol.CheckMark] = white;

            // BOTÕES
            colors[(int)ImGuiCol.Button] = tabColor;
            colors[(int)ImGuiCol.ButtonHovered] = mainAccentColor;
            colors[(int)ImGuiCol.ButtonActive] = mainAccentColor;

            // TÍTULOS
            colors[(int)ImGuiCol.TitleBg] = background;
            colors[(int)ImGuiCol.TitleBgActive] = background;
            colors[(int)ImGuiCol.TitleBgCollapsed] = background;

            // TABS
            colors[(int)ImGuiCol.Tab] = tabColor;
            colors[(int)ImGuiCol.TabHovered] = mainAccentColor;
            colors[(int)ImGuiCol.TabActive] = mainAccentColor;
            colors[(int)ImGuiCol.TabUnfocused] = tabColor;
            colors[(int)ImGuiCol.TabUnfocusedActive] = mainAccentColor;

            // HEADERS
            colors[(int)ImGuiCol.Header] = tabColor;
            colors[(int)ImGuiCol.HeaderHovered] = mainAccentColor;
            colors[(int)ImGuiCol.HeaderActive] = mainAccentColor;

            // SLIDERS
            colors[(int)ImGuiCol.SliderGrab] = white;
            colors[(int)ImGuiCol.SliderGrabActive] = white;

            // SEPARADORES
            colors[(int)ImGuiCol.Separator] = border;
            colors[(int)ImGuiCol.SeparatorHovered] = mainAccentColor;
            colors[(int)ImGuiCol.SeparatorActive] = mainAccentColor;

            // SCROLLBARS / GRIPS
            colors[(int)ImGuiCol.ScrollbarBg] = childBg;
            colors[(int)ImGuiCol.ScrollbarGrab] = tabColor;
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = mainAccentColor;
            colors[(int)ImGuiCol.ScrollbarGrabActive] = mainAccentColor;

            colors[(int)ImGuiCol.ResizeGrip] = new Vector4(1, 1, 1, 0.1f);
            colors[(int)ImGuiCol.ResizeGripHovered] = mainAccentColor;
            colors[(int)ImGuiCol.ResizeGripActive] = mainAccentColor;
        }
    }
}
