using System.Numerics;
using ImGuiNET;

// ReSharper disable once CheckNamespace
namespace MonoGame.EcGuiLab;

internal class Layout
{
    private float labelWidth = 250;
    
    private bool expandComponents;
    private bool expand1;
    private bool expand2;
    private bool expand1A;
    private bool expand1B;
    
    private string text1 = "text1";
    private string text2 = "text2";
    private string text3 = "text3";
    private string text4 = "text4";
    private string text5 = "text5";
    
    private float indentWidth;
    private bool  checkbox;
    
    internal static readonly Layout Instance = new Layout();
    
    private static void DrawWidgetBg(Vector4 bg, float width)
    {
        var drawList        = ImGui.GetWindowDrawList();
        var itemSpacingY    = ImGui.GetStyle().ItemSpacing.Y;
        var p0              = ImGui.GetWindowPos() + ImGui.GetCursorPos() + new Vector2(-ImGui.GetScrollX(), -ImGui.GetScrollY() - itemSpacingY / 2);
        var textHeight      = ImGui.GetFrameHeight() + itemSpacingY;
        var p1              = p0 + new Vector2(width, textHeight);
        drawList.AddRectFilled(p0, p1, ImGui.GetColorU32(bg));
    }
    
    private const ImGuiTreeNodeFlags TreeFlags = ImGuiTreeNodeFlags.FramePadding | ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.NavLeftJumpsBackHere;
    
    internal void Draw()
    {
        ImGui.SetNextWindowPos(new(1700, 10),  ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSize(new(500, 380), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowBgAlpha(1);
        indentWidth = ImGui.GetStyle().IndentSpacing;
        // var unindent = ImGui.GetTreeNodeToLabelSpacing();
        if (ImGui.Begin("Test Layout"))
        {
            DrawWidgetBg(new Vector4(1f,0.5f,1f,1), 300);
            ImGui.Checkbox("Enabled", ref checkbox);
            
            ImGui.SliderFloat("##label-width",    ref labelWidth, 150, 500, null, ImGuiSliderFlags.AlwaysClamp);
 
            expandComponents = ImGui.TreeNodeEx("components", TreeFlags);
            
            if (expandComponents) {
                ImGui.Unindent(indentWidth);
                DrawNode    ("Component 1", 1);
                DrawLeafComponent("Component 2", 2);
                DrawNode    ("Component 3", 3);
                ImGui.Indent(indentWidth);
                ImGui.TreePop();
            }
        }
        ImGui.End();
    }
    
    private void DrawNode(string component, int id)
    {
        ImGui.PushID(id);
        expand1 = ImGui.TreeNodeEx(component, TreeFlags);
        // var unindent = ImGui.GetTreeNodeToLabelSpacing();
        if (expand1) {
            ImGui.Unindent(indentWidth);
            if (ImGui.TreeNodeEx("leaf 1", TreeFlags | ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.AllowOverlap)) {
                ImGui.SameLine(labelWidth);
                ImGui.InputText("##text1", ref text1, 100);
                ImGui.TreePop();
            }
                
            expand1A = ImGui.TreeNodeEx("object 1 a", TreeFlags );
            if (expand1A) {
                if (ImGui.TreeNodeEx("leaf 1", TreeFlags | ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.AllowOverlap)) {
                    ImGui.SameLine(labelWidth);
                    ImGui.InputText("##text2", ref text2, 100);
                    ImGui.TreePop();
                }
                ImGui.TreePop();    
            }
            if (ImGui.TreeNodeEx("leaf 2", TreeFlags | ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.AllowOverlap)) {
                ImGui.SameLine(labelWidth);
                ImGui.InputText("##text3", ref text3, 100);
                ImGui.TreePop();
            }
            expand1B = ImGui.TreeNodeEx("object 1 b", TreeFlags);
            if (expand1B) {
                if (ImGui.TreeNodeEx("leaf 4", TreeFlags | ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.AllowOverlap)) {
                    ImGui.SameLine(labelWidth);
                    ImGui.InputText("##text4", ref text4, 100);
                    ImGui.TreePop();
                }
                ImGui.TreePop();    
            }
            ImGui.Indent(indentWidth);
            ImGui.TreePop();
        }
        ImGui.PopID();
    }
    
    private void DrawLeafComponent(string component, int id)
    {
        ImGui.PushID(id);
        expand2 = ImGui.TreeNodeEx(component, TreeFlags | ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.AllowOverlap);
        if (expand2) {
            ImGui.SameLine(labelWidth);
            ImGui.InputText("##text5", ref text5, 100);
            ImGui.TreePop();
        }
        ImGui.PopID();
    }
}