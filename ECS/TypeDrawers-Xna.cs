using Friflo.EcGui;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;


// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace Demo;



/// <summary>Display the components of a 2D point struct <see cref="Coord2"/> in a single line.</summary>
internal sealed class PointDrawer : TypeDrawer
{
    public  override    string[]    SortFields      => ["X", "Y"];
    public  override    string[]    FormatFields    => ["X", "Y"];
    public  override    int         DefaultWidth    => 250;

    public override ItemFlags DrawValue(in DrawValue drawValue)
    {
        if (!drawValue.GetValue<Point>(out var value, out var exception)) {
            return drawValue.DrawException(exception);
        }
        if (EcUtils.InputInt2(ref value.X, ref value.Y, drawValue, out var flags)) {
            drawValue.SetValue(value);
        }
        return flags;
    }
    
    public  override void Format(MemberFormat format) {
        format.GetValue<Point>(out var value, out var exception);
        format.Append(value.X, exception);
        format.Append(value.Y, exception);
    }
}

internal sealed class Vector2Drawer : TypeDrawer
{
    public  override     int        DefaultWidth    => 300;
    public  override     string[]   SortFields      => ["X", "Y"];
    public  override     string[]   FormatFields    => ["X", "Y"];

    public override ItemFlags DrawValue(in DrawValue drawValue) {
        if (!drawValue.GetValue<Vector2>(out var value, out var exception)) {
            return drawValue.DrawException(exception);
        }
        if (EcUtils.InputFloat2(ref value.X, ref value.Y, drawValue, out var flags)) {
            drawValue.SetValue(value);
        }
        return flags;
    }
    
    public  override void Format(MemberFormat format) {
        format.GetValue<Vector2>(out var value, out var exception);
        format.Append(value.X, exception);
        format.Append(value.Y, exception);
    }
}

internal sealed class Vector3Drawer : TypeDrawer
{
    public  override     int        DefaultWidth    => 400;
    public  override     string[]   SortFields      => ["X", "Y", "Z"];
    public  override     string[]   FormatFields    => ["X", "Y", "Z"];

    public override ItemFlags DrawValue(in DrawValue drawValue) {
        if (!drawValue.GetValue<Vector3>(out var value, out var exception)) {
            return drawValue.DrawException(exception);
        }
        if (EcUtils.InputFloat3(ref value.X, ref value.Y, ref value.Z, drawValue, out var flags)) {
            drawValue.SetValue(value);
        }
        return flags;
    }
    
    public  override void Format(MemberFormat format) {
        format.GetValue<Vector3>(out var value, out var exception);
        format.Append(value.X, exception);
        format.Append(value.Y, exception);
        format.Append(value.Z, exception);
    }
}

internal sealed class Vector4Drawer : TypeDrawer
{
    public  override     int        DefaultWidth    => 500;
    public  override     string[]   SortFields      => ["X", "Y", "Z", "W"];
    public  override     string[]   FormatFields    => ["X", "Y", "Z", "W"];

    public override ItemFlags DrawValue(in DrawValue drawValue) {
        if (!drawValue.GetValue<Vector4>(out var value, out var exception)) {
            return drawValue.DrawException(exception);
        }
        if (EcUtils.InputFloat4(ref value.X, ref value.Y, ref value.Z, ref value.W, drawValue, out var flags)) {
            drawValue.SetValue(value);
        }
        return flags;
    }
    
    public  override void Format(MemberFormat format) {
        format.GetValue<Vector4>(out var value, out var exception);
        format.Append(value.X, exception);
        format.Append(value.Y, exception);
        format.Append(value.Z, exception);
        format.Append(value.W, exception);
    }
}


/// <summary>
/// Enables drawing the fields of the <see cref="Sprite"/> struct in a single line.<br/>
/// Displays the sprite image next to its components.
/// </summary>
internal sealed class SpriteDrawer : TypeDrawer
{
    private readonly ImGuiRenderer renderer;
    
    public  override    string[]    SortFields      => ["col", "row", "setId"];
    public  override    string[]    FormatFields    => ["col", "row", "setId"];
    public  override    int         DefaultWidth    => 250;
    
    internal SpriteDrawer(ImGuiRenderer renderer) {
        this.renderer = renderer;
    }

    public override ItemFlags DrawValue(in DrawValue drawValue)
    {
        if (!drawValue.GetValue<Sprite>(out var sprite, out var exception)) {
            return drawValue.DrawException(exception);
        }
        var flags = ItemFlags.None;
        
        // --- draw sprite
        var width   = drawValue.Size.Y;
        var height  = drawValue.Size.Y;
        var (errId, error) = GetTileSetTexture(sprite.setId, drawValue, renderer, out var tileSet, out nint textureId);
        if (error != null) {
            // draw error. E.g    -E  >  missing Entity
            ImGui.SetNextItemWidth(width);
            ImGui.Button(errId, new System.Numerics.Vector2(width, height));
            ImGui.SetItemTooltip(error);
            flags |= Flags();
        } else {
            var texture2D = tileSet.texture2D;
            var uv0 = new System.Numerics.Vector2((float)(sprite.col       * tileSet.spriteWidth)  / texture2D.Width,
                                  (float)(sprite.row       * tileSet.spriteHeight) / texture2D.Height);
            var uv1 = new System.Numerics.Vector2((float)((sprite.col + 1) * tileSet.spriteWidth)  / texture2D.Width,
                                  (float)((sprite.row + 1) * tileSet.spriteHeight) / texture2D.Height);
            ImGui.Image(textureId, new System.Numerics.Vector2(width, height), uv0, uv1);
        }
        
        // --- draw col, row, entity id
        ImGui.SameLine();
        drawValue.Size.X -= width + ImGui.GetStyle().ItemSpacing.X;
        if (EcUtils.InputInt3(ref sprite.col, ref sprite.row, ref sprite.setId, drawValue, out var int2Flags)) {
            drawValue.SetValue(sprite);
        }
        flags |= int2Flags;
        return flags;
    }
    
    private static (string, string) GetTileSetTexture(int entityId, in DrawValue drawValue, ImGuiRenderer renderer, out TileSet tileSet, out nint textureId)
    {
        tileSet     = default;
        textureId   = 0;
        if (!drawValue.Entity.Store.TryGetEntityById(entityId, out var tileSetEntity)) {
            return ("-E", "missing Entity");
        }
        if (!tileSetEntity.TryGetComponent(out tileSet)) {
            return ("-T", "missing TileSet component");
        }
        if (tileSet.texture2D == null) {
            return ("tn", "TileSet.texture2D is null");
        }
        textureId = renderer.BindTexture(tileSet.texture2D);
        return(null, null);
    }
    
    public  override void Format(MemberFormat format) {
        format.GetValue<Sprite>(out var value, out var exception);
        format.Append(value.col,    exception);
        format.Append(value.row,    exception);
        format.Append(value.setId,  exception);
    }
}

