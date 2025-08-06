# Tower Of Hubris
## Table of Contents
- [About](#about)
- [Features](#features)
- [How to Run](#how-to-run)
  + [Compiled Version](#compiled-version)
  + [Compile Yourself](#compile-yourself)
- [How to Play](#how-to-play)
  + [Level Selection](#level-selection)
  + [Level Node Types](#level-node-types)
  + [Units](#units)
  + [Abilities](#abilities)
  + [Targeting](#targeting)
  + [Combat Sequence](#combat-sequence)
  + [Power Corrosion](#power-corrosion)
  + [Restart and Exit](#restart--exit)
- [Credits](#credits)

## About
Tower of Hubris is a video game prototype inspired by titles like Limbus Company by Project Moon.

In this game, you control a team of four characters in turn-based combat, progressing level by level to ascend a tower. Each floor increases in difficulty by weakening your team while empowering enemies.

![ezgif-89a390cc5163d7](https://github.com/user-attachments/assets/ef2af337-8ed7-4e9d-9518-d7d21e9dc333)

## Features
### Turn-Based Combat
Engage in strategic turn-based battles. Assign targets and determine the action order of your units to gain the upper hand.

![ezgif-8963f85a0a8320](https://github.com/user-attachments/assets/56687122-0f50-4888-9fda-cee79b106f16)


### Procedurally Generated Level Tree & Encounters
- Levels are organized in a procedurally generated tree of interconnected nodes.
- Each node contains a random enemy encounter.
- The further you progress from the starting node, the harder the encounters become.
- Final nodes feature the most challenging enemies.

![ezgif-8cacc84901fce5](https://github.com/user-attachments/assets/3bbb733f-47ea-48b0-abbc-a34304cad3dd)

### Enemy Types
- 2 enemy types: Corrupted Security Guard and Deviant Android.
- 3 tiers per type:
  + Higher tiers have more abilities and are significantly stronger.
 
![ezgif-881ed6a4f6525d](https://github.com/user-attachments/assets/c4efe27b-e84c-4c81-9491-62fad29a8f90)
 
### 4 Unique Player Characters
- You control 4 characters.
- Each character has 3 abilities, one per tier.

### List of Abilities
- Each character has 3 tiered abilities.
- Abilities are assigned randomly each turn, with probabilities adjusted based on past rolls.
- Higher-tier abilities are more powerful and rare.

## How to Run
You have 2 options to run the project:

### Compiled Version
- Download compiled project by clicking this [LINK](https://github.com/RitoKito/TowerOfHubris/releases/download/prototype-v0.1/TowerOfHubris_windows_x86_64.zip) or from [Release Page](https://github.com/RitoKito/TowerOfHubris/releases)
- Unzip downloaded folder
- Double click TowerOfHubris.exe

### Compile Yourself
You can download the project with the source code and compile it with Godot Engine yourself.
- Download this repository by either:
  + Clicking this [LINK](https://github.com/RitoKito/TowerOfHubris/archive/refs/tags/prototype-v0.1.zip)
  + [Release Page](https://github.com/RitoKito/TowerOfHubris/releases)
  + Cloning this repository
- Unzip the downloaded file
- [Download Godot Engine](https://godotengine.org/download/windows/) Version 4.2.2 .NET or higher
  + !THIS PROJECT WILL RUN ONLY ON .NET VERSION OF GODOT AS IT WAS MADE USING C#!
- Unzip downloaded folder and run Godot Engine
- Select "Import" and selected unzipped folder
- Select Project → Export → Change Export Path to your preference → Export Project → Save
- Head to the export folder and Double click TowerOfHubris.exe

### 

## How to Play
### Level Selection
- Begin at the root node of the level tree.
- Your goal is to reach the top node and move to the next floor.
- You can only select nodes directly connected to your current position.
- There are 4 node colours:
  + Blue nodes indicate completed nodes
  + Green nodes indicate nodes available for selection.
  + Yellow nodes indicate available paths.
  + Grey nodes indicate paths that are no longer accessible.
<img width="368" height="380" alt="image" src="https://github.com/user-attachments/assets/2d1cecb5-c9eb-410e-b619-8a8962579020" />

### Level Node Types:
- Normal Nodes (Skull Icon):
  + Standard difficulty. Fewer, lower-tier enemies near the root.
- Extreme Nodes (Horned Skull Icon):
 +High difficulty. Always contain 4 enemies, often tier 2 or above.
<img width="332" height="87" alt="image" src="https://github.com/user-attachments/assets/c0edb752-3a9d-4d44-bab1-4fae06eeb8b1" />

### Combat
#### Units
Each unit has two key attributes:
- HP (Health Points): Units are defeated when HP reaches 0.
- Dmg Res (Damage Resistance): Percentage of incoming damage that is negated.
<img width="275" height="281" alt="Screenshot 2025-08-07 063810" src="https://github.com/user-attachments/assets/f9a8126c-ea72-4097-ba37-095333f99d97" />

#### Abilities
- Player units have 3 abilities (tier 1, 2, and 3).
- Enemy units only use abilities available at their tier:
  + Tier 1: 1 ability
  + Tier 2: 2 abilities (tiers 1 & 2)
  + Tier 3: All 3 abilities
Note: Higher-tier abilities deal more base damage.

- The abilities are indicated by the boxes above each unit.
- Hover over the box to read about the current unit's ability.

Each ability has:
- CR (Critical Rate): Chance of a critical hit.
- CD (Critical Damage): Bonus damage multiplier on critical hits.
<img width="242" height="200" alt="Screenshot 2025-08-07 063832" src="https://github.com/user-attachments/assets/2f4d78fa-32d3-40cd-b31e-5012062e9952" />

At the start of each round, every unit is assigned a random ability tier:
- Tier 1 is the most common.
- If a unit doesn't receive a tier 2 or 3 ability, their chance of getting one increases in the next round.

#### Targeting
- At the start of each round, enemies auto-target player units (indicated by red lines).
- To assign a player unit’s target:
  + Hover over the player unit.
  + Click and hold the Left Mouse Button.
  + Drag to an enemy unit.
  + Release the button.
  + A green line shows successful targeting.
- Units act in the order targets are assigned.
- If a unit's target is defeated before it can act, a new target is auto-assigned.
<img width="954" height="342" alt="image" src="https://github.com/user-attachments/assets/8e8e1906-b9b9-4367-9df7-479d31b589d6" />


#### Combat Sequence
- Press the "Resolve" button to begin the combat phase.
- The screen will show two black bars indicating the start of the sequence.
- Combat sequence ends when:
  + All units have acted, or
  + All enemies or player units are defeated.
- After combat sequence has ended you will be offered 3 random rewards of which you must select 1.
<img width="920" height="416" alt="image" src="https://github.com/user-attachments/assets/447e1a81-6983-401b-bf82-ea71ca30405c" />


#### Power Corrosion
- After clearing the final node of a floor, you'll ascend to a new one.
- With each new floor:
  + Player units receive debuffs to attributes and abilities.
  + Enemy units receive buffs, increasing their power.

### Restart & Exit
- Restart: Press ESC → Select Restart to begin from Floor 0.
- Exit: Press ESC → Select Exit, or simply close the game window.

## Credits
Programming and Art by Nikita Trukhin <br/>
Music by Andrey Sitkov<br/>
Sound Effects by [freesound_community](https://pixabay.com/users/freesound_community-46691455/)<br/>

This project is built with Godot Engine<br/>
https://godotengine.org/license/