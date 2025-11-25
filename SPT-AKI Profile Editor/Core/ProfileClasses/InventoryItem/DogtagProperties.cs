using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class DogtagProperties
    {
        public DogtagProperties(string side, string nickname, int level)
        {
            Nickname = nickname;
            Side = side;
            Level = level;
            Status = "Killed by ";
            AvailableWeapons = [.. AppData.ServerDatabase.ItemsDB.Values.Where(x => x.IsWeapon)];
            WeaponName = AvailableWeapons.FirstOrDefault()?.Id;
        }

        public string AccountId { get; set; }

        public string ProfileId { get; set; }

        public string Nickname { get; set; }

        public string Side { get; set; }

        public int Level { get; set; }

        public string Time { get; set; }

        public string Status { get; set; }

        public string KillerAccountId { get; set; }

        public string KillerProfileId { get; set; }

        public string KillerName { get; set; }

        public string WeaponName { get; set; }

        [JsonIgnore]
        public List<TarkovItem> AvailableWeapons { get; }

        public void UpdateProperties()
        {
            var accountId = AppData.Profile.Characters.Pmc.Aid;
            var id = ExtMethods.GenerateNewId([accountId]);
            AccountId = id;
            ProfileId = id;
            KillerAccountId = accountId;
            KillerProfileId = AppData.Profile.Characters.Pmc.PmcId;
            Time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
            KillerName = AppData.Profile.Characters.Pmc.Info.Nickname;
            WeaponName += " Name";
        }
    }
}