using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class ProfileProgress
    {
        public InfoProgress Info;
        public Merchant[] Merchants;
        public Dictionary<string, QuestStatus> Quests;
        public Dictionary<int, int> Hideout;
        public Dictionary<string, bool> ExaminedItems;
        public string[] Clothing;
        public SkillsProgress CommonSkills;
        public SkillsProgress MasteringSkills;
        public UnlockedInfo Crafts;
        public BuildsData Builds;

        public class InfoProgress
        {
            public Character Pmc;
            public Character Scav;

            public class Character
            {
                public string Nickname;
                public string Side;
                public string Voice;
                public long? Experience;
                public string Head;
                public string Pockets;
                public Health HealthMetrics;

                public class Health(CharacterHealth health)
                {
                    public Metric Head = new(health?.BodyParts?.Head);
                    public Metric Chest = new(health?.BodyParts?.Chest);
                    public Metric Stomach = new(health?.BodyParts?.Stomach);
                    public Metric LeftArm = new(health?.BodyParts?.LeftArm);
                    public Metric RightArm = new(health?.BodyParts?.RightArm);
                    public Metric LeftLeg = new(health?.BodyParts?.LeftLeg);
                    public Metric RightLeg = new(health?.BodyParts?.RightLeg);
                    public Metric Hydration = new(health?.Hydration);
                    public Metric Energy = new(health?.Energy);

                    public class Metric
                    {
                        public float Current;
                        public float Maximum;

                        [JsonConstructor]
                        public Metric(float current, float maximum)
                        {
                            Current = current;
                            Maximum = maximum;
                        }

                        public Metric(CharacterMetric characterMetric)
                        {
                            if (characterMetric == null)
                                return;
                            Current = characterMetric.Current;
                            Maximum = characterMetric.Maximum;
                        }

                        public Metric(CharacterBodyParts.WrappedCharacterMetric characterMetric)
                            : this(characterMetric?.Health)
                        {
                        }
                    }
                }
            }
        }

        public class Merchant(string id,
            bool enabled,
            int level,
            float standing,
            long salesSum)
        {
            public string Id = id;
            public bool Enabled = enabled;
            public int Level = level;
            public float Standing = standing;
            public long SalesSum = salesSum;
        }

        public class SkillsProgress
        {
            public Dictionary<string, float> Pmc;
            public Dictionary<string, float> Scav;
        }

        public class BuildsData
        {
            [JsonConverter(typeof(ValidListConverter<WeaponBuild>))]
            public List<WeaponBuild> WeaponsBuilds;

            [JsonConverter(typeof(ValidListConverter<EquipmentBuild>))]
            public List<EquipmentBuild> EquipmentBuilds;
        }
    }
}