# Diskspace_Notification

A Windows Forms utility that monitors system drive (C:) disk space usage and alerts users when free space drops below 10%.

## Features

- Displays current free space in GB and percentage
- Progress bar with color indicator (green = healthy, red = below 10% free)
- Fires a `DiskSpaceLow` event when free space is critically low
- Bilingual alert message (Azerbaijani / English)
- Quick link to open a Helpdesk support ticket

## Requirements

- Windows OS
- .NET Framework 4.7.2

## Build

Open `Diskspace_Notification.sln` in Visual Studio and build with **Debug** or **Release** configuration.

## Usage

Run `bin\Release\Diskspace_Notification.exe`. The app launches on top of all windows and shows the current disk usage on load. Click **Refresh** to update. Use the language buttons to switch the alert text between Azerbaijani and English.
