### 言語ファイルを編集 \作成:
[ロシア語版](/Guidelines/LocalizationsRu.md)

#### アップデート：バージョン 2.4 以降、設定で言語ファイルを自分で編集できるようになりました。

言語ファイルには、アプリの初回実行時に [Localizations](/JPFAQ.md#どこでアプリの-appsettingsjson-ログファイルと言語ファイルのバックアップ) フォルダーに作成されます。
言語ファイルが作成された後は、いつでもファイルを編集できます。
アプリが起動すると、言語ファイルが自動的にロードされ、設定ダイアログ ボックスの対応するフィールドで表示されます。

言語ファイルは、次のような構造の JSON ファイルです：
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

フィールド「Key」は：キーワード（key）であり，サーバーの「dir_globals」フォルダー内の JSON 言語ファイルと一致する必要があります。
```json
\\ AppSettings.json
  "DirsList": {
    "dir_globals": "Aki_Data\\Server\\database\\locales\\global",
  }
```

フィールド「Name」: 設定ダイアログの対応するフィールドに表示される翻訳された名前。
フィールド「Translations」: 文字列セット「key」:「value」。 翻訳された語句が「value」フィールドに入力されます。

言語ファイルを作成した後、その形式が [https://jsonformatter.curiousconcept.com/](https://jsonformatter.curiousconcept.com/) (またはその他の JSON バリデーター) で合法かどうかを確認することをお勧めします。