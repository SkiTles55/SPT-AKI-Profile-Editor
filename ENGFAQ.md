### Server gives error Mod SPT-AKI-ProfileEditor is missing package.json
This is not a mod, but a separate program. It does not need to be unpacked into the server mods folder.
</br></br>

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
    "file_serverexe": "Aki.Server.exe"
  },
```
#### UPD for SPT-AKI 3.0.0: [Profile Editor does not work with version 3.0.0?](https://youtu.be/XO2r4dG_kpk)
#### UPD: Starting from version 2.4 the program a dialog with a list of files\folders that could not be found in the selected folder. If necessary, you can edit the relative paths in this dialog.
</br></br>

### Some items are missing in the "Adding items" window
Many items are not available for adding to the stash because they can break the profile. Maybe this will change in future updates, maybe not.
#### UPD: Starting from version 2.7, you can use the "All Items" dialog ("Stash" - "Adding items" - "All Items"). This dialog allows you to add all items without constraint checks. This function can damage the profile, use at your own risk!
</br></br>

### Antivirus \ Windows Defender \ SmartScreen considers this program to be suspicious
This program is provided open source and does not contain any viruses, provided that you have downloaded the program from this GitHub repository. If you do not trust this repository, stop using the program.
</br></br>

### The statuses of individual quests are not saved
The server checks the quest statuses for compliance with various conditions (level, other quests, etc.), and corrects them. Therefore, change the status of the quest after unlocking it in the game, or set all profile characteristics to maximum.
#### UPD: You can turn off checking quest statuses in [ALLINONE MOD](https://hub.sp-tarkov.com/files/file/1-allinone-mod/)
#### UPD 2: Starting from version 2.2, the program checks the statuses of quests for compliance with a large part of the server conditions, and suggests correcting the found inconsistencies if "Always ignore" is not set in the settings ("Default action for profile issues" item, in the "Additional" section).
</br></br>

### Traders levels are not saved
The level of PMCs is below that required for traders levels. Raise the PMC level.
#### UPD: Starting from version 2.2, the program checks the levels of traders for compliance with the level of PMCs, and suggests correcting the level if "Always ignore" is not set in the settings ("Default action for profile issues" item, in the "Additional" section).
</br></br>

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
</br></br>

### Only active quests are displayed in the program
Turn on "Adding missing quests to the profile" in Settings - Additional
</br></br>

### The program does not display all available skills / weapon skills
Turn on "Adding missing Scav skills to the profile \ Adding missing weapon masterings to profile" in Settings - Additional
</br></br>

### Names of items, quests, skills, etc. do not match the names in the game
The language selected in the game does not match to the selected localization in the profile editor. If the required localization is missing, you can create the necessary one, using the English localization as a basis: [(Editing \ creating localization files)](/Guidelines/LocalizationsENG.md)
#### UPD: Starting from version 2.4, you can create a new localization in the settings by clicking on the "+" button next to the selected language.

### "Item added by mod found. Use the section \"Cleaning from mods\" to remove such items." error when adding an item
Typically, this error occurs if stash have weapon with a spare part added by mods. Remove this weapon\move it to some container\or use a helper mod.
