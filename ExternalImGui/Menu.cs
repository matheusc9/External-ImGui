using System;
using System.Numerics;
using ImGuiNET;
using ExternalImGui;
using System.IO;
using ExternalImGui.Features;

namespace ExternalImGui
{
    public static class Menu
    {
        static string selectedConfig = "";
        static string newConfigName = "my_config";

        public static unsafe void DrawMenu()
        {

            ImGui.SetNextWindowSize(MenuData.menuSize);
            ImGui.Begin("Main Menu", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);

            // TITULO NO TOPO
            ImGui.Separator();
            CenterText("G A M E L O C K", MenuData.menuColor);

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(15, 10));

            if (ImGui.BeginTabBar("MainTabs", ImGuiTabBarFlags.None))
            {
                if (ImGui.BeginTabItem("Players"))
                {

                    // --- PLAYERS TAB ---
                    BeginChild("Players", true);

                    CustomCheckbox("Enable", ref MenuData.enableEsp);
                    CustomCheckbox("Box", ref MenuData.enableBox, ref MenuData.boxColor);
                    CustomCheckbox("Healthbar", ref MenuData.enableHealthbar);
                    CustomCheckbox("Name", ref MenuData.enableNames);
                    CustomCheckbox("Line", ref MenuData.enableLines, ref MenuData.linesColor);
                    // Custom part
                    ImGui.Spacing();
                    ImGui.TextColored(new Vector4(MenuData.menuColor[0], MenuData.menuColor[1], MenuData.menuColor[2], 0.5f), "Custom");
                    ImGui.Separator();
                    DrawColorPicker("Style Color", ref MenuData.customColor); ImGui.SameLine(); ImGui.Text("Style Color");
                    MenuData.boxMode = DrawCombo("Box Style", MenuData.boxMode, MenuData.boxModes);
                    ImGui.EndChild();

                    ImGui.SameLine(); // para ficar do lado

                    // Preview part
                    BeginChild("Preview", true);
                    ESP.DrawPreview();
                    ImGui.EndChild();

                    ImGui.EndTabItem();
                }

                // VISUALS TAB
                if (ImGui.BeginTabItem("Visuals"))
                {
                    // Crosshair
                    BeginChild("Crosshair", true);
                    CustomCheckbox("Enable", ref MenuData.enableCrosshair, ref MenuData.crosshairColor);
                    DrawSlider("Size", ref MenuData.CrosshairSize, 5.0f, 50.0f);
                    CustomCheckbox("Rage Crosshair", ref MenuData.enableRageCross);
                    ImGui.EndChild();

                    ImGui.SameLine();

                    // Others
                    BeginChild("Others", true);
                    CustomCheckbox("Draw FOV", ref MenuData.drawFovCircle, ref MenuData.drawFovCircleColor);
                    DrawSlider("Radius", ref MenuData.fovRadius, 1.0f, 15.0f);

                    CustomCheckbox("Night Mode", ref MenuData.enableNightMode);
                    CustomCheckbox("Screen Filter", ref MenuData.enableWM, ref MenuData.wmColor);
                    ImGui.EndChild();

                    ImGui.EndTabItem();
                }

                // MISCELLANEOUS TAB
                if (ImGui.BeginTabItem("Misc"))
                {
                    BeginChild("Misc", true);

                    ImGui.Text("Menu Color"); ImGui.SameLine(); DrawColorPicker("Menu Color", ref MenuData.menuColor);

                    ImGui.Spacing();
                    if (ImGui.Button("Unload"))
                        Environment.Exit(0);

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }

                // CONFIGS TAB
                if (ImGui.BeginTabItem("Config"))
                {
                    Vector2 size = new Vector2(282, 324);
                    ImGui.BeginChild("Create", size, true);
                    ImGui.TextColored(new Vector4(MenuData.menuColor[0], MenuData.menuColor[1], MenuData.menuColor[2], 0.5f), "Create");
                    ImGui.Separator();
                    ImGui.InputText("##save", ref newConfigName, 100); ImGui.SameLine(); if (ImGui.Button("Save")) { ConfigManager.Save(newConfigName); newConfigName = ""; }
                    ImGui.Spacing();
                    if (ImGui.Button("Reset to Default"))
                        ConfigManager.ResetToDefault();
                    ImGui.EndChild();

                    ImGui.SameLine();

                    BeginChild("Manager", true);
                    List<string> configs = ConfigManager.GetAvailableConfigs();
                    string[] configArray = configs.ToArray();

                    int currentConfigIndex = configs.IndexOf(selectedConfig);
                    if (currentConfigIndex < 0) currentConfigIndex = 0;

                    if (configArray.Length > 0)
                    {
                        currentConfigIndex = DrawCombo("Select", currentConfigIndex, configArray);
                        selectedConfig = configArray[currentConfigIndex];
                    }
                    else
                    {
                        selectedConfig = ""; // Nada selecionado
                        ImGui.TextDisabled("No configs available.");
                    }



                    if (ImGui.Button("Load") && !string.IsNullOrEmpty(selectedConfig))
                        ConfigManager.Load(selectedConfig);
                    ImGui.SameLine();
                    if (ImGui.Button("Delete") && !string.IsNullOrEmpty(selectedConfig))
                    {
                        ConfigManager.Delete(selectedConfig);
                        selectedConfig = "";
                    }
                    ImGui.EndChild();

                    ImGui.EndTabItem();
                }


                ImGui.EndTabBar();
            }

