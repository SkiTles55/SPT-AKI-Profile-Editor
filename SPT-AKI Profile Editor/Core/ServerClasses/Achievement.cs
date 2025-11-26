using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Achievement
    {
        [JsonConstructor]
        public Achievement(string id, string imageUrl)
        {
            Id = id;
            ImageUrl = imageUrl;
            LoadBitmapImage();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public BitmapImage BitmapImage { get; private set; }

        private void LoadBitmapImage()
        {
            var imagePath = Path.Combine(AppData.AppSettings.ServerPath,
                AppData.AppSettings.DirsList[SPTServerDir.achievementImages],
                Path.GetFileNameWithoutExtension(ImageUrl) + ".png");

            if (File.Exists(imagePath))
            {
                try
                {
                    BitmapImage = new BitmapImage(new Uri(imagePath));
                }
                catch { }
            }
        }
    }
}