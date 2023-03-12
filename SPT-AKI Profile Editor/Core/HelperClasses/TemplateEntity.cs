using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class TemplateEntity
    {
        public TemplateEntity(string id,
                              Dictionary<string, IComparable> values,
                              List<TemplateEntity> templateEntities,
                              string localizedName,
                              Dictionary<string, IComparable> startValues)
        {
            Id = id;
            Values = values;
            TemplateEntities = templateEntities;
            LocalizedName = localizedName;
            if (values != null && startValues != null)
            {
                List<ChangedValue> changedValues = new();
                foreach (var value in values)
                {
                    if (startValues.ContainsKey(value.Key))
                    {
                        var startValue = startValues[value.Key];
                        var localizedkey = TemplateEntityLocalizationHelper.GetValueKeyLocalizedName(value.Key);
                        changedValues.Add(new ChangedValue(localizedkey,
                                                           startValue,
                                                           value.Value));
                    }
                }
                ChangedValues = changedValues;
            }
        }

        public string Id { get; }
        public Dictionary<string, IComparable> Values { get; }
        public List<TemplateEntity> TemplateEntities { get; }

        [JsonIgnore]
        public bool NotEmpty => Values?.Count > 0 || TemplateEntities?.Count > 0;

        [JsonIgnore]
        public string LocalizedName { get; }

        [JsonIgnore]
        public List<ChangedValue> ChangedValues { get; }


        private static JsonSerializerSettings SerializerSettings => new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static TemplateEntity Load(string path)
        {
            try
            {
                return JsonConvert.DeserializeObject<TemplateEntity>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {

                Logger.Log($"Template load error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public void Save(string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, SerializerSettings);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {

                Logger.Log($"Template save error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }

    public class ChangedValue
    {
        public ChangedValue(string name, IComparable startValue, IComparable newValue)
        {
            Name = name;
            StartValue = startValue;
            NewValue = newValue;
        }

        public string Name { get; }
        public IComparable StartValue { get; }
        public IComparable NewValue { get; }
    }
}