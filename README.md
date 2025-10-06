# 🎮 Helix Jump Game

A polished 3D endless arcade game built with Unity featuring true object pooling, procedural platform generation, multiple difficulty modes, and optimized performance for WebGL deployment.

[![Play on GitHub Pages](https://img.shields.io/badge/▶️_Play_Now-Live_Demo-brightgreen?style=for-the-badge&logo=github)](https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/)
[![View Repository](https://img.shields.io/badge/View-Repository-blue?style=for-the-badge&logo=github)](https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto)

![Unity](https://img.shields.io/badge/Unity-6000.0.26f2-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-11.0-239120?logo=c-sharp)
![Platform](https://img.shields.io/badge/Platform-WebGL-blue)
![Status](https://img.shields.io/badge/Status-Complete-success)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Play Online](#play-online)
- [Features](#features)
- [How to Play](#how-to-play)
- [Controls](#controls)
- [Difficulty Modes](#difficulty-modes)
- [Technical Highlights](#technical-highlights)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Building](#building)
- [Credits](#credits)
- [License](#license)
- [Contact](#contact)

---

## 🎯 Overview

Helix Jump is a skill-based endless arcade game where players navigate a bouncing ball through a rotating helix tower. Avoid deadly platforms, pass through safe platforms, and survive as long as possible while competing for the highest scores across four difficulty levels!

**Development Time:** 3 days  
**Unity Version:** 6000.0.26f2  
**Target Platform:** WebGL (browser-playable)  
**Performance:** 60 FPS constant with zero GC allocations during gameplay

---

## 🎮 Play Online

### **[▶️ PLAY NOW - Live Demo](https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/)**

**Requirements:**
- Modern web browser (Chrome 90+, Firefox 88+, Edge 90+)
- JavaScript enabled
- **Important:** Click anywhere to unlock audio (browser autoplay policy)
- Keyboard or mouse/touchscreen for controls

**Quick Start:**
1. Click the link above
2. Click anywhere on the page to enable audio
3. Select difficulty from main menu
4. Use A/D keys or Arrow keys to rotate, or drag with mouse
5. Avoid deadly platforms (red) and survive as long as possible!

---

## ✨ Features

### 🎮 **Core Gameplay**
- **True Object Pooling**: 20 platforms cycle infinitely with zero runtime instantiation
- **Procedural Generation**: Random gaps (1-3 segments) and deadly segments (0-4 based on difficulty)
- **Physics-Based Ball**: Custom bounce system with independent height and frequency control
- **Smooth Controls**: Lerp-based rotation with configurable sensitivity (keyboard, mouse, touch)
- **Endless Gameplay**: Infinite platform cycling with consistent 60 FPS performance
- **Visual Continuity**: Separate cylinder management for seamless center pole throughout gameplay
- **Countdown System**: 3-second countdown with device-specific control hints

### 🏆 **Progression System**
- **4 Difficulty Modes**: Easy, Medium, Hard, Expert with distinct challenge levels
- **Per-Difficulty Tracking**: Separate high scores and best times for each difficulty
- **Persistent Storage**: PlayerPrefs-based score and settings persistence across sessions
- **High Score Logic**: Higher score wins; equal scores compare by time (lower time wins)
- **New High Score Celebration**: Visual and audio feedback on record breaks

### 🎨 **Polish & UI**
- **Main Menu**: Clean layout with difficulty descriptions and per-difficulty statistics
- **Real-Time HUD**: Score and timer displayed during gameplay
- **Game Over Screen**: Final score, time, high score, and best time statistics
- **Pause System**: ESC key pauses with resume, settings, and exit options
- **Settings Panel**: Volume controls, sensitivity adjustments, invert options, reset defaults
- **Control Hints**: Device-specific hints during countdown (keyboard/mouse/touch)
- **Smooth Camera**: Lerp-based follow for polished visual experience

### 🔊 **Audio System**
- **Background Music**: Separate tracks for main menu and gameplay
- **6 Sound Effects**:
  - Ball bounce on safe platforms
  - Button clicks for UI interactions
  - Death sound on deadly platform collision
  - Level completion celebration
  - New high score achievement
  - Platform clear on gap passage (including Platform_0)
- **Optimized Playback**: 5-source round-robin pool for instant, zero-delay SFX
- **Volume Controls**: Independent BGM and SFX sliders with squared curve for better control
- **WebGL Compatibility**: Vorbis compression for all audio, browser autoplay handling

### ⚙️ **Settings & Customization**
- **Audio Settings**:
  - Separate BGM and SFX volume sliders (0-100%)
  - Squared volume curve for improved slider feel
- **Control Settings**:
  - Keyboard sensitivity (0.5x - 3x)
  - Mouse sensitivity (0.5x - 3x)
  - Touch sensitivity (0.5x - 3x)
  - Independent invert toggles per input type
- **Smoothing Control**: Rotation smoothing factor (0-0.5, default 0.15)
- **Reset to Defaults**: One-click restoration of default settings
- **Persistent Settings**: All settings saved via PlayerPrefs

### 🎯 **Difficulty Modes**

| Difficulty | Bounce Speed | Deadly Segments | Description |
|------------|-------------|-----------------|-------------|
| **Easy** | 1.75x | 1-2 per platform | Slow bouncing, few dangers, beginner-friendly |
| **Medium** | 2.5x | 1-3 per platform | Balanced challenge, moderate speed |
| **Hard** | 3.25x | 2-4 per platform | Fast bouncing, more dangers, high skill required |
| **Expert** | 4.0x | 3-4 per platform | Extreme speed and hazards, master level |

---

## 🎮 How to Play

### **Objective**
Navigate the bouncing ball through the rotating helix tower by controlling the rotation. Avoid deadly platforms (red) while passing through gaps in safe platforms (blue/green). Survive as long as possible and achieve the highest score!

### **Game Rules**
1. **Ball Physics**: Ball bounces automatically with consistent height
2. **Safe Platforms** (Blue/Green): Pass through safely, +1 point per platform
3. **Deadly Platforms** (Red): Instant game over on contact
4. **Gaps**: Navigate through gaps to descend and score points
5. **Scoring**: +1 point per platform passed (starting from Platform_1)
6. **Timer**: Time tracked for leaderboard and high score tiebreaker
7. **Endless**: No finish line—survive and score as long as possible

### **Winning Strategy**
- **Timing**: Master rotation timing to align with gaps ahead
- **Prediction**: Look ahead to predict upcoming platform patterns
- **Quick Adjustments**: Use rapid directional changes when needed
- **Momentum**: Maintain smooth rotation through safe zones
- **Progressive Challenge**: Start on Easy, work your way up to Expert

---

## 🕹️ Controls

### **Desktop**
- **A Key / Left Arrow (←)**: Rotate helix left
- **D Key / Right Arrow (→)**: Rotate helix right
- **Mouse Drag**: Click and drag left/right to rotate
- **ESC**: Pause game / Close menus

### **Mobile / Tablet**
- **Swipe Left/Right**: Rotate helix
- **Tap Pause Button**: Open pause menu

### **Settings Customization**
- Adjust sensitivity for each input type independently (0.5x - 3x)
- Invert controls per input device (keyboard, mouse, touch)
- Modify smoothing factor for rotation feel (0-0.5)

### **Control Hints**
Device-specific control hints displayed during countdown:
- **Desktop (with mouse)**: "Use A/D or Arrow keys\nor drag with mouse to rotate"
- **Desktop (keyboard only)**: "Use A/D or Arrow keys to rotate"
- **Mobile**: "Swipe left or right to rotate"
- **Tablet**: "Drag left or right to rotate"

---

## 🏗️ Technical Highlights

### **Architecture**
- **Singleton Managers**: DontDestroyOnLoad pattern for cross-scene persistence
- **Event-Driven UI**: C# events for decoupled, responsive UI updates
- **Modular Design**: Clear separation of concerns (audio, input, difficulty, score, UI)
- **Object Pooling**: Pure pooling with fixed 20-platform cycle
- **Optimized Performance**: Constant 60 FPS on WebGL with zero GC spikes during gameplay

### **Key Systems**

#### **True Object Pooling**
```
// Fixed pool of 20 platform GameObjects, recycled infinitely
private List<GameObject> _platformPool = new List<GameObject>();

// Platform reuse logic (wrapping index)
int poolIndex = platformIndex % _platformPool.Count; // Wrap: 0-19, 0-19...
GameObject platform = _platformPool[poolIndex];

// Only position and rotation change
platform.transform.localPosition = newPosition;
platform.transform.localRotation = randomRotation;
// Segments and configuration remain unchanged (pure reuse)
```

**Benefits:**
- **Zero runtime instantiation**: All platforms created once at start
- **Zero GC allocations**: No new/destroy during gameplay
- **Constant memory**: Fixed footprint regardless of play duration
- **Predictable performance**: No frame drops or stuttering
- **WebGL optimized**: Smooth browser performance

#### **Procedural Platform Generation**
```
// Each platform procedurally generated at start with:
- Random Y rotation (0-360°) for visual variety
- Random gap size (1-3 segments) and position
- Random deadly segment count (0-4 based on difficulty)
- Random deadly segment positions (excluding gap areas)

// Configuration locked after initial creation
// Only position/rotation modified during reuse
```

#### **Custom Ball Physics**
```
// Independent bounce height and frequency control
_calculatedGravity = 2f * _targetBounceHeight * (_bounceFrequency²);
_calculatedBounceVelocity = √(2 × gravity × height);

// Result: Consistent bounce height regardless of frequency
// Higher frequency = faster bouncing, same height
// Lower frequency = slower bouncing, same height
```

**Physics Explanation:**
- Bounce frequency controls how fast the ball completes each bounce cycle
- Target bounce height (3 units) remains constant across all difficulties
- Gravity and initial velocity calculated to achieve both parameters simultaneously

#### **Smooth Rotation System**
```
// Lerp-based smoothing for polished feel
_currentVelocity = Mathf.Lerp(_currentVelocity, _targetSpeed, 
                               1f - _smoothingFactor);

// Default: 0.15 smoothing factor
// Range: 0 (instant/no smoothing) to 0.5 (very smooth)
// Provides natural acceleration/deceleration feel
```

#### **Audio Optimization**
```
// 5-source round-robin pool for instant playback
private AudioSource[] _sfxSources = new AudioSource;
private int _nextSfxIndex = 0;

// Zero-overhead playback (no availability checking)
_sfxSources[_nextSfxIndex].PlayOneShot(clip);
_nextSfxIndex = (_nextSfxIndex + 1) % 5;

// Benefits:
// - No foreach loops = zero GC
// - No availability checking = instant playback
// - Round-robin = fair source distribution
// - Vorbis compression for WebGL compatibility
```

#### **Cylinder Management**
```
// 23 cylinders generated independently from platforms:
// - 20 cylinders for platform positions (Y=0 to Y=-76)
// - 3 cylinders above start (Y=+4, +8, +12)

// Transfer logic:
// - Platforms 0-3: No cylinder transfer (initial buffer)
// - Platform 4 onwards: Top cylinder moves to bottom per platform passed
// - Sequential transfer maintains continuous visual pole
```

#### **Per-Difficulty Persistence**
```
// Separate tracking for each difficulty level
PlayerPrefs Keys:
- "HighScore_Easy" / "HighScoreTime_Easy"
- "HighScore_Medium" / "HighScoreTime_Medium"
- "HighScore_Hard" / "HighScoreTime_Hard"
- "HighScore_Expert" / "HighScoreTime_Expert"

// High score comparison logic:
if (score > highScore) return true;              // Higher score wins
if (score == highScore && time < highScoreTime) return true; // Faster time wins
return false;
```

#### **Performance Optimizations**
- Platform recycling checked every 3 frames instead of every frame
- Direct List access with modulo wrapping for efficient lookups
- Cached collections to avoid allocations during gameplay
- Squared volume curve for better slider control
- No build compression for faster initial load times

---

## 📁 Project Structure

```
AnusheelSoni-Assignment-Helix_Jump_Proto/
├── docs/                          # GitHub Pages (WebGL Build)
│   ├── Build/                     # Compressed game files
│   ├── TemplateData/              # Unity web template
│   └── index.html                 # Game entry point
│
├── Assets/
│   ├── _Scenes/
│   │   ├── MainMenu.unity         # Main menu scene
│   │   └── MainGame.unity         # Gameplay scene
│   │
│   ├── Scripts/
│   │   ├── Managers/              # Singleton managers
│   │   ├── Gameplay/              # Game logic
│   │   ├── Input/                 # Input handling
│   │   └── UI/                    # UI controllers
│   │
│   ├── Prefabs/
│   │   ├── Ball.prefab
│   │   └── PlatformSegment.prefab
│   │
│   ├── Materials/
│   │   ├── SafePlatform.mat
│   │   ├── DeadlyPlatform.mat
│   │   ├── CentralPole.mat
│   │   ├── Ball.mat
│   │   └── BallTrail.mat
│   │
│   ├── Audio/
│   │   ├── Music/                 # BGM tracks (Vorbis)
│   │   └── SFX/                   # Sound effects (Vorbis)
│   │
│   ├── Models/
│   │   └── PlatformSegment.fbx    # Custom platform mesh
│   │
│   ├── Plugins/
│   │   └── TextMesh Pro/          # TextMeshPro package
│   │
│   └── Settings/
│       └── (URP pipeline assets)
│
└── ProjectSettings/               # Unity configuration

```

**Note:** For complete detailed folder structure, see `FOLDER_STRUCTURE.md` in the repository root.

---

## 🚀 Installation

### **Playing the Game**

**Option 1: Play Online (Recommended)**
- Visit: [https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/](https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/)
- No download or installation required
- Works on desktop and mobile browsers
- Click anywhere to unlock audio

**Option 2: Clone and Open in Unity**
```
# Clone the repository
git clone https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto.git

# Navigate to folder
cd AnusheelSoni-Assignment-Helix_Jump_Proto

# Open in Unity Hub
# Add → Select project folder → Open with Unity 6000.0.26f2
```

### **For Developers**

**Prerequisites:**
- Unity 6000.0.26f2
- JetBrains Rider 2025.2.2.1 or Visual Studio 2022
- WebGL Build Support (optional, for building)
- Git (optional, for version control)

**Setup Steps:**
1. Install Unity 6000.0.26f2 via Unity Hub
2. Clone repository or download ZIP
3. Open project in Unity Hub
4. Wait for initial import and compilation
5. Open MainMenu scene to start

---

## 🔨 Building

### **WebGL Build Process**

1. **Open Build Settings**
   - File → Build Settings (Ctrl+Shift+B)

2. **Configure Platform**
   - Select **WebGL** in platform list
   - Click **Switch Platform** (wait 1-2 minutes)

3. **Add Scenes**
   - Ensure both scenes are in build:
     - MainMenu.unity (index 0)
     - MainGame.unity (index 1)

4. **Configure Audio (Critical for WebGL)**
   
   **For all SFX files** (Ball_Bounce_SFX, Button_Click_SFX, etc.):
   - Select audio file in Project window
   - Inspector → Platform Settings → **WebGL**
   - Settings:
     ```
     Load Type: Decompress On Load
     Compression Format: Vorbis
     Quality: 70-100
     Sample Rate: Preserve Sample Rate
     ```
   
   **For Music files** (MainMenu_BGM, MainGame_BGM):
   - Select audio file in Project window
   - Inspector → Platform Settings → **WebGL**
   - Settings:
     ```
     Load Type: Streaming
     Compression Format: Vorbis
     Quality: 70-100
     ```

5. **Build**
   - Click **Build** button
   - Choose output folder (e.g., `docs/`)
   - Wait 5-10 minutes for build completion
   - **No compression** is used for faster loading

6. **Test Locally**
   - Open `docs/index.html` in browser
   - Or use **Build and Run** for automatic testing

---

## 🎓 Learning Outcomes

This project demonstrates proficiency in:

- ✅ **Unity 3D Development**: Scene management, prefabs, materials, URP
- ✅ **Physics Programming**: Custom bounce mechanics, collision handling
- ✅ **Object Pooling**: True pooling with zero runtime instantiation
- ✅ **Procedural Generation**: Random platform configurations, gap placement
- ✅ **Audio Engineering**: Optimized 5-source SFX pool, volume control
- ✅ **UI/UX Design**: Responsive menus, HUD, settings, pause system
- ✅ **State Management**: Singleton pattern, DontDestroyOnLoad
- ✅ **Performance Optimization**: Zero GC allocations, constant 60 FPS
- ✅ **Cross-Platform Input**: Keyboard, mouse, touch support with device detection
- ✅ **Data Persistence**: PlayerPrefs for scores and settings
- ✅ **WebGL Deployment**: Browser compatibility, GitHub Pages hosting
- ✅ **Version Control**: Git workflow with detailed, descriptive commits

---

## 📝 Credits

**Developer:** Anusheel Soni  
**Email:** anusheelsoni4@gmail.com  
**GitHub:** [@eroxion](https://github.com/eroxion)  
**Date:** October 2025  
**Development Time:** 3 days

**Tools & Technologies:**
- Unity 6000.0.26f2
- C# Programming Language
- JetBrains Rider 2025.2.2.1
- Blender (3D modeling for platform segments)
- Git & GitHub
- GitHub Pages (web hosting)

**Assets:**
- All assets used under **Creative Commons 0** (CC0 - Public Domain)
- Audio: Royalty-free sound effects and music
- 3D Models: Custom-created in Blender

---

## 📄 License

This project was created as part of a game development assignment for **Eternoplay**. All rights reserved.

---

## 📧 Contact

**Developer:** Anusheel Soni  
**Email:** anusheelsoni4@gmail.com  
**GitHub:** [github.com/eroxion](https://github.com/eroxion)

**For Inquiries:**
- Technical questions: Create an issue on GitHub
- General feedback: Email anusheelsoni4@gmail.com
- Bug reports: Use GitHub Issues with detailed description

---

## 🔗 Quick Links

- **🎮 Play Game:** [https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/](https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/)
- **📦 Repository:** [https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto](https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto)
- **📝 Commits:** [View Development History](https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto/commits)

---

## 🙏 Acknowledgments

Special thanks to **Eternoplay** for providing this opportunity to showcase game development skills through this challenging and rewarding assignment.

---

**⭐ If you enjoyed this project, consider giving it a star on GitHub!**

---

*Last Updated: October 6, 2025*