using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class TemplateEntityLocalizationHelper
    {
        public static string GetValueKeyLocalizedName(string propertyName)
        {
            return propertyName switch
            {
                "Nickname" => AppData.AppLocalization.Translations["tab_info_nickname"],
                "Side" => AppData.AppLocalization.Translations["tab_info_side"],
                "Voice" => AppData.AppLocalization.Translations["tab_info_voice"],
                "Level" => AppData.AppLocalization.Translations["tab_info_level"],
                "Experience" => AppData.AppLocalization.Translations["tab_info_experience"],
                "Progress" => AppData.AppLocalization.Translations["tab_skills_exp"],
                _ => propertyName,
            };
        }

        public static string GetPropertyLocalizedName(string propertyName)
        {
            return propertyName switch
            {
                "Common" => AppData.AppLocalization.Translations["tab_skills_title"],
                "Mastering" => AppData.AppLocalization.Translations["tab_mastering_title"],
                _ => propertyName,
            };
        }
    }
}