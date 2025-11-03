Troid Developer Guidelines
==========================

This document serves as the internal rulebook for all developers
contributing to the Troid Essentials project. Adhereing to these
guidelines is mandatory to maintain project consistency,
clearn version control history, and efficient team collaboration.



### 1. Project Hierarchy & Naming Conventions

All assets and code must strictly adhere to the folder structure stated below.
 

#### 1.1. Folder structure

- **Assets Root:** The `Assets/` folder is divided into three top-level directories:
	- `Assets/_Game`: All custom project files (code, art, scenes, prefabs) _must be placed inside this folder_.
	- `Assets/_Thirdparty`: All externally sourced assets (Asset Store packages, external libraries).
	- `Assets/_Sandbox`: For temporal protyping and testing **ONLY**. _Do not derive functional game code from this folder!_


#### 1.2. Naming Conventions

- ** Files and Folders:** Use _CamelCase_ for all file and folder names (i.e. `PlayerController.cs`, `MainMenuScene`).
- **Spaces:** _**DO NOT**_ use spaces in any file or folder names!
- **Scripts:** C# scripts must have a descriptive name, that clearly indicates their purpose (i.e. `AsteroidSpawner`, `HealthManager`).



### 2. Git Commit Message Standards

All commit messages must use the following standardized prefix format to keep 
Git history searchable and readable. Commit messages should be concise and descriptive.

**Format:** `PREFIX: Summary of change`

| Prefix   | Type of change                                          | Example                                                  |
| -------- | ------------------------------------------------------- | -------------------------------------------------------- |
| FEAT     | A new feature or system (i.e. adding a new enemy type). | `FEAT: Add basic shield regeneration logic to player`    |
| FIX      | A bug fix (i.e., fixing an issue with collisions).      | `FIX: Correct player rotation offset on input update`    |
| CHORE    | Maintenance, refactoring or documentation updates.      | `CHORE: Update Dev-Guidelines with naming rules`         |
| REFACTOR | Restructuring code without changing functionality.      | `REFACTOR: Simplify input handling into dedicated class` |
