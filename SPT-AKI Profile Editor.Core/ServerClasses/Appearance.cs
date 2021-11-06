﻿using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    class Appearance
    {
        [JsonPropertyName("head")]
        public string[] Heads { get; set; }
        [JsonPropertyName("voice")]
        public string[] Voices { get; set; }
    }
}