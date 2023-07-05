using Newtonsoft.Json;
using System;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class DogtagProperties
    {
        public DogtagProperties(string side, string nickname, int level, string weaponName)
        {
            Nickname = nickname;
            Side = side;
            Level = level;
            Status = "Killed by ";
            WeaponName = weaponName;
        }

        [JsonProperty("AccountId")]
        public string AccountId { get; set; }

        [JsonProperty("ProfileId")]
        public string ProfileId { get; set; }

        [JsonProperty("Nickname")]
        public string Nickname { get; set; }

        [JsonProperty("Side")]
        public string Side { get; set; }

        [JsonProperty("Level")]
        public int Level { get; set; }

        [JsonProperty("Time")]
        public string Time { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("KillerAccountId")]
        public string KillerAccountId { get; set; }

        [JsonProperty("KillerProfileId")]
        public string KillerProfileId { get; set; }

        [JsonProperty("KillerName")]
        public string KillerName { get; set; }

        [JsonProperty("WeaponName")]
        public string WeaponName { get; set; }

        public void UpdateProperties()
        {
            var accountId = AppData.Profile.Characters.Pmc.Aid;
            var id = ExtMethods.GenerateNewId(new string[] { accountId });
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