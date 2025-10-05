# Helix Jump - Folder Structure Documentation

## Project: AnusheelSoni-Assignment-Helix_Jump_Proto

This document defines the organizational structure for all project assets.

---

## Assets Folder Hierarchy

### `_Scenes/`
**Purpose**: Contains all Unity scene files
- `MainMenu.unity` - Game entry point with Play/Settings buttons
- `MainGame.unity` - Core gameplay scene with helix tower

**Convention**: Scene names use PascalCase, descriptive action-based naming

---

### `Scripts/`
**Purpose**: All C# scripts organized by responsibility

#### `Scripts/Core/`
**Purpose**: Core game systems and managers
- `GameManager.cs` - Game state, score tracking, time management
- `ScoreData.cs` - Score comparison logic with time tiebreaker
- `GameConstants.cs` - Global constants and configuration values

#### `Scripts/Gameplay/`
**Purpose**: Gameplay mechanics and physics interactions
- `PlatformGenerator.cs` - Procedural platform generation system
- `BallController.cs` - Ball physics and collision detection
- `HelixRotator.cs` - Tower rotation mechanics

#### `Scripts/Input/`
**Purpose**: Input detection and platform-aware handling
- `InputHandler.cs` - Unified input for keyboard/mouse/touch with device detection

#### `Scripts/UI/`
**Purpose**: User interface controllers and menu systems
- `UIManager.cs` - Central UI coordinator, platform-specific configuration
- `MainMenuUI.cs` - Main menu button handlers
- `SettingsUI.cs` - Settings panel with rotation speed slider (desktop only)
- `GameOverUI.cs` - Game over screen display logic

#### `Scripts/Utilities/`
**Purpose**: Helper scripts and reusable components
- `CameraFollower.cs` - Smooth camera following ball descent

---

### `Plugins/`
**Purpose**: External plugins and platform-specific code

#### `Plugins/WebGL/`
**Purpose**: WebGL-specific JavaScript plugins
- `MobileDetection.jslib` - JavaScript library for mobile device detection

---

### `Prefabs/`
**Purpose**: Reusable GameObject prefabs
- `Platform.prefab` - Complete platform with segments
- `PlatformSegment.prefab` - Individual platform piece
- `Ball.prefab` - Player ball with physics and trail

**Convention**: Prefab names are nouns in PascalCase

---

### `Materials/`
**Purpose**: All material assets
- `Ball_Mat.mat` - Ball visual material
- `Platform_Safe_Mat.mat` - Safe platform segment material (blue)
- `Platform_Deadly_Mat.mat` - Deadly platform segment material (red)
- `Background_Mat.mat` - Environment background material

**Convention**: Material names end with `_Mat` suffix

---

### `Models/`
**Purpose**: 3D models imported from Blender or other tools
- `PlatformSegment.fbx` - Circular sector mesh for platform piece

**Convention**: Model files use descriptive PascalCase names

---

### `UI/`
**Purpose**: UI prefabs and sprite assets
- `MainMenuCanvas.prefab` - Main menu UI layout
- `GameplayCanvas.prefab` - In-game HUD elements
- `SettingsPanel.prefab` - Settings menu panel (desktop only)

**Convention**: UI prefabs end with `Canvas` or `Panel` suffix

---

## Naming Conventions

### General Rules
- **No spaces** in any file or folder names (use CamelCase)
- **Descriptive names** that clearly indicate purpose
- **Consistent capitalization** across similar asset types

### Script Naming
- **Classes**: PascalCase (e.g., `GameManager`, `BallController`)
- **Public fields/properties**: PascalCase (e.g., `RotationSpeed`)
- **Private fields**: camelCase with underscore prefix (e.g., `_currentScore`)
- **Methods**: PascalCase, verb-based (e.g., `IncrementScore()`, `StartGame()`)
- **Constants**: ALL_CAPS with underscores (e.g., `MAX_PLATFORMS`)
- **Events**: PascalCase with "On" prefix (e.g., `OnGameOver`)

### Asset Naming
- **Prefabs**: PascalCase nouns (e.g., `Ball.prefab`)
- **Materials**: PascalCase with `_Mat` suffix (e.g., `Platform_Safe_Mat.mat`)
- **Scenes**: PascalCase descriptive (e.g., `MainMenu.unity`)
- **Models**: PascalCase descriptive (e.g., `PlatformSegment.fbx`)

---

## Version Control Guidelines

### Do NOT commit:
- `Library/` folder (Unity cache)
- `Temp/` folder (temporary files)
- `.vs/` folder (Visual Studio cache)
- `*.csproj` files (auto-generated)
- `*.sln` files (auto-generated)
- `Builds/` folder (build outputs)

### DO commit:
- `Assets/` folder (all assets)
- `Packages/` folder (package manifest)
- `ProjectSettings/` folder (project configuration)
- `.gitignore` file (exclusion rules)
- README files (documentation)

---

## Maintenance Notes

- **Empty folders**: Avoid committing empty folders. Create folders only when needed.
- **Third-party assets**: Keep separate from internal assets (currently none in project)
- **Sandbox testing**: Use `Scripts/Utilities/` or create temporary `_Sandbox/` folder for experiments
- **Consistency**: Follow this structure throughout entire project - no deviations

---

**Last Updated**: October 5, 2025
**Project Phase**: Initial Setup