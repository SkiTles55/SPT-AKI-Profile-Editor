### 服务器抛出 SPT-AKI-ProfileEditor 模组 丢失 package.json 的异常
这不是一个模组（mod），而是一个单独的程序。它不需要解压到服务器 mods 文件夹中。
</br></br>

### 程序提示: "你所选的文件路径似乎不是正确的 SPT-AKI 服务器路径。"
确保服务器根目录中存在文件夹 user\profiles。在使用本程序之前，你必须至少启动一次服务器，让启动器创建存档文件，并至少进入一次游戏。

请确保你使用本程序版本所的兼容的 SPT-AKI 版本。如果 [README](CHREADME.md)) 中声明兼容的版本的范围不在你现在使用的 SPT-AKI 服务器范围内，请等待程序更新。又或者你可以试试编辑 [AppSettings.json](CHFAQ.md#我在哪里可以找到应用程序的-appsettingsjson-文件日志本地化和备份) 中的文件以修改版本签名来尝试骗过软件，但不保证这种办法一定能正常工作！

确保 AppSettings.json 文件中 DirsList 和 FilesList 部分中的所有文件夹和文件都存在于服务器文件夹中，并且具有正确的命名格式（某些非原生 SPT-AKI 版本可能会更改文件名，比如说 Server1.exe 而不是 Server.exe）。
你可以根据需要对这些部分进行必要的更改。

[AppSettings.json](CHFAQ.md#我在哪里可以找到应用程序的-appsettingsjson-文件日志本地化和备份) 文件中 DirsList 和 FilesList 部分的示例：
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
#### SPT-AKI 3.0.0 的 UPD：[存档编辑器不适用于 3.0.0 版本？（链接转到YouTube，你可能需要VPN）](https://youtu.be/XO2r4dG_kpk)
#### 更新：从版本 2.4 开始，程序会出现一个对话框，其中包含在所选文件夹中找不到的文件\文件夹列表。如有必要，您可以在此对话框中编辑相对路径。
</br></br>

### 我在“添加物品”中找不到一些物品（如任务物品）。
许多物品无法添加到仓库中，因为它们可能会破坏配置文件。也许这会在未来的更新中改变，也许不会。
#### 更新：从版本 2.7 开始，您可以使用“所有项目”对话框（“隐藏”-“添加物品”-“所有物品”）。此对话框允许您添加所有物品而无需进行保护约束检查。此功能可能会损坏存档文件，使用风险自负！
</br></br>

### Antivirus \ Windows Defender \ SmartScreen （反病毒程序）认为此程序有病毒风险
该程序是开源的，不包含任何病毒，前提是你已从 GitHub 的发布页面中下载该程序。如果不信任此存储库，请停止使用该程序。
</br></br>

### 任务的修改的单项或多项没有生效
服务器检查任务要求是否符合各种条件（等级、前置任务等），并进行更正。因此，请在游戏中解锁任务后更改任务要求，或将所有配置文件特征设置为最大值。
#### 更新: 您可以在[ALLINONE MOD](https://hub.sp-tarkov.com/files/file/1-allinone-mod/) 中关闭检查任务要求。
#### 更新 2: 从版本 2.2 开始，如果设置中未设置“自动忽略”（“通用解决方案修复存档问题”，位于设置的“额外”区域），程序会自行检查任务要求是否符合大部分服务器条件。
</br></br>

### 商人等级的修改没有生效
PMC的等级低于了商人要求的升级等级。请提升你的PMC等级。
#### 更新: 从版本 2.2 开始，如果设置中未设置“自动忽略”（“通用解决方案修复存档问题”，位于设置的“额外”区域），程序会自行检查商人的级别是否符合 PMC 级别。
</br></br>

### 我在哪里可以找到应用程序的 AppSettings.json 文件、日志、本地化和备份？
#### 应用程序版本 2.2 及更高版本
用户文件夹\AppData\Roaming\SPT-AKI Profile Editor</br>
%AppData%\SPT-AKI Profile Editor （按WIN + R，将此路径复制到出现的窗口中并按Enter键；或者运行程序，设置 - 排障 - 打开数据文件夹）</br>
Backups - 存档备份</br></br>
Localizations - 本地化文件</br>
Logs - 日志</br>
AppSettings.json - 配置文件</br>
#### 应用程序版本 2.0 - 2.1.1:
应用根目录文件夹</br>
Backups - 存档备份</br></br>
Localizations - 本地化文件</br>
Logs - 日志</br>
AppSettings.json - 配置文件</br>
</br></br>

### 程序中仅显示已激活任务
在 设置 - 额外 中打开 “向存档添加缺失任务”
</br></br>

### 程序不显示所有可用的 技能/武器专精
在 设置 - 额外 中打开 “向存档添加缺失的Scav技能 / 向存档添加缺失的武器专精”
</br></br>

### 物品、任务、技能等名称与游戏中的名称不符
游戏中选择的语言与配置文件编辑器中选择的本地化不匹配。如果缺少所需的本地化，您可以使用英语本地化作为基础创建必要的本地化文件： [(编辑 \ 创建本地化文件)](/Guidelines/LocalizationsCH.md)
#### 更新: 从版本 2.4 开始，您可以通过单击所选语言旁边的“+”按钮在设置中创建新的本地化。

### 添加物品时出现 “此物品由模组添加，用“移除模组物品”来移除此类物品。” 异常
一般地，如果仓库的武器带有模组添加的武器配件，则会出现此错误。移除此武器\将其移至某个容器\或使用帮助模组。

### 导入武器预设时出现错误“此文件不包含版本”
SPT-AKI 3.7.0 更改后，从旧版本导出的预设不兼容。但是您可以通过在兼容格式的文本编辑器中打开导出的预设并添加关键字段"type": "weapon"来更新它们。更改后预设文件必须如下所示：
```json
{
  "id": "4fcc7a8c299aa5179f2edf06",
  "name": "TestBuild",
  "root": "1926a17442c1c63c14641ba3",
  "items": [
    // items array
  ],
  "type": "weapon",
  "Parent": "5447b5f14bdc2d61278b4567"
}
```
