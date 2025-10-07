# 🎮 Helix Jump Game

A polished 3D endless arcade game built with Unity featuring true object pooling, procedural platform generation, multiple difficulty modes, and optimized performance for WebGL deployment.

[![Play on GitHub Pages](https://img.shields.io/badge/▶️_Play_Now-Live_Demo-brightgreen?https://eroxion.github.io/AnusheelSoni.io/badge/View-Repository-blue?style=for-the-badge)

![Unity](https://img.shields.io/badge/Unity-6000.0.26f2-black.shields.io/badge/C%23-11Platform-WebGL-)






***

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

***

## 🎯 Overview

Helix Jump is a skill-based endless arcade game where players navigate a bouncing ball through a rotating helix tower. Avoid deadly platforms, pass through safe platforms, and survive as long as possible while competing for the highest scores across four difficulty levels!

**Development Time:** 3 days  
**Unity Version:** 6000.0.26f2  
**Target Platform:** WebGL (browser-playable)  
**Performance:** 60 FPS constant with zero GC allocations during gameplay

***

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
- **Smooth Controls**: Lerp-based rotation with configurable sensitivity and global multiplier
- **Endless Gameplay**: Infinite platform cycling with consistent 60 FPS performance
- **Visual Continuity**: Separate cylinder management (23 cylinders) for seamless center pole
- **Instant Start**: 3-second countdown transitions directly to gameplay (no flash messages)
- **Fixed Platform Recycling**: Stable recycling logic preventing gaps beyond 100+ platforms

### 🏆 **Progression System**
- **4 Difficulty Modes**: Easy, Medium, Hard, Expert with distinct challenge levels
- **Per-Difficulty Tracking**: Separate high scores and best times for each difficulty
- **Persistent Storage**: PlayerPrefs-based score and settings persistence across sessions
- **High Score Logic**: Higher score wins; equal scores compare by time (lower time wins)
- **Complete Scoring**: All platforms count including Platform_0 (starting platform)
- **New High Score Celebration**: Visual and audio feedback on record breaks

### 🎨 **Polish & UI**
- **Main Menu**: Clean layout with difficulty descriptions and per-difficulty statistics
- **Real-Time HUD**: Score and timer displayed during gameplay
- **Game Over Screen**: Final score, time, high score, and best time statistics
- **Pause System**: ESC key pauses with resume, settings, and exit options
- **Settings Panel**: Volume controls, sensitivity adjustments, global multiplier, invert options
- **Smart Control Hints**: Device-specific hints with enhanced WebGL detection for accurate mobile/desktop browser recognition
- **Smooth Camera**: Lerp-based follow for polished visual experience

### 🔊 **Audio System**
- **Background Music**: Separate tracks for main menu and gameplay
- **6 Sound Effects**:
  - Ball bounce on safe platforms
  - Button clicks for UI interactions
  - Death sound on deadly platform collision
  - Level completion celebration
  - New high score achievement
  - Platform clear on gap passage (all platforms including Platform_0)
- **Optimized Playback**: 5-source round-robin pool for instant, zero-delay SFX
- **Volume Controls**: Independent BGM and SFX sliders with squared curve for natural feel
- **WebGL Compatibility**: Vorbis compression for all audio, browser autoplay handling

### ⚙️ **Settings & Customization**
- **Audio Settings**:
  - Separate BGM and SFX volume sliders (0-100%)
  - Squared volume curve for improved slider feel
- **Global Rotation Multiplier** *(New!)*:
  - Master speed control (0.1x - 5.0x) affecting all input types
  - Formula: InputSpeed × GlobalMultiplier × BaseMultiplier = degrees/second
  - Real-time adjustment with instant feedback
- **Control Settings**:
  - Keyboard sensitivity (0.1x - 3.0x)
  - Mouse sensitivity (0.1x - 100x)
  - Touch sensitivity (0.1x - 100x)
  - Independent invert toggles per input type
- **Reset to Defaults**: One-click restoration of default settings
- **Persistent Settings**: All settings saved via PlayerPrefs

### 🎯 **Difficulty Modes**

| Difficulty | Bounce Speed | Deadly Segments | Description |
|------------|-------------|-----------------|-------------|
| **Easy** | 1.75x | 1-2 per platform | Slow bouncing, few dangers, beginner-friendly |
| **Medium** | 2.5x | 1-3 per platform | Balanced challenge, moderate speed |
| **Hard** | 3.25x | 2-4 per platform | Fast bouncing, more dangers, high skill required |
| **Expert** | 4.0x | 3-4 per platform | Extreme speed and hazards, master level |

***

## 🎮 How to Play

### **Objective**
Navigate the bouncing ball through the rotating helix tower by controlling the rotation. Avoid deadly platforms (red) while passing through gaps in safe platforms (blue/green). Survive as long as possible and achieve the highest score!

### **Game Rules**
1. **Ball Physics**: Ball bounces automatically with consistent height
2. **Safe Platforms** (Blue/Green): Pass through safely, +1 point per platform
3. **Deadly Platforms** (Red): Instant game over on contact
4. **Gaps**: Navigate through gaps to descend and score points
5. **Scoring**: +1 point per platform passed (including Platform_0)
6. **Timer**: Time tracked for leaderboard and high score tiebreaker
7. **Endless**: No finish line—survive and score as long as possible

### **Winning Strategy**
- **Timing**: Master rotation timing to align with gaps ahead
- **Prediction**: Look ahead to predict upcoming platform patterns
- **Quick Adjustments**: Use rapid directional changes when needed
- **Momentum**: Maintain smooth rotation through safe zones
- **Speed Control**: Adjust global multiplier in settings to find your comfort zone
- **Progressive Challenge**: Start on Easy, work your way up to Expert

***

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
- Global rotation multiplier (0.1x - 5.0x) affects all input types equally
- Adjust sensitivity for each input type independently
- Invert controls per input device (keyboard, mouse, touch)

### **Control Hints**
Smart device-specific control hints displayed during countdown with enhanced WebGL browser detection:
- **Desktop Browser**: "Use A/D or Arrow keys\nor drag with mouse to rotate"
- **Mobile Browser**: "Swipe left or right to rotate"
- **Tablet Browser**: "Drag left or right to rotate"

***

## 🏗️ Technical Highlights

### **Architecture**
- **Singleton Managers**: DontDestroyOnLoad pattern for cross-scene persistence
- **Event-Driven UI**: C# events for decoupled, responsive UI updates
- **Modular Design**: Clear separation of concerns (audio, input, difficulty, score, UI)
- **True Object Pooling**: Fixed 20-platform cycle with zero runtime instantiation
- **Optimized Performance**: Constant 60 FPS on WebGL with zero GC spikes during gameplay
- **Comprehensive Documentation**: XML documentation on all classes and methods

### **Recent Improvements**

#### **Platform Recycling Fix** *(Critical)*
```csharp
// Fixed boundary calculation with half-spacing safety offset
float offsetY = -ballY - (_platformSpacing * 0.5f);
int passedIndex = Mathf.FloorToInt(offsetY / _platformSpacing);

// Unified spacing across ScoreManager and PlatformGenerator
// Platform spacing read directly from PlatformGenerator
_platformSpacing = generator.PlatformSpacing;

// Result: No doubled gaps, no mid-air disappearance, stable 100+ platforms
```

#### **Global Rotation Multiplier** *(New Feature)*
```csharp
// Master speed control affecting all input types
float degreesPerSecond = inputSpeed × globalMultiplier × baseMultiplier;

// Example calculations:
// Keyboard 1.1 × Global 1.0 × Base 200 = 220°/s (default)
// Keyboard 1.1 × Global 2.0 × Base 200 = 440°/s (2x faster)
// Mouse 50 × Global 0.5 × Base 200 = 5000°/s (half speed)

// Benefits:
// - Unified speed control across keyboard/mouse/touch
// - Fine-tune rotation speed to personal preference
// - Range: 0.1x (very slow) to 5.0x (very fast)
```

#### **Enhanced WebGL Device Detection** *(Improved)*
```csharp
// Browser user agent parsing for accurate device detection
string userAgent = GetBrowserUserAgent();
bool isMobile = userAgent.Contains("mobile") || 
                userAgent.Contains("android") || 
                userAgent.Contains("iphone");

// Result: Correct control hints on desktop vs mobile browsers
// Handles: Chrome, Firefox, Safari, Edge on desktop and mobile
```

### **Key Systems**

#### **True Object Pooling**
```csharp
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
```csharp
// Each platform procedurally generated at start with:
- Random Y rotation (0-360°) for visual variety
- Random gap size (1-3 segments) and position
- Random deadly segment count (0-4 based on difficulty)
- Random deadly segment positions (excluding gap areas)

// Configuration locked after initial creation
// Only position/rotation modified during reuse
```

#### **Custom Ball Physics**
```csharp
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
```csharp
// Lerp-based smoothing for polished feel
_currentVelocity = Mathf.Lerp(_currentVelocity, _targetSpeed, 
                               1f - _smoothingFactor);

// Default: 0.65 smoothing factor
// Range: 0.1 (very responsive) to 1.0 (very smooth)
// Provides natural acceleration/deceleration feel
// Global multiplier applies before smoothing
```

#### **Audio Optimization**
```csharp
// 5-source round-robin pool for instant playback
private AudioSource[] _sfxSources = new AudioSource[5];
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
```csharp
// 23 cylinders generated independently from platforms:
// - 20 cylinders for platform positions (Y=0 to Y=-76)
// - 3 cylinders above start (Y=+4, +8, +12)

// Transfer logic:
// - Platforms 0-3: No cylinder transfer (initial buffer)
// - Platform 4 onwards: Top cylinder moves to bottom per platform passed
// - Sequential transfer maintains continuous visual pole
```

#### **Per-Difficulty Persistence**
```csharp
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
- Platform recycling checked every 3 frames instead of every frame (66% CPU reduction)
- Direct List access with modulo wrapping for efficient lookups
- Cached collections to avoid allocations during gameplay
- Squared volume curve for better slider control feel
- No build compression for faster initial load times
- Unified spacing calculation prevents drift and recycling bugs

***

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
│   │   │   ├── AudioManager.cs
│   │   │   ├── DifficultyManager.cs
│   │   │   ├── ScoreManager.cs
│   │   │   ├── SettingsManager.cs
│   │   │   ├── UIManager.cs
│   │   │   ├── MainMenuManager.cs
│   │   │   └── PauseManager.cs
│   │   ├── Gameplay/              # Game logic
│   │   │   ├── BallController.cs
│   │   │   ├── PlatformGenerator.cs
│   │   │   ├── HelixRotator.cs
│   │   │   └── CameraController.cs
│   │   ├── Input/                 # Input handling
│   │   │   └── InputHandler.cs
│   │   └── UI/                    # UI controllers
│   │       ├── SettingsUI.cs
│   │       └── DifficultyUI.cs
│   │
│   ├── Prefabs/
│   │   ├── Ball.prefab
│   │   └── PlatformSegment.prefab
│   │
│   ├── Materials/
│   │   ├── Ball_Mat.mat
│   │   ├── BallTrail_Mat.mat
│   │   ├── Platform_Safe_Mat.mat
│   │   ├── Platform_Deadly_Mat.mat
│   │   └── CentralPole_Mat.mat
│   │
│   ├── Audio/
│   │   ├── MainMenu_BGM.ogg
│   │   ├── MainGame_BGM.ogg
│   │   ├── Ball_Bounce_SFX.ogg
│   │   ├── Button_Click_SFX.ogg
│   │   ├── Death_SFX.ogg
│   │   ├── Level_Win_SFX.ogg
│   │   ├── New_HighScore_SFX.ogg
│   │   └── Platform_Clear_SFX.ogg
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
├── README.md                      # This file
├── README_FolderStructure.md      # Detailed folder structure
├── README_NamingConventions.md    # Coding standards
└── ProjectSettings/               # Unity configuration
```

**Note:** For complete detailed folder structure, see `README_FolderStructure.md` in the repository root.

***

## 🚀 Installation

### **Playing the Game**

**Option 1: Play Online (Recommended)**
- Visit: [https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/](https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/)
- No download or installation required
- Works on desktop and mobile browsers
- Click anywhere to unlock audio

**Option 2: Clone and Open in Unity**
```bash
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

***

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
- ✅ **Cross-Platform Input**: Keyboard, mouse, touch with WebGL browser detection
- ✅ **Data Persistence**: PlayerPrefs for scores and settings
- ✅ **WebGL Deployment**: Browser compatibility, GitHub Pages hosting
- ✅ **Version Control**: Git workflow with detailed, descriptive commits
- ✅ **Code Documentation**: Comprehensive XML documentation across all scripts
- ✅ **Debugging**: Critical platform recycling bug fixes, scoring corrections

***

## 📝 Credits

**Developer:** Anusheel Soni  
**Email:** [anusheelsoni4@gmail.com](mailto:anusheelsoni4@gmail.com)  
**GitHub:** [@eroxion](https://github.com/eroxion)  
**Date:** October 2025  
**Development Time:** 3 days

**Tools & Technologies:**
- Unity 6000.0.26f2
- C# Programming Language
- JetBrains Rider 2025.2.2.1
- Blender 4.0 (3D modeling for platform segments)
- Git & GitHub
- GitHub Pages (web hosting)

**Assets:**
- All assets used under **Creative Commons 0** (CC0 - Public Domain)
- Audio: Royalty-free sound effects and music
- 3D Models: Custom-created in Blender

***

## 📄 License

This project was created as part of a game development assignment for **Eternoplay**. All rights reserved.

---

## 📧 Contact

**Developer:** Anusheel Soni  
**Email:** [anusheelsoni4@gmail.com](mailto:anusheelsoni4@gmail.com)  
**GitHub:** [github.com/eroxion](https://github.com/eroxion)

**For Inquiries:**
- Technical questions: Create an issue on GitHub
- General feedback: Email [anusheelsoni4@gmail.com](mailto:anusheelsoni4@gmail.com)
- Bug reports: Use GitHub Issues with detailed description

***

## 🔗 Quick Links

- **🎮 Play Game:** [https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/](https://eroxion.github.io/AnusheelSoni-Assignment-Helix_Jump_Proto/)
- **📦 Repository:** [https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto](https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto)
- **📝 Commits:** [View Development History](https://github.com/eroxion/AnusheelSoni-Assignment-Helix_Jump_Proto/commits)
- **📂 Folder Structure:** [README_FolderStructure.md](README_FolderStructure.md)
- **📖 Naming Conventions:** [README_NamingConventions.md](README_NamingConventions.md)

***

## 🙏 Acknowledgments

Special thanks to **Eternoplay** for providing this opportunity to showcase game development skills through this challenging and rewarding assignment.

***

**⭐ If you enjoyed this project, consider giving it a star on GitHub!**

***

*Last Updated: October 7, 2025*

***
