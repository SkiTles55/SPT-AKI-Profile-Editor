using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterAchievement : BindableEntity
    {
        private bool isReceived;
        private BitmapImage bitmapImage;

        public CharacterAchievement(string id,
                                    Dictionary<string, long> achievements,
                                    string imageUrl,
                                    string rarity)
        {
            Id = id;
            Rarity = rarity;

            if (achievements.TryGetValue(id, out long timestamp))
            {
                Timestamp = timestamp;
                isReceived = true;
            }
            else
            {
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                isReceived = false;
            }

            LoadImage(imageUrl);

            if (AppData.ServerDatabase.LocalesGlobal.TryGetValue(id.NameLowercased(), out var localizedName))
                LocalizedName = localizedName;
            else
                LocalizedName = id;

            if (AppData.ServerDatabase.LocalesGlobal.TryGetValue(id.DescriptionLowercased(), out var localizedDescription))
                LocalizedDescription = localizedDescription;
            else
                LocalizedName = "";
        }

        public string Id { get; }

        public long Timestamp { get; }

        public BitmapImage BitmapImage
        {
            get => bitmapImage;
            set
            {
                bitmapImage = value;
                OnPropertyChanged(nameof(BitmapImage));
            }
        }

        public string Rarity { get; }

        public bool IsReceived
        {
            get => isReceived;
            set
            {
                isReceived = value;
                OnPropertyChanged(nameof(IsReceived));
            }
        }

        public string LocalizedName { get; }

        public string LocalizedDescription { get; }

        private void LoadImage(string imageUrl)
        {
            var imagePath = Path.Combine(AppData.AppSettings.ServerPath,
                            AppData.AppSettings.DirsList[SPTServerDir.achievementImages],
                            Path.GetFileNameWithoutExtension(imageUrl) + ".png");

            if (!File.Exists(imagePath))
                return;

            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();
                if (bitmapImage.CanFreeze)
                    bitmapImage.Freeze();
                BitmapImage = bitmapImage;
            }
            catch { }
        }
    }
}