# Helix Jump - Folder Structure Documentation

## Project: AnusheelSoni-Assignment-Helix_Jump_Proto

This document defines the organizational structure for all project assets in the final implementation.

---

## Assets Folder Hierarchy

### `_Scenes/`
**Purpose**: Contains all Unity scene files
- `MainMenu.unity` - Game entry point with difficulty selection
- `MainGame.unity` - Core gameplay scene with endless helix tower

**Convention**: Scene names use PascalCase, descriptive naming

---

### `Scripts/`
**Purpose**: All C# scripts organized by responsibility

#### `Scripts/Managers/`
**Purpose**: Singleton managers with DontDestroyOnLoad pattern
- `AudioManager.cs` - 5-source audio pool for BGM and SFX
- `DifficultyManager.cs` - 4 difficulty modes with per-difficulty persistence
- `ScoreManager.cs` - Score tracking, timer, and high score with time tiebreaker
- `SettingsManager.cs` - PlayerPrefs-based settings persistence
- `UIManager.cs` - HUD, screens, and UI state management
- `PauseManager.cs` - Pause functionality and menu
- `MainMenuManager.cs` - Main menu navigation and difficulty selection

#### `Scripts/Gameplay/`
**Purpose**: Core gameplay mechanics and physics
- `PlatformGenerator.cs` - True object pooling with 20-platform cycle and procedural generation
- `BallController.cs` - Custom bounce physics with independent height/frequency control
- `HelixRotator.cs` - Rotation input handling with lerp-based smoothing
- `CameraController.cs` - Smooth camera follow using lerp

#### `Scripts/Input/`
**Purpose**: Cross-platform input handling
- `InputHandler.cs` - Unified input system for keyboard, mouse, and touch with device detection

#### `Scripts/UI/`
**Purpose**: UI controllers and panel management
- `DifficultyUI.cs` - Difficulty selection panel controller
- `SettingsUI.cs` - Settings panel with volume controls and sensitivity sliders

---

### `Prefabs/`
**Purpose**: Reusable GameObject prefabs
- `Ball.prefab` - Player ball with Rigidbody, collision detection, and trail renderer
- `PlatformSegment.prefab` - Individual platform segment for procedural generation

**Convention**: Prefab names are nouns in PascalCase

---

### `Materials/`
**Purpose**: All material assets
- `Ball_Mat.mat` - Ball visual material
- `BallTrail_Mat.mat` - Trail renderer material
- `Platform_Safe_Mat.mat` - Safe platform segments (blue/green)
- `Platform_Deadly_Mat.mat` - Deadly platform segments (red)
- `CentralPole_Mat.mat` - Center cylinder material
- `Platform_Finish_Mat.mat` - Legacy finish platform material (unused in endless mode)

**Convention**: Material names end with `_Mat` suffix

---

### `Models/`
**Purpose**: 3D models created in Blender
- `PlatformSegment.fbx` - Circular sector mesh for platform segments

**Convention**: Model files use descriptive PascalCase names with `.fbx` extension

---

### `Audio/`
**Purpose**: Audio assets for BGM and SFX

#### `Audio/Music/`
**Purpose**: Background music tracks (Vorbis format for WebGL)
- `MainMenu_BGM.ogg` - Main menu background music
- `MainGame_BGM.ogg` - Gameplay background music

#### `Audio/SFX/`
**Purpose**: Sound effects (Vorbis format for WebGL)
- `Ball_Bounce_SFX.ogg` - Ball bounce on safe platform
- `Button_Click_SFX.ogg` - UI button interactions
- `Death_SFX.ogg` - Deadly platform collision
- `Level_Win_SFX.ogg` - Level completion/victory
- `New_HighScore_SFX.ogg` - New high score achievement
- `Platform_Clear_SFX.ogg` - Gap passage sound (including Platform_0)

**Convention**: Audio files use descriptive names with `_BGM` or `_SFX` suffix, `.ogg` format

---

### `Settings/`
**Purpose**: Universal Render Pipeline (URP) configuration assets
- `DefaultVolumeProfile.asset` - Default post-processing volume
- `SampleSceneProfile.asset` - Scene-specific volume profile
- `Mobile_Renderer.asset` - Mobile-optimized renderer
- `Mobile_RPAsset.asset` - Mobile URP pipeline asset
- `PC_Renderer.asset` - Desktop-optimized renderer
- `PC_RPAsset.asset` - Desktop URP pipeline asset
- `UniversalRenderPipelineGlobalSettings.asset` - Global URP settings

**Convention**: Settings use descriptive names with platform prefix where applicable

---

### `Plugins/`
**Purpose**: Third-party plugins and packages

#### `Plugins/TextMesh Pro/`
**Purpose**: TextMeshPro package for advanced text rendering
- `Fonts/` - TMP font assets
- `Resources/` - TMP default resources
- `Shaders/` - TMP shader files
- `Sprites/` - TMP sprite assets (emoji support)

**Convention**: Standard Unity package structure maintained

---

## Folder Structure Overview

```
Assets/
├── _Scenes/              (2 scenes: MainMenu, MainGame)
├── Scripts/
│   ├── Managers/         (7 singleton managers)
│   ├── Gameplay/         (4 core gameplay scripts)
│   ├── Input/            (1 unified input handler)
│   └── UI/               (2 UI controllers)
├── Prefabs/              (2 reusable prefabs)
├── Materials/            (6 materials)
├── Models/               (1 custom FBX model)
├── Audio/
│   ├── Music/            (2 BGM tracks)
│   └── SFX/              (6 sound effects)
├── Settings/             (7 URP configuration assets)
└── Plugins/
    └── TextMesh Pro/     (Standard TMP package)
```

---

## Version Control Guidelines

### Do NOT commit:
- `Library/` folder (Unity cache)
- `Temp/` folder (temporary files)
- `.vs/` folder (Visual Studio cache)
- `obj/` folder (build intermediates)
- `*.csproj` files (auto-generated)
- `*.sln` files (auto-generated)
- `Builds/` folder (local build outputs)

### DO commit:
- `Assets/` folder (all project assets)
- `docs/` folder (GitHub Pages WebGL build)
- `Packages/` folder (package manifest)
- `ProjectSettings/` folder (project configuration)
- `.gitignore` file (exclusion rules)
- `README.md` (main documentation)
- `README_FolderStructure.md` (this file)
- `README_NamingConventions.md` (coding conventions)

---

## Special Notes

### Object Pooling Architecture
- `PlatformGenerator.cs` manages a fixed pool of 20 platform GameObjects
- Platforms reuse segments, only position and rotation change during recycling
- Separate cylinder management (23 cylinders) independent of platform pool

### Audio System Design
- 5-source round-robin pool for instant SFX playback with zero overhead
- All audio uses Vorbis compression for WebGL compatibility
- No PCM audio (removed for WebGL optimization)

### Endless Mode Implementation
- No finish platform concept in final version
- `Platform_Finish_Mat.mat` exists but is unused (legacy from earlier design)
- Infinite platform cycling with consistent 60 FPS performance

### Cross-Platform Support
- `InputHandler.cs` detects device type and adapts controls
- Settings panel includes separate sensitivity sliders per input type
- Device-specific control hints shown during countdown

---

## Maintenance Notes

- **Consistency**: All folder and file names follow established conventions
- **No empty folders**: Only folders with content are committed
- **Legacy assets**: `Platform_Finish_Mat.mat` retained for reference but unused
- **Documentation**: All major scripts include XML documentation comments

---

**Last Updated**: October 7, 2025  
**Project Status**: Complete - Ready for Submission  
**Development Time**: 3 days