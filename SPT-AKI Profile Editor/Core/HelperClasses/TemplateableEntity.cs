using System;
using System.Collections.Generic;
using System.Reflection;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public abstract class TemplateableEntity : BindableEntity
    {
        private readonly Dictionary<string, IComparable> changedValues = new();
        private readonly Dictionary<string, IComparable> startValues = new();

        public abstract string TemplateEntityId { get; }
        public abstract string TemplateEntityLocalizedName { get; }

        protected void SetProperty<T>(string name, ref T oldValue, T newValue) where T : IComparable<T>
        {
            var skip = false;
            if (!startValues.ContainsKey(name))
            {
                startValues.Add(name, (IComparable)newValue);
                skip = true;
            }

            if (oldValue == null || oldValue.CompareTo(newValue) != 0)
            {
                if (!skip)
                {
                    if (startValues[name].CompareTo(newValue) == 0)
                        changedValues.Remove(name);
                    else
                    {
                        if (changedValues.ContainsKey(name))
                            changedValues[name] = (IComparable)newValue;
                        else
                            changedValues.Add(name, (IComparable)newValue);
                    }
                }

                oldValue = newValue;
                OnPropertyChanged(name);
            }
        }

        //protected void SetProperty<T>(string name, ref T? oldValue, T? newValue) where T : struct, IComparable<T>
        //{
        //    if (oldValue.HasValue != newValue.HasValue || (newValue.HasValue && oldValue.Value.CompareTo(newValue.Value) != 0))
        //    {
        //        Debug.WriteLine($"Changed: {name} - from {oldValue} to {newValue}");
        //        oldValue = newValue;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        public List<TemplateEntity> GetAllChanges()
        {
            List<TemplateEntity> templateEntities = new();
            foreach (PropertyInfo property in GetType().GetProperties())
                if (property.GetValue(this, null) is TemplateableEntity value)
                    templateEntities.Add(new(TemplateEntityId,
                                             TemplateEntityLocalizedName,
                                             value.changedValues,
                                             value.GetAllChanges()));
            return templateEntities;
        }
    }

    public class TemplateEntity
    {
        public TemplateEntity(string id,
                              string name,
                              Dictionary<string, IComparable> values,
                              List<TemplateEntity> templateEntities)
        {
            Id = id;
            Name = name;
            Values = values;
            TemplateEntities = templateEntities;
        }

        public string Id { get; }
        public string Name { get; }
        public Dictionary<string, IComparable> Values { get; }
        public List<TemplateEntity> TemplateEntities { get; }
    }
}