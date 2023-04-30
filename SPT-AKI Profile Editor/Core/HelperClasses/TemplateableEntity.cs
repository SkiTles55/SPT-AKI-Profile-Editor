using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public abstract class TemplateableEntity : BindableEntity
    {
        public readonly Dictionary<string, IComparable> changedValues = new();
        public readonly Dictionary<string, IComparable> startValues = new();

        public abstract string TemplateEntityId { get; }

        public abstract string TemplateLocalizedName { get; }

        public RelayCommand RevertChange => new(obj =>
        {
            if (obj is string propertyName && startValues.ContainsKey(propertyName))
            {
                var property = GetType().GetProperty(propertyName);
                property?.SetValue(this, Convert.ChangeType(startValues[propertyName], property.PropertyType));
            }
        });

        public TemplateEntity GetAllChanges()
        {
            var templateEntities = GetChangesList();
            if (changedValues.Count > 0 || templateEntities != null)
                return new(TemplateEntityId,
                           changedValues.Count > 0 ? changedValues : null,
                           templateEntities,
                           TemplateLocalizedName,
                           startValues,
                           null);
            return null;
        }

        public TemplateEntity GetTemplateEntity() => new(TemplateEntityId,
                                                         changedValues,
                                                         GetChangesList(),
                                                         TemplateLocalizedName,
                                                         startValues,
                                                         null);

        public void ApplyTemplate(TemplateEntity template)
        {
            if (template == null || template.Id != TemplateEntityId)
                return;

            foreach (PropertyInfo property in GetType().GetProperties())
            {
                var changedValue = template
                    .ChangedValues?
                    .Where(x => x.Name == property.Name)
                    .FirstOrDefault()?
                    .NewValue;
                if (changedValue != null)
                    property.SetValue(this, Convert.ChangeType(changedValue, property.PropertyType));

                var propertyValue = property.GetValue(this, null);
                if (propertyValue is TemplateableEntity entity)
                    ApplyTemplateToEntity(template, entity);

                if (propertyValue is IEnumerable<TemplateableEntity> entityList)
                    ApplyTemplateToEntityList(template, property, entityList);
            }
        }

        protected void SetProperty<T>(string name, ref T oldValue, T newValue) where T : IComparable<T>
        {
            var skip = NeedSkip(name, newValue);
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
                OnPropertyChanged($"Is{name}Changed");
            }
        }

        private static void ApplyTemplateToEntityList(TemplateEntity template,
                                                      PropertyInfo property,
                                                      IEnumerable<TemplateableEntity> entityList)
        {
            var innerTemplate = template
                .TemplateEntities
                .Where(x => x.Id == property.Name)
                .FirstOrDefault();
            if (innerTemplate != null)
            {
                foreach (var listInnerTemplate in innerTemplate.TemplateEntities)
                {
                    var changedProperty = entityList
                        .Where(x => x.TemplateEntityId == listInnerTemplate.Id)
                        .FirstOrDefault();
                    if (changedProperty != null)
                        changedProperty.ApplyTemplate(listInnerTemplate);
                }
            }
        }

        private static void ApplyTemplateToEntity(TemplateEntity template,
                                                  TemplateableEntity entity)
        {
            var innerTemplate = template
                                    .TemplateEntities
                                    .Where(x => x.Id == entity.TemplateEntityId)
                                    .FirstOrDefault();
            if (innerTemplate != null)
                entity.ApplyTemplate(innerTemplate);
        }

        private static TemplateEntity GetTemplateEntity(TemplateableEntity item)
        {
            var changedValues = item.changedValues;
            var templateEntities = item.GetChangesList();
            if (changedValues?.Count > 0 || templateEntities?.Count > 0)
                return new(item.TemplateEntityId,
                           changedValues?.Count > 0 ? changedValues : null,
                           templateEntities?.Count > 0 ? templateEntities : null,
                           item.TemplateLocalizedName,
                           item.startValues,
                           item.RevertChange);
            return null;
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
        private static TemplateEntity GetTemplateEntity(IEnumerable<TemplateableEntity> item,
                                                        string propertyName)
        {
            var changedValues = item
                .Select(x => x.GetTemplateEntity())
                .Where(x => x.NotEmpty)
                .ToList();
            if (changedValues.Count > 0)
                return new(propertyName,
                           null,
                           changedValues.Count > 0 ? changedValues : null,
                           TemplateEntityLocalizationHelper.GetPropertyLocalizedName(propertyName),
                           null, 
                           null);
            return null;
        }

        private List<TemplateEntity> GetChangesList()
        {
            List<TemplateEntity> templateEntities = new();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                var propertyValue = property.GetValue(this, null);
                TemplateEntity templateEntity = null;

                if (propertyValue is TemplateableEntity entity)
                    templateEntity = GetTemplateEntity(entity);

                if (propertyValue is IEnumerable<TemplateableEntity> entityList)
                    templateEntity = GetTemplateEntity(entityList, property.Name);

                if (templateEntity != null)
                    templateEntities.Add(templateEntity);
            }
            return templateEntities.Count > 0 ? templateEntities : null;
        }

        private bool NeedSkip<T>(string name, T newValue) where T : IComparable<T>
        {
            if (newValue is int check && check == 0 && !startValues.ContainsKey(name))
                return true;

            if (!startValues.ContainsKey(name))
            {
                startValues.Add(name, (IComparable)newValue);
                return true;
            }
            return false;
        }
    }
}