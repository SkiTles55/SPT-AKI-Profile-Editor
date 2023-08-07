import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt-aki/models/external/IPostDBLoadMod";
import { JsonUtil } from "@spt-aki/utils/JsonUtil";
import { DatabaseServer } from "@spt-aki/servers/DatabaseServer";
import { ILogger } from "@spt-aki/models/spt/utils/ILogger";
import { VFS } from "@spt-aki/models/spt/utils/VFS";

class Mod implements IPostDBLoadMod 
{
    logger: ILogger
	
	public postDBLoad(container: DependencyContainer): void 
    {
        this.logger = container.resolve<ILogger>("WinstonLogger");
        const jsonUtil = container.resolve<JsonUtil>("JsonUtil");
        const vfs = container.resolve<VFS>("VFS");
        const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");
		const modName = "SPT-AKI ProfileEditorHelper";
        this.logger.log(`[${modName}] : Started database exporting`, "green");
		
		const exportPath = "user/mods/ProfileEditorHelper/exportedDB":
        const tables = databaseServer.getTables();
		
		vfs.writeFile(`${exportPath}/handbook.json`, jsonUtil.serialize(tables.templates.handbook));
        this.logger.log(`[${modName}] : Handbook exported`, "green");
		
		vfs.writeFile(`${exportPath}/items.json`, jsonUtil.serialize(tables.templates.items));
        this.logger.log(`[${modName}] : Item templates exported`, "green");
		
		vfs.writeFile(`${exportPath}/quests.json`, jsonUtil.serialize(tables.templates.quests));
        this.logger.log(`[${modName}] : Quests exported`, "green");
		
		for (const [traderKey, traderBase] of Object.entries(tables.traders)) 
		{
			vfs.writeFile(`${exportPath}/traders/${traderKey}.json`, jsonUtil.serialize(traderBase.base));
		}
        this.logger.log(`[${modName}] : Traders exported`, "green");
		
		for (const [localeKey, localeDict] of Object.entries(tables.locales.global)) 
		{
			vfs.writeFile(`${exportPath}/locales/${localeKey}.json`, jsonUtil.serialize(localeDict));
		}
        this.logger.log(`[${modName}] : Locales exported`, "green");
		
        this.logger.log(`[${modName}] : DB successfully exported`, "green");
    }
}

module.exports = { mod: new Mod() }
