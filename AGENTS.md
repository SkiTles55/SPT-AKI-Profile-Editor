# AGENTS.md

## Purpose
This file helps AI coding agents understand the structure, build/test commands, and conventions of the SPT-AKI Profile Editor repository.

## What this repository contains
- `SPT-AKI Profile Editor/`: WPF desktop application targeting `net9.0-windows10.0.17763.0`.
- `SPT-AKI Profile Editor.Tests/`: NUnit-based unit test project.
- `SPT-AKI Profile Editor.ModHelper/`: helper mod project referenced by the main app.
- `SPT-AKI Profile Editor.Installer/`: Visual Studio installer project.
- `Guidelines/`: user-facing guides for localization and mod helper usage.

## Build and test commands
Use the solution root as the working directory.
- Build: `dotnet build "SPT-AKI Profile Editor.sln"`
- Test: `dotnet test "SPT-AKI Profile Editor.Tests\SPT-AKI Profile Editor.Tests.csproj"`

## Important conventions
- UI is built with WPF + MahApps.Metro and follows an MVVM-style pattern.
- The main application project embeds JSON localization files under `Resources/Localizations`.
- The app expects a helper-mod DLL under `ModHelper/` and copies it to the output directory.
- Tests use NUnit 4.1 and the `Microsoft.NET.Test.Sdk` test runner.

## Notes for AI agents
- Prioritize the main WPF project and its view models for behavior changes.
- Preserve localization resource structure and existing JSON translations when modifying text/strings.
- Avoid changing release artifacts or installer files unless the user explicitly asks for packaging or deployment updates.
- Use `README.md` and the language-specific guides under `Guidelines/` for user-facing feature descriptions.

## Useful links
- [README.md](README.md)
- [English README](ENGREADME.md)
- [Localization guide](Guidelines/LocalizationsENG.md)
- [Helper mod guide](Guidelines/ModHelperENG.md)