            ImGui.PopStyleVar();
            ImGui.End();
        }

        // --------------------------------
        // HELPERS
        // --------------------------------

        static void BeginChild(string id, bool enable)
        {
            Vector2 size = new Vector2(MenuData.menuSize.X - 318, MenuData.menuSize.Y - 76);
            ImGui.BeginChild(id, size, enable);
            ImGui.TextColored(new Vector4(MenuData.menuColor[0], MenuData.menuColor[1], MenuData.menuColor[2], 0.5f), id);
            ImGui.Separator();
        }

        // Versão sem color picker
        static bool CustomCheckbox(string label, ref bool v)
        {
            Vector4 dummyColor = default;
            return CustomCheckbox(label, ref v, false, ref dummyColor);
        }

        // Versão com color picker
        static bool CustomCheckbox(string label, ref bool v, ref Vector4 color)
        {
            return CustomCheckbox(label, ref v, true, ref color);
        }

        // Método principal privado
        private static bool CustomCheckbox(string label, ref bool v, bool hasColorPicker, ref Vector4 color)
        {
            ImGui.PushID(label);

            bool changed = false;

            // Se tiver color picker, desenha antes do texto
            if (hasColorPicker)
            {
                DrawColorPicker(label, ref color);
                ImGui.SameLine();
            }

            // Área clicável do texto (sem hover visual)
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new Vector4(0, 0, 0, 0)); // Remove hover
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new Vector4(0, 0, 0, 0)); // Remove active
            if (ImGui.Selectable(label, false, ImGuiSelectableFlags.None, new Vector2(0, ImGui.GetFrameHeight())))
            {
                v = !v;
                changed = true;
            }
            ImGui.PopStyleColor(2);

            // Alinha a checkbox no final da linha
            float availableWidth = ImGui.GetContentRegionAvail().X;
            float checkboxWidth = ImGui.GetFrameHeight() * 1.55f;

            if (availableWidth > checkboxWidth)
            {
                ImGui.SameLine(ImGui.GetCursorPosX() + availableWidth - checkboxWidth);
            }
            else
            {
                ImGui.SameLine();
            }

            Vector2 p = ImGui.GetCursorScreenPos();
            float height = ImGui.GetFrameHeight() - 4.0f;
            float width = height * 1.55f;

            Vector4 mcol = MenuData.menuColor;
            Vector4 baseColor = new Vector4(mcol[0], mcol[1], mcol[2], mcol[3]);
            Vector4 lighter = new Vector4(
                Math.Min(baseColor.X * 1.2f, 1.0f),
                Math.Min(baseColor.Y * 1.2f, 1.0f),
                Math.Min(baseColor.Z * 1.2f, 1.0f),
                baseColor.W
            );
            Vector4 darker = new Vector4(
                Math.Max(baseColor.X * 0.3f, 0.0f),
                Math.Max(baseColor.Y * 0.3f, 0.0f),
                Math.Max(baseColor.Z * 0.3f, 0.0f),
                baseColor.W
            );

            UInt32 color_bg_on = ImGui.ColorConvertFloat4ToU32(darker);
            UInt32 color_bg_off = ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1.0f));
            UInt32 color_knob = ImGui.ColorConvertFloat4ToU32(lighter);

            // Checkbox clicável - só altera se não foi alterado pelo texto
            ImGui.InvisibleButton("checkbox_clickable", new Vector2(width, height));
            if (ImGui.IsItemClicked() && !changed)
            {
                v = !v;
                changed = true;
            }

            ImDrawListPtr draw_list = ImGui.GetWindowDrawList();
            draw_list.AddRectFilled(p, new Vector2(p.X + width, p.Y + height), v ? color_bg_on : color_bg_off, height * 0.5f);

            float knob_radius = height * 0.4f;
            Vector2 knob_pos = v ? new Vector2(p.X + width - height * 0.5f, p.Y + height * 0.5f) : new Vector2(p.X + height * 0.5f, p.Y + height * 0.5f);

            draw_list.AddCircleFilled(knob_pos, knob_radius, color_knob);

            ImGui.PopID();

            return changed;
        }

        // Método DrawColorPicker ajustado
        static void DrawColorPicker(string id, ref Vector4 color)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(1, 1));

            // Botão de cor menor para ficar proporcional ao checkbox
            float buttonSize = ImGui.GetFrameHeight() - 4.0f;
            if (ImGui.ColorButton($"##btn{id}", color, ImGuiColorEditFlags.AlphaBar, new Vector2(buttonSize, buttonSize)))
                ImGui.OpenPopup($"ColorPopup{id}");

            if (ImGui.BeginPopup($"ColorPopup{id}"))
            {
                ImGui.ColorPicker4($"##picker{id}", ref color,
                    ImGuiColorEditFlags.NoInputs |
                    ImGuiColorEditFlags.AlphaBar |
                    ImGuiColorEditFlags.DisplayRGB |
                    ImGuiColorEditFlags.NoSidePreview |
                    ImGuiColorEditFlags.NoSmallPreview |
                    ImGuiColorEditFlags.NoLabel
                );
                ImGui.EndPopup();
            }
            ImGui.PopStyleVar();
        }

        static void DrawSlider(string id, ref float size, float min, float max)
        {
            ImGui.PushItemWidth(200);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(6, 1));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(4, 4));
            ImGui.SetWindowFontScale(0.9f);
            ImGui.Text(id);
            ImGui.SetWindowFontScale(1.0f);
            ImGui.PopStyleVar(); // ItemSpacing

            ImGui.SliderFloat("##" + id, ref size, min, max, "##" + id);
            ImGui.SameLine();

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
            ImGui.SetWindowFontScale(0.9f);
            ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.8f, 1f), $"{Convert.ToInt32(size)}");
            ImGui.SetWindowFontScale(1.0f);
            ImGui.PopStyleVar();

            ImGui.PopStyleVar(); // FramePadding
            ImGui.PopItemWidth();
        }

        public static int DrawCombo(string label, int currentIndex, string[] options)
        {
            if (options.Length == 0)
                return 0;

            ImGui.PushItemWidth(200);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(6, 1));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(4, 4));

            ImGui.Text(label);
            if (currentIndex < 0 || currentIndex >= options.Length)
                currentIndex = 0;

            if (ImGui.BeginCombo("##" + label, options[currentIndex]))
            {
                for (int i = 0; i < options.Length; i++)
                {
                    bool isSelected = (i == currentIndex);
                    if (ImGui.Selectable(options[i], isSelected))
                        currentIndex = i;

                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }

            ImGui.PopStyleVar();
            ImGui.PopStyleVar();
            ImGui.PopItemWidth();
            return currentIndex;
        }

        static void CenterText(string text, Vector4 color)
        {
            float windowWidth = ImGui.GetWindowSize().X;
            float textWidth = ImGui.CalcTextSize(text).X;
            ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);
            ImGui.TextColored(color, text);
        }
    }
}
