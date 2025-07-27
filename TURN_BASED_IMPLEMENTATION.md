# Turn-Based Logic Implementation

This document describes the turn-based logic implementation for Flash Thunder.

## Overview

The turn-based system has been implemented with the following key components:

### Core Components

1. **TurnRange Component** (`FlashThunder.GameLogic.Components.Turn.TurnRange`)
   - Tracks available actions for units during their turn
   - Properties:
     - `MaxMoves` / `MovesRemaining`: Movement actions per turn
     - `MaxActions` / `ActionsRemaining`: Attack/ability actions per turn
     - `HasMoved` / `HasAttacked`: Tracks what actions have been used
     - `CanMove` / `CanAttack` / `CanAct`: Helper properties for validation

2. **TurnDisplayInfo Component** (`FlashThunder.GameLogic.Turn.Components.TurnDisplayInfo`)
   - Stores turn information for UI display
   - Tracks current team name, round number, and if it's the player's turn

### Systems

1. **TurnActionResetSystem**
   - Automatically resets all unit actions when a new turn begins
   - Monitors for changes in the current team and resets `TurnRange` components

2. **TurnBasedMovementSystem** 
   - Validates movement actions against turn constraints
   - Prevents units from moving multiple times per turn
   - Tracks movement usage in `TurnRange` component

3. **TurnBasedAttackSystem**
   - Validates attack actions against turn constraints  
   - Prevents units from attacking multiple times per turn
   - Tracks action usage in `TurnRange` component

4. **EndTurnInputSystem**
   - Handles player input to end their turn (M key)
   - Publishes `NextTurnRequest` events when triggered

5. **TurnDisplayUpdateSystem**
   - Updates turn display information for the UI
   - Maintains `TurnDisplayInfo` resource with current game state

### Existing Infrastructure Enhanced

1. **NextTurnHandler** (Enhanced)
   - Now properly increments round numbers when cycling back to first team
   - Maintains turn order and team transitions

2. **TurnOrderResource** (Existing)
   - Manages which team's turn it is
   - Tracks turn order and current team index

## Game Flow

1. **Turn Start**: When a new turn begins, `TurnActionResetSystem` resets all units' action points
2. **Player Actions**: During their turn, players can:
   - Move units (limited by `TurnRange.MaxMoves`)
   - Attack with units (limited by `TurnRange.MaxActions`) 
   - End their turn manually (M key)
3. **Action Validation**: Systems validate each action against remaining action points
4. **Turn End**: When the player ends their turn, the system cycles to the next team
5. **Round Progression**: After all teams have had their turn, the round number increments

## Key Features

- **Action Point System**: Units have limited moves and actions per turn
- **Turn Validation**: Systems prevent units from exceeding their action limits
- **Automatic Reset**: Actions are automatically restored at the start of each turn
- **Player Control**: Players can manually end their turn using the M key
- **Round Tracking**: The system tracks round numbers as the game progresses
- **UI Integration**: Turn information is available for UI display

## Configuration

- Units are initialized with `TurnRange(maxMoves: 1, maxActions: 1)` by default
- The EndTurn action is bound to the 'M' key in `keybinds.json`
- Systems are integrated into the main game loop via `GameRunningStateFactory`

## Testing

The core turn logic has been validated with unit tests covering:
- Initial action point allocation
- Action usage and tracking  
- Turn reset functionality
- Action limit enforcement

This implementation provides a solid foundation for turn-based strategy gameplay with proper action management and turn progression.