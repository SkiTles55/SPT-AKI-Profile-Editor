### Server gives error Mod SPT-AKI-ProfileEditor is missing package.json
This is not a mod, but a separate program. It does not need to be unpacked into the server mods folder.

### The program displays the message: "The selected path does not seem to be a SPT-AKI server location."
Make sure that folder user\profiles exist in the server folder. Before using the program, you must start the server, create a profile in the launcher, and go into the game at least once.

Make sure you are using the correct version of the program for your SPT-AKI server. If the [README](ENGREADME.md)) does not indicate compatibility with your version of the server, wait for an update. Or you can try editing the relative paths in the AppSettings.json file, but success is not guaranteed!

### Some items are missing in the "Adding items" window
Many items are not available for adding to the stash because they can break the profile. Maybe this will change in future updates, maybe not.

### Antivirus \ Windows Defender \ SmartScreen considers this program to be suspicious
This program is provided open source and does not contain any viruses, provided that you have downloaded the program from this GitHub repository. If you do not trust this repository, stop using the program.

### The statuses of individual quests are not saved
The server checks the quest statuses for compliance with various conditions (level, other quests, etc.), and corrects them. Therefore, change the status of the quest after unlocking it in the game, or set all profile characteristics to maximum.
#### UPD: You can turn off checking quest statuses in [ALLINONE MOD](https://hub.sp-tarkov.com/files/file/1-allinone-mod/)
