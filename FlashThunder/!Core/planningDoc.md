# 1) Project Statement
A turn-based military strategy game based on a fight for what is left of the resources
on the planet, following the destruction of most major infrastructure and governments 
post-nuclear war. Somewhat arcadey and based off of Noobs in Combat on ROBLOX.

# 2) Roadmap
## Requirements (MVP)
- Game can progress in turns by the player ending their turn
- Player can select, move, and order units to attack enemies
- Units can only cross tiles where they are accessible by their movement limitations
  (water, air, land)
- Units can be spawned from world buildings
## Post-MVP
- UI expansion to show game state
- Most/all development capable of data-driven addition
- Enemy AI based off utility system

- Pixel art to go with units
- Full storyline + implementation of a campaign

# 3) Tasks

# 4) Scheduling
- About 15-20hrs per week this summer. Expecting MVP to be completed by the end of July
  (Will take a break to learn webdev after that)
- Once i'm back at uni this will probably be my weekend project.

# 5) Extra
## Decisions
- Switched from DefaultECS to fennecs for better documentation and community support.
- "One-shot systems" should be world commands, not systems. (Scrapped request entities)
   ^ Followup: Changed back to one-shot systems because commands can't be easily done w/o
     providing dependencies from the caller. Renamed to "handlers" and will only listen to
	 events.
- World extensions:
    - Resource setter/getter (like defaultECS)
	- Event bus (like defaultECS)
	- 
## Architecture
- Using MonoGame framework to get more experience building stuff
- Using fennecs for game ECS
- Using Gum for user interface, following the Model-View-Presenter pattern
	- Using an event bus for communication between ECS world and UI, rather than exposing the 
	  world as the model