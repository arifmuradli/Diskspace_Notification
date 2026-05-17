# CLAUDE.md — Diskspace_Notification

## Project overview

Windows Forms desktop app (.NET Framework 4.7.2, C# 7.3) that monitors the system drive (C:) and fires a `DiskSpaceLow` event when configurable thresholds are breached. The event is consumed by an external process or script — the app itself only raises it.

## Key files

| File | Purpose |
|------|---------|
| `Form1.cs` | All logic: disk space calculation, threshold evaluation, label text building |
| `App.config` | Runtime configuration — thresholds and condition mode |
| `Form1.Designer.cs` | Auto-generated WinForms layout — do not edit manually |
| `Properties/Resources.Designer.cs` | Auto-generated resource accessor — do not edit manually |
| `.github/workflows/release.yml` | CI: builds and publishes a GitHub Release on tag push |

## Architecture

- **Threshold logic** lives entirely in `Form1.RefreshDiskSpace()`. It reads three keys from `ConfigurationManager.AppSettings` on every call so changes to `App.config` take effect on next refresh without recompile.
- **Alert text** (`label3`) is built dynamically by `BuildAzerbaijaniText()` / `BuildEnglishText()` via `BuildThresholdDescription()`. These methods also read from `App.config`, so the displayed thresholds always match the active config.
- **Condition engine** supports four modes: `%`, `GB`, `OR` (default), `AND`. Any unrecognised value falls back to `OR`.

## App.config keys

| Key | Default | Notes |
|-----|---------|-------|
| `DiskSpaceThresholdPercent` | `10` | % free space trigger |
| `DiskSpaceThresholdGB` | `5` | GB free space trigger |
| `DiskSpaceCondition` | `OR` | `%` \| `GB` \| `OR` \| `AND` |

## Language constraints

This project targets **.NET Framework 4.7.2** which uses **C# 7.3**. Do not use language features above C# 7.3:
- No switch expressions (C# 8+) — use `if/else` chains
- No nullable reference types (C# 8+)
- No records, init-only setters, pattern matching enhancements (C# 9+)

## Build

```
msbuild Diskspace_Notification.sln /p:Configuration=Release /p:Platform="Any CPU"
```

Output: `bin\Release\Diskspace_Notification.exe`

## Release process

Push a `v*` tag — GitHub Actions handles the rest:
```
git tag v1.0.0
git push origin v1.0.0
```

## WinForms conventions

- Event handler names (`button3_Click`, `button4_Click`) follow the WinForms designer naming convention and must match the wiring in `Form1.Designer.cs`. Renaming them requires updating both files.
- `label3.Text` is seeded by `Form1.resx` in the designer but overridden in `Form1_Load_1` to inject the live config values.
