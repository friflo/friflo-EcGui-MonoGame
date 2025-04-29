using System;
using System.IO;
using Demo;
using Friflo.EcGui;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;

// ReSharper disable RedundantOverriddenMember
namespace MonoGame.EcGuiLab;

public class Game1 : Game
{
    private GraphicsDeviceManager   graphics;
    private SpriteBatch             spriteBatch;
    internal ImGuiRenderer          guiRenderer;

    public Game1() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        graphics.PreferredBackBufferWidth  =  (int)(0.8f  * displayMode.Width);    // 0.5f
        graphics.PreferredBackBufferHeight =  (int)(0.8f * displayMode.Height);   // 0.47f
        
        graphics.SynchronizeWithVerticalRetrace = false;    // disable VSync
        IsFixedTimeStep = false;                            // disable VSync
        
        Window.AllowUserResizing = true;
    }

    protected override void Initialize() {
        Window.Title = "friflo EcGui - MonoGame";
        
        // --- ImGui integration
        guiRenderer = new ImGuiRenderer(this);
        var io = ImGui.GetIO();
        io.ConfigFlags |=  ImGuiConfigFlags.DockingEnable | ImGuiConfigFlags.NavEnableKeyboard | ImGuiConfigFlags.NavEnableGamepad;
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        var pixelHeight1080 = (displayMode.AspectRatio >= 1 ? displayMode.Height : displayMode.Width) / 1080f;
        io.Fonts.AddFontFromFileTTF(Path.Combine(AppContext.BaseDirectory, "Content", "Inter-Regular.ttf"), pixelHeight1080 * 20);
    //  io.Fonts.AddFontFromFileTTF(Path.Combine(AppContext.BaseDirectory, "Content", "Inter-Regular.ttf"), pixelHeight1080 * 28);
    //  io.Fonts.AddFontFromFileTTF(Path.Combine(AppContext.BaseDirectory, "Content", "Inter-Regular.ttf"), pixelHeight1080 * 15);
        io.Fonts.Build();               // optional - using custom font: Inter-Regular.ttf
    //  io.FontGlobalScale = 2f;        // Hacky way to increase font size. Fonts are rendered blurry.
        guiRenderer.RebuildFontAtlas();
        ImGui.StyleColorsLight();       // optional
        EcGui.Setup.SetDefaultStyles(); // optional
        
        Tests.Run();
        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        DemoECS.CreateEntityStore(this);    // set up your ECS here
        DemoECS.CustomizeEcGui(this);       // customize UI
        TestEcGui.TestApi();
    }

    protected override void Update(GameTime gameTime) {
        //  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        //      Keyboard.GetState().IsKeyDown(Keys.Escape))
        //      Exit();

        // TODO: Add your update logic here
        DemoECS.Update(); // call your ECS simulation here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue); // CornflowerBlue

        // TODO: Add your drawing code here

        base.Draw(gameTime);
        
        // --- ImGui integration
        guiRenderer.BeginLayout(gameTime);
        EcGui.HistorySnapshot();     // optional - required to show histories
        EcGui.ExplorerWindow();
        EcGui.InspectorWindow();
        
        // Layout.Instance.Draw();
        // ImGui.ShowDemoWindow();
        guiRenderer.EndLayout();
    }
    
    internal Texture2D LoadTexture2D(string assetName) => Content.Load<Texture2D>(assetName);
}