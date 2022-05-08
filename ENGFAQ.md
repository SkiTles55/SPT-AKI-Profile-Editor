### Server gives error Mod SPT-AKI-ProfileEditor is missing package.json
This is not a mod, but a separate program. It does not need to be unpacked into the server mods folder.

### The program displays the message: "The selected path does not seem to be a SPT-AKI server location."
Make sure that folder user\profiles exist in the server folder. Before using the program, you must start the server, create a profile in the launcher, and go into the game at least once.

Make sure you are using the correct version of the program for your SPT-AKI server. If the [README](ENGREADME.md)) does not indicate compatibility with your version of the server, wait for an update. Or you can try editing the relative paths in the [AppSettings.json](ENGFAQ.md#where-can-i-find-the-appsettingsjson-file-logs-localizations-and-backups-maded-by-application) file, but success is not guaranteed!

Make sure that all folders and files from the DirsList and FilesList sections in the [AppSettings.json](ENGFAQ.md#where-can-i-find-the-appsettingsjson-file-logs-localizations-and-backups-maded-by-application) file are present in the server folder, and have the correct names (some non-origin SPT-AKI builds may change file names, for example Server1.exe instead of Server.exe).
You can make changes to these sections as needed.

An example of the DirsList and FilesList sections in the [AppSettings.json](ENGFAQ.md#where-can-i-find-the-appsettingsjson-file-logs-localizations-and-backups-maded-by-application) file:
```json
"DirsList": {
    "dir_globals": "Aki_Data\\Server\\database\\locales\\global",
    "dir_traders": "Aki_Data\\Server\\database\\traders",
    "dir_bots": "Aki_Data\\Server\\database\\bots\\types",
    "dir_profiles": "user\\profiles"
  },
  "FilesList": {
    "file_globals": "Aki_Data\\Server\\database\\globals.json",
    "file_items": "Aki_Data\\Server\\database\\templates\\items.json",
    "file_quests": "Aki_Data\\Server\\database\\templates\\quests.json",
    "file_areas": "Aki_Data\\Server\\database\\hideout\\areas.json",
    "file_handbook": "Aki_Data\\Server\\database\\templates\\handbook.json",
    "file_serverexe": "Server.exe"
  },
```

### Some items are missing in the "Adding items" window
Many items are not available for adding to the stash because they can break the profile. Maybe this will change in future updates, maybe not.

### Antivirus \ Windows Defender \ SmartScreen considers this program to be suspicious
This program is provided open source and does not contain any viruses, provided that you have downloaded the program from this GitHub repository. If you do not trust this repository, stop using the program.

### The statuses of individual quests are not saved
The server checks the quest statuses for compliance with various conditions (level, other quests, etc.), and corrects them. Therefore, change the status of the quest after unlocking it in the game, or set all profile characteristics to maximum.
#### UPD: You can turn off checking quest statuses in [ALLINONE MOD](https://hub.sp-tarkov.com/files/file/1-allinone-mod/)
#### UPD 2: Starting from version 2.2, the program checks the statuses of quests for compliance with a large part of the server conditions, and suggests correcting the found inconsistencies if "Always ignore" is not set in the settings (Troubleshooting item, in the Additional section).

### Traders levels are not saved
The level of PMCs is below that required for traders levels. Raise the PMC level.
#### UPD: Starting from version 2.2, the program checks the levels of traders for compliance with the level of PMCs, and suggests correcting the level if "Always ignore" is not set in the settings (Troubleshooting item, in the Additional section).

### Where can I find the AppSettings.json file, logs, localizations and backups maded by application?
#### For application version 2.2 and higher:
User folder\AppData\Roaming\SPT-AKI Profile Editor</br>
%AppData%\SPT-AKI Profile Editor (press WIN + R, copy this path into the window that appears and press Enter, or run the program, Settings - Troubleshooting - Open app data folder)</br>
Backups - profile backups</br></br>
Localizations - application localizations</br>
Logs - application logs</br>
AppSettings.json - application settings</br>
#### For application versions 2.0 - 2.1.1:
Application root folder</br>
Backups - profile backups</br>
Localizations - application localizations</br>
Logs - application logs</br>
AppSettings.json - application settings</br>
