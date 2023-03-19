using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class TemplateEntity: BindableEntity
    {
        public TemplateEntity(string id,
                              Dictionary<string, IComparable> values,
                              List<TemplateEntity> templateEntities,
                              string localizedName,
                              Dictionary<string, IComparable> startValues,
                              RelayCommand revertCommand)
        {
            Id = id;
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
                        changedValues.Add(new ChangedValue(value.Key,
                                                           localizedkey,
                                                           startValue,
                                                           value.Value,
                                                           revertCommand));
                    }
                }
                ChangedValues = changedValues != null ? new(changedValues) : null;
            }
        }

        [JsonConstructor]
        public TemplateEntity(string id,
                              List<ChangedValue> changedValues,
                              List<TemplateEntity> templateEntities)
        {
            Id = id;
            ChangedValues = changedValues != null ? new(changedValues) : null;
            TemplateEntities = templateEntities;
        }

        public string Id { get; }
        public ObservableCollection<ChangedValue> ChangedValues { get; private set; }
        public List<TemplateEntity> TemplateEntities { get; }

        [JsonIgnore]
        public bool NotEmpty => ChangedValues?.Count > 0 || TemplateEntities?.Count > 0;

        [JsonIgnore]
        public string LocalizedName { get; }

        [JsonIgnore]
        public RelayCommand RevertChangedValue => new(obj =>
        {
            if (obj is ChangedValue value)
            {
                value.RevertCommand?.Execute(value.Name);
                ChangedValues?.Remove(value);
                if (ChangedValues?.Count == 0)
                    ChangedValues = null;
                OnPropertyChanged(nameof(ChangedValues));
            }
        });

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
        public ChangedValue(string name,
                            string localizedName,
                            IComparable startValue,
                            IComparable newValue,
                            RelayCommand revertCommand)
        {
            Name = name;
            LocalizedName = localizedName;
            StartValue = startValue;
            NewValue = newValue;
            RevertCommand = revertCommand;
        }

        public string Name { get; }

        [JsonIgnore]
        public string LocalizedName { get; }

        [JsonIgnore]
        public IComparable StartValue { get; }

        public IComparable NewValue { get; }

        [JsonIgnore]
        public RelayCommand RevertCommand { get; }
    }
}