# Tile Breaker

## Quickfire Instructions
- Download the latest verison of Tile Breaker from releases page.
- The game is portable, hence, no installation needed, just open and begin playing.
- Click anywhere or press Enter to start.
- Hover mouse cursor on top right corner for the retry option to appear.
- Hover mouse cursor on top left corner for the settings option to appear.

# Game Design Document

## Introduction

### Summary
Tile Breaker is a retro style arcade like game based on Breakout (1976) by Atari with features and graphical improvements.

### Inspiration
Breakout by Atari

### Platform
Windows and Linux

### Genre
Arcade and Retro

### Target Audience
People who want to experience a Breakout like game again.

### Development Software
- Godot 4 for game engine.
- Visual Studio for IDE.
- Inkscape for vector graphics.

## Concept

### Gameplay overview
The game consists of a grid of tiles, a primary striker and a paddle (which is in control of the player). The player moves the paddle to bounce off the striker and make them collide with a tile. On collsion, a tile is destroyed and may initiate effects or summon a pickable drop

Game declares victory if all the tiles are destroyed, but, if all the strikers fall below the screen, the player loses. Activiies like destroying tiles and collecting drops contribute to total score.

### Navigating UI
- The game can be restarted by hovering the mouse near top-right corner and clicking twice.
- The settings panel will appear if the mouse is hovered at top-left corner.

## Game Experience

### UI
Unlike the original Breakout, this game follows high resolution rendering, instead of pixelated one.
The game features proximity panels, that appear when cursor is hovered near corners of the window, inspired by Windows 8.

### Controls
The whole game is be operated with a mouse only. The support for keyboard and gamepad will be added in later releases.
