using System;
using Demo;
using Friflo.EcGui;
using Friflo.Engine.ECS;

// ReSharper disable once CheckNamespace
namespace MonoGame.EcGuiLab;

internal static class TestEcGui
{
    internal static void TestApi()
    {
        // Test: adding multiple OnMemberChanged<> handlers 
        OnMemberChanged<TileSet> testHandler1 = (ref TileSet _, Entity _, string _, in TileSet _) => throw new InvalidOperationException("unexpected");
        OnMemberChanged<TileSet> testHandler2 = (ref TileSet _, Entity _, string _, in TileSet _) => throw new InvalidOperationException("unexpected");
        
        EcGui.Setup.AddOnMemberChanged      (testHandler1);
        EcGui.Setup.AddOnMemberChanged      (testHandler2);
        
        EcGui.Setup.RemoveOnMemberChanged   (testHandler1);
        EcGui.Setup.RemoveOnMemberChanged   (testHandler2);
    }
}