### 编辑 \ 创建本地化文件:
[俄语版本](/Guidelines/LocalizationsRu.md)

#### 更新: 2.4版本后，你可以在设置中自行编辑本地化文件。

本地化文件会在应用首次运行时创建于 [Localizations](/CHFAQ.md#我在哪里可以找到应用程序的-appsettingsjson-文件日志本地化和备份) 文件夹。
本地化文件在被创建后，你就可以随时编辑文件。
当程序启动时，本地化文件将被自动加载并显示在设置对话框的相应字段中。

本地化文件是一个 JSON 文件，其结构如下：
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

字段"Key"：本地化关键字（key），必须与服务器"dir_globals"文件夹中的 JSON 本地化文件匹配。
```json
\\ AppSettings.json
  "DirsList": {
    "dir_globals": "Aki_Data\\Server\\database\\locales\\global",
  }
```

字段"name"：要在设置对话框的相应字段中显示的本地化名称。
字段"Translations"：字符串集"key":"value"。 翻译后的短语被输入到“值”字段中。

编辑 \ 创建本地化文件后，建议在 [https://jsonformatter.curiousconcept.com/](https://jsonformatter.curiousconcept.com/) （或任何其他 JSON 验证器）检查其格式是否合法。