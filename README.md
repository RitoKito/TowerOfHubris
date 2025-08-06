<img width="1037" height="579" alt="image" src="https://github.com/user-attachments/assets/cb1c492f-2683-46d8-b110-81a9e8492029" /># Tower Of Hubris

## Table of Content
- [About](#about)
- [Features](#features)
- [How to Play](#how-to-play)

## About
Tower of Hubris is a video game prototype inspired by titles like Limbus Company by Project Moon.

In this game, you control a team of four characters in turn-based combat, progressing level by level to ascend a mysterious tower. Each floor increases in difficulty — weakening your team while empowering enemies — as you strive to reach the top.

## Features
### Turn-Based Combat
Engage in strategic turn-based battles. Assign targets and determine the action order of your units to gain the upper hand.

### Procedurally Generated Level Tree & Encounters
- Levels are organized in a procedurally generated tree of interconnected nodes.
- Each node contains a random enemy encounter.
- The further you progress from the starting node, the harder the encounters become.
- Final nodes feature the most challenging enemies.

### Enemy Variety
- 2 enemy types: Corrupted Security Guard and Deviant Android.
- 3 tiers per type:
  + Higher tiers have more abilities and are significantly stronger.
 
### 4 Unique Player Characters
- You control 4 characters.
- Each character has 3 abilities, one per tier.

### Diverse Abilities
- Each character has 3 tiered abilities.
- Abilities are assigned randomly each turn, with probabilities adjusted based on past rolls.
- Higher-tier abilities are more powerful and rare.

## How to Play
### Level Selection
- Begin at the root node of the level tree.
- Your goal is to reach the top node and move to the next floor.
- You can only select nodes directly connected to your current position.
- Grey nodes indicate paths that are no longer accessible.

### Node Types:
- Normal Nodes (Skull Icon):
  + Standard difficulty. Fewer, lower-tier enemies near the root.

- Extreme Nodes (Horned Skull Icon):
 +High difficulty. Always contain 4 enemies, often tier 2 or above.

### Combat
#### Units
Each unit has two key attributes:
- HP (Health Points): Units are defeated when HP reaches 0.
- Dmg Res (Damage Resistance): Percentage of incoming damage that is negated.
####Abilities
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

At the start of each round, every unit is assigned a random ability tier:
- Tier 1 is the most common.
- If a unit doesn't receive a tier 2 or 3 ability, their chance of getting one increases in the next round.

#### Targeting
- At the start of each round, enemies auto-target player units (indicated by red lines).
- To assign a player unit’s target:
- Hover over the player unit.
- Click and hold the Left Mouse Button.
- Drag to an enemy unit.
- Release the button.
- A green line shows successful targeting.
- Units act in the order targets are assigned.
- If a unit's target is defeated before it can act, a new target is auto-assigned.

#### Combat Sequence
- Press the "Resolve" button to begin the combat phase.
- The screen will show two black bars indicating the start of the sequence.
- Combat ends when:
  + All units have acted, or
  + All enemies or player units are defeated.
- Otherwise a new turn begins.

#### Power Corrosion
- After clearing the final node of a floor, you'll ascend to a new one.
- With each new floor:
  + Player units receive debuffs to attributes and abilities.
  + Enemy units receive buffs, increasing their power.

### Restart & Exit
- Restart: Press ESC → Select Restart to begin from Floor 0.
- Exit: Press ESC → Select Exit, or simply close the game window.
