# (game name TBA)
- A 2D gridlocked turn-based strategy RPG, moderately inspired off Roblox's [Noobs in Combat](https://www.roblox.com/games/5734383673/Noobs-in-Combat-v5-7). Built from scratch in C# under the MonoGame framework.
  
**Built with:**
- **MonoGame** – game framework
- **Fennecs** – lightweight ECS
- **Gum UI** – for layout and UI screens
- **Aseprite** – pixel art

**Currently completed features:**
- **Terrain-aware unit pathfinding** - ex: ground units can't traverse sea, mud tiles cost more to traverse than grass tiles, etc.
- **Data-driven loading of most gameplay elements** - keybinds, entities, textures, maps, and tiles are all loaded via JSON
- **Modular entity factory** - loads entities entirely from data, with support for custom loaders to inject dependencies and/or handle complex de/serialization processes


---
> This is an active work-in-progress project. Aiming for completion by the end of 2025.
