using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterAchievement : BindableEntity
    {
        private bool isReceived;

        public CharacterAchievement(string id,
                                    Dictionary<string, long> achievements,
                                    string imageUrl,
                                    string side)
        {
            Id = id;
            Side = side;

            if (achievements.TryGetValue(id, out long timestamp)) {
                Timestamp = timestamp;
                isReceived = true;
            } else
            {
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                isReceived = false;
            }

                var imagePath = Path.Combine(AppData.AppSettings.ServerPath,
                    AppData.AppSettings.DirsList[SPTServerDir.achievementImages],
                    Path.GetFileNameWithoutExtension(imageUrl) + ".png");

            if (File.Exists(imagePath))
            {
                try
                {
                    BitmapImage = new BitmapImage(new Uri(imagePath));
                }
                catch { }
            }
        }

        public string Id { get; }
        public long Timestamp { get; }
        public BitmapImage BitmapImage { get; }
        public string Side { get; }

        public bool IsReceived
        {
            get => isReceived;
            set
            {
                isReceived = value;
                OnPropertyChanged(nameof(IsReceived));
            }
        }
    }
}