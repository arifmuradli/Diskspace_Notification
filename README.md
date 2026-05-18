# Diskspace_Notification

A Windows Forms utility that monitors system drive (C:) disk space and alerts users when free space drops below configurable thresholds.

## Features

- Displays current free space in GB and percentage
- Progress bar with color indicator (green = healthy, red = threshold breached)
- Fires a `DiskSpaceLow` event consumed by an external process or script
- Bilingual alert message (Azerbaijani / English) with threshold values injected dynamically from config
- All thresholds and trigger conditions are configurable via `App.config` — no recompile needed

## Requirements

- Windows OS
- .NET Framework 4.7.2

## Build

Open `Diskspace_Notification.sln` in Visual Studio and build with **Debug** or **Release** configuration.

Releases are also built automatically via GitHub Actions on every version tag push (`v*`).

## Usage

Run `bin\Release\Diskspace_Notification.exe`. The app launches on top of all windows and shows current disk usage on load. Use the flag buttons to switch the alert text between Azerbaijani and English.

## Configuration (`App.config`)

All settings live in the `<appSettings>` section:

| Key | Default | Description |
|-----|---------|-------------|
| `DiskSpaceThresholdPercent` | `10` | Warn when free space drops below this percentage |
| `DiskSpaceThresholdGB` | `5` | Warn when free space drops below this many gigabytes |
| `DiskSpaceCondition` | `OR` | Controls which threshold(s) trigger the alert (see below) |

### `DiskSpaceCondition` values

| Value | Behaviour |
|-------|-----------|
| `%` | Trigger only when free space is below `DiskSpaceThresholdPercent` |
| `GB` | Trigger only when free space is below `DiskSpaceThresholdGB` |
| `OR` | Trigger when **either** condition is met *(default)* |
| `AND` | Trigger only when **both** conditions are met simultaneously |

### Example

```xml
<appSettings>
    <add key="DiskSpaceThresholdPercent" value="15" />
    <add key="DiskSpaceThresholdGB"      value="10" />
    <add key="DiskSpaceCondition"        value="AND" />
</appSettings>
```

This triggers only when free space is simultaneously below 15% **and** below 10 GB.

The bilingual alert label reads its threshold description directly from these keys, so changing values in `App.config` updates both the trigger logic and the displayed message without recompiling.

## Startup Monitoring Script (`diskspacemonitoring.vbs`)

`diskspacemonitoring.vbs` is a VBScript intended to run as a **user startup script** (e.g. deployed via Group Policy). It runs silently in the background and checks the C: drive free space every **30 minutes**. When free space drops below the configured threshold it launches `DiskSpace.exe` to show the notification window to the user.

**Deployment:** Add the script to the user's startup via Group Policy or place a shortcut in `%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup`.

## GitHub Actions

Pushing a tag (e.g. `git tag v1.0.0 && git push origin v1.0.0`) triggers the release workflow which:
1. Builds the project in Release mode using MSBuild
2. Creates a GitHub Release with the compiled `.exe` attached
