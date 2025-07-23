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

## Post-MVP (high - low prio.)
- UI expansion to show game state
- Most/all development capable of data-driven addition
- Enemy AI based off utility system

- Pixel art to go with units
- Full storyline + implementation of a campaign
- UI animations and transitions

# 3) Active Tasks
- Turn limited actions + turn system


- Unit collisions

# 4) Scheduling
- About 15-20hrs per week this summer. Expecting MVP to be completed by the end of July
  (Will take a break to learn webdev after that)
- Once i'm back at uni this will probably be my weekend project.

# 5) Extra
- good references and stuff:
https://github.com/JohnnyTurbo
https://fennecs.tech
https://bevy-cheatbook.github.io

## Decisions
- Switched from DefaultECS to fennecs for better documentation and community support.
- "One-shot systems" should be world commands, not systems. (Scrapped request entities)
   ^ Followup: Changed back to one-shot systems because commands can't be easily done w/o
     providing dependencies from the caller. Renamed to "handlers" and will only listen to
	 events.
- World extensions:
    - Resource setter/getter (like defaultECS)
	- Event bus (like defaultECS)
- Rather than systems, services should be used to handle resources. This lets us reuse logic
  about resource usage rather than retrieving it and having to manually get stuff out of it
  every time we want to do something with it.
	- Will avoid service locators for now though. Looking into services as their own resource
	  or abandoning resources altogether for services.
- Very highly considering using Tiled for tilemap building. This seems very conventional and better
  than putting characters in an array x_x


### On Attacks
- UnitSkill: The deserialized result of an entity attack. The info is used to stage attacks and update the UI.
- AttackBehavior: The strategy used to perform an attack. should not be specific, rather encompassing
  of a category of attacks
- AttackParams: the information about an attack SPECIFIC to the attackbehavior
- AttackData: the container created through the ECS world to signal that an attack is to be executed

Entity has a Skills component that contains a list of UnitSkills
Entity can add a AttackRequest component to itself for AttackSystem to process.




## Architecture
- Using MonoGame framework to get more experience building stuff
- Using fennecs for game ECS
- Using Gum for user interface, following the Model-View-Presenter pattern
	- Using an event bus for communication between ECS world and UI, rather than exposing the 
	  world as the model

- SYSTEM: A per-frame updater that manipulates data. Handles most of the game logic.
- HANDLER: A listener for events that happen in the world. Systems fire events for handlers to
- act on.
- SERVICE: A helper to make common actions less repetitive. Usually for information 
  retrieval that would otherwise be awkward in pure ECS.
- RESOURCE: A singleton entity that can be retrieved anywhere from the world. Generally
  should not NEED to be touched by services (otherwise, they would just be inside that service)

- Handling attacks with 