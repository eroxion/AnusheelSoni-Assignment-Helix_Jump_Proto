# 🎮 Helix Jump Game

A polished 3D arcade game built with Unity featuring procedural platform generation, difficulty modes, and smooth controls.

[![Play on GitHub Pages](https://img.shields.io/badge/Play-Live%20Demo-brightgreen?style=for-the-badge)](https://[your-username].github.io/[your-repo-name]/Builds/WebGL/)

![Unity](https://img.shields.io/badge/Unity-2022.3+-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-11.0-239120?logo=c-sharp)
![Platform](https://img.shields.io/badge/Platform-WebGL-blue)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [How to Play](#how-to-play)
- [Controls](#controls)
- [Difficulty Modes](#difficulty-modes)
- [Technical Highlights](#technical-highlights)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Building](#building)
- [Credits](#credits)

---

## 🎯 Overview

Helix Jump is a skill-based arcade game where players navigate a bouncing ball through a rotating helix tower. Avoid deadly platforms, clear safe platforms, and reach the finish line while competing for the best time!

**Development Time:** 6-8 hours  
**Unity Version:** 2022.3 LTS or higher  
**Target Platform:** WebGL (playable in browser)

---

## ✨ Features

### 🎮 **Core Gameplay**
- Procedurally generated helix tower with 50+ platforms
- Realistic physics-based ball mechanics with custom bounce system
- Smooth rotation controls with configurable sensitivity
- Deadly and safe platform types
- Finish platform for level completion
- 3-second countdown with control hints

### 🏆 **Progression System**
- 4 difficulty modes (Easy, Medium, Hard, Expert)
- Per-difficulty high score tracking
- Best time recording for each difficulty
- Persistent score storage across sessions

### 🎨 **Polish & UI**
- Clean main menu with difficulty selection
- Real-time HUD (score, timer)
- Game over and victory screens
- Pause menu with ESC key
- Settings panel (audio, controls)
- Device-specific control hints
- Smooth camera follow

### 🔊 **Audio System**
- Background music for menu and gameplay
- 6 sound effects (bounce, button click, death, win, high score, platform clear)
- Optimized audio pooling (8-source SFX system)
- Real-time volume controls
- Separate BGM and SFX sliders

### ⚙️ **Settings & Customization**
- Audio volume controls (BGM, SFX)
- Control sensitivity (keyboard, mouse, touch)
- Invert controls per input type
- Reset to defaults option
- Settings persist across sessions

### 🎯 **Difficulty Modes**

| Difficulty | Bounce Speed | Deadly Segments | Description |
|------------|-------------|-----------------|-------------|
| **Easy** | 1.75x | 1-2 per platform | Slow bouncing, few dangers |
| **Medium** | 2.5x | 1-3 per platform | Balanced challenge |
| **Hard** | 3.25x | 2-4 per platform | Fast bouncing, more dangers |
| **Expert** | 4.0x | 3-4 per platform | Extreme speed and hazards |

---

## 🎮 How to Play

### **Objective**
Navigate the ball through the rotating helix tower, avoiding deadly platforms (red) and passing through safe platforms (blue/green) to reach the finish platform at the bottom.

### **Game Rules**
1. **Ball Physics**: The ball bounces automatically with consistent height
2. **Safe Platforms** (Blue/Green): Pass through safely, resets combo
3. **Deadly Platforms** (Red): Instant game over if touched
4. **Finish Platform** (Gold): Reach to complete the level
5. **Scoring**: +10 points per platform cleared
6. **Timer**: Race against time for best records

### **Winning**
- Reach the finish platform at the bottom
- Beat your previous high score
- Achieve the fastest time for your chosen difficulty

---

## 🕹️ Controls

### **Desktop**
- **A / D Keys**: Rotate helix left/right
- **Arrow Keys**: Alternative rotation controls
- **Mouse Drag**: Click and drag to rotate
- **ESC**: Pause game

### **Mobile / Tablet**
- **Swipe**: Touch and drag left/right to rotate
- **Tap Pause Button**: Pause game

### **Settings**
- Adjust sensitivity for each input type
- Invert controls independently
- Customize to your preference

---

## 🏗️ Technical Highlights

### **Architecture**
- **Singleton Managers**: DontDestroyOnLoad pattern for cross-scene persistence
- **Event-Driven UI**: C# events for score/time updates
- **Modular Design**: Separated concerns (audio, input, difficulty, score)
- **Optimized Performance**: 60 FPS target on WebGL

### **Key Systems**

#### **Ball Physics**
