# SPT-AKI Profile Editor
[RU README version](README.md)

### Редактирование\создание файлов локализации:
[RU version](/Guidlines/LocalizationsRu.md)

Localization files are generated in the Localizations folder when the application is first launched.
Files can be edited and new ones created.
When the application starts, they will be loaded and displayed in the corresponding field of the settings dialog.

The localization file is a JSON file with the following structure:
```json
{
  "Key": "en",
  "Name": "English",
  "Translations": {
    "key1": "Value1",
    "key2": "Value2",
    "key3": "Value3"
  }
}
```

Field "Key": localization key, must match the json localization file from the server's "dir_globals" folder.
```json
\\ AppSettings.json
  "DirsList": {
    "dir_globals": "Aki_Data\\Server\\database\\locales\\global",
  }
```

Field "Name": the name of the localization to be displayed in the corresponding field of the settings dialog.
Field "Translations": string set "key": "value". The translated phrases are entered in the "value" field.

After editing \ creating a localization file, it is recommended to check its validity at [https://jsonformatter.curiousconcept.com/](https://jsonformatter.curiousconcept.com/) (or any other json validator)