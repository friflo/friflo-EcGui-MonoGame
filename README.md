# friflo EcGui - MonoGame

Minimal **MonoGame** setup to showcase use and integration of **friflo ExGui**.

**friflo EcGui** can be integrated in every environment that support [**ImGui.NET**](https://github.com/ImGuiNET/ImGui.NET).

Ready-to-run Demos are available for: MonoGame, Godot, SDL3 GPU and Silk.NET.OpenGL.

## EcGui Goals

- Provide instant access to entities their components, tags and relations at runtime via **Explorer** and **Inspector** window.
- **EcGui** accelerates development speed of your ECS significant.  
  It makes the need of a debugger (Watch & Variables window) or logging in big parts obsolete.
- Integration requires only 3 simple method calls:
  ```cs
  // on startup
  EcGui.AddExplorerStore("Store", store);
  // in render loop
  EcGui.ExplorerWindow();
  EcGui.InspectorWindow();
  ```
  Additional queries and systems can be added to the **Explorer** at any time with:
  ```cs
  EcGui.AddExplorerQuery(myQuery, myQuery);
  EcGui.AddExplorerSystems(root);
  ```

- ECS data rendering, editing and interaction with the **EcGui** is instant.
- The impact on game loop performance and rendering is negligible.  
  The entire execution time (ECS data access, layout and rendering) is ~0.1 - 0.5 ms per frame on a modern dev system.  
  The execution requires no heap allocation in common cases to prevent impacting performance by GC collections.