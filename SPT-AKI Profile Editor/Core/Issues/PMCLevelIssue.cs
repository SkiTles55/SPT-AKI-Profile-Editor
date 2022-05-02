using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class PMCLevelIssue : ProfileIssue
    {
        public PMCLevelIssue(CharacterTraderStandingExtended trader) : base(trader.Id)
        {
            RequiredLevel = trader.TraderBase.LoyaltyLevels[trader.LoyaltyLevel - 1].MinLevel;
            Description = $"For trader {trader.LocalizedName} needed PMC Level {RequiredLevel}";
        }

        public int RequiredLevel { get; }

        public override Action FixAction => () => { AppData.Profile.Characters.Pmc.Info.Level = RequiredLevel; };

        public override string Description { get; }
    }
}