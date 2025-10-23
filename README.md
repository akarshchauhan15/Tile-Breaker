# Tile Breaker

## Quickfire Instructions
- Download the latest verison of Tile Breaker from releases page.
- The game is portable, hence, no installation needed, just open and begin playing.
- Click anywhere or press Enter to start.
- Read or skip the initial guide provided.
- Hover mouse cursor on top right corner for the retry option to appear.
- Hover mouse cursor on top left corner for the settings option to appear.
- Hover mouse cursor on bottom left corner for the high scores menu to appear.

## Images
![Start](https://github.com/user-attachments/assets/dbd93f9c-1fef-476e-8f89-46fcf74ba4d6)
![Playing](https://github.com/user-attachments/assets/24c2f59a-d882-4e6f-ba28-6f0763626dd6)
![Hovered](https://github.com/user-attachments/assets/c77284cb-02b9-47b3-8194-539567f64fdc)

# Game Design Document

## Introduction

### Summary
Tile Breaker is a retro style arcade like game based on Breakout (1976) by Atari with features and graphical improvements.

### Inspiration
**Breakout** by Atari

### Platform
Windows and Linux

### Genre
Arcade and Retro

### Target Audience
People who want to experience a Breakout like game again.

### Development Software
- **Godot 4** for game engine.
- **Visual Studio** for IDE.
- **Inkscape** for vector graphics.

## Concept

### Gameplay overview
The game consists of a grid of tiles, a primary striker and a paddle. The player moves the paddle to bounce off the striker and make them collide with a tile. On collsion, a tile is destroyed and may initiate effects or summon a pickable drop.

Game declares victory if all the tiles are destroyed, but, if all the strikers fall below the screen, the player loses. Activities like destroying tiles and collecting drops contribute to total score.

### Navigating UI
- The game can be restarted by hovering the mouse near top-right corner and clicking twice.
- The settings panel will appear if the mouse is hovered at top-left corner.
- To view the previous high scores, the user has to hover the mouse hear botton-left corner.

## Game Experience

### UI
Unlike the original Breakout, this game follows high resolution rendering, instead of pixelated one.
The game features proximity panels, that appear when cursor is hovered near corners of the window, inspired by Windows 8.

### Controls
Currently, the whole game is to be operated with a mouse only. The support for keyboard and gamepad will be added in later releases.

## Game Mechanics

### Drops
If a tile is hit, it may summon a drop. Drops simply are physical pickable powers. Their effects are initiated as soon as the paddle collides with it.

#### Drop Types
- Saver - Grants one temporary save for each striker. If active, the striker bounces off the bottom edge instead of going through. Lasts one time and is indicated by green.
- Ridigify - Makes all strikers hard and heavy, so that they smash through the tile without reversing direction. Lasts for a few collision till the striker cools down. Indicated by flaming red hot striker.
- Extend - Extends the lengths of paddle for few seconds.
- Gun - The paddle fires a few rounds of pair of bullets that destroy tiles.

### Tile Effects
If a tile is hit, it has a few chance to initiate an effect.

#### Tile Effects Types
- New Striker - Adds a new striker into the arena.
- Explosion - Removes either adjacent or all tiles of the row and column of the tile.
- Respawn - Revives either adjacent or all tiles of the row and column of the tile.
