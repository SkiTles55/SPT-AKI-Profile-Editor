using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public abstract class TemplateableEntity : BindableEntity
    {
        private readonly Dictionary<string, IComparable> changedValues = new();
        private readonly Dictionary<string, IComparable> startValues = new();

        public abstract string TemplateEntityId { get; }

        public List<TemplateEntity> GetAllChanges()
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

        public void ApplyTemplates(List<TemplateEntity> templates)
        {
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                if (property.GetValue(this, null) is TemplateableEntity entity)
                {
                    var template = templates.Where(x => x.Id == entity.TemplateEntityId).FirstOrDefault();
                    if (template != null)
                        entity.ApplyTemplate(template);
                }
            }
        }

        public TemplateEntity GetTemplateEntity() => new(TemplateEntityId,
                                                         changedValues,
                                                         GetAllChanges());

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

        //protected void SetProperty<T>(string name, ref T? oldValue, T? newValue) where T : struct, IComparable<T>
        //{
        //    if (oldValue.HasValue != newValue.HasValue || (newValue.HasValue && oldValue.Value.CompareTo(newValue.Value) != 0))
        //    {
        //        Debug.WriteLine($"Changed: {name} - from {oldValue} to {newValue}");
        //        oldValue = newValue;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        private static TemplateEntity GetTemplateEntity(TemplateableEntity item)
        {
            var changedValues = item.changedValues;
            var templateEntities = item.GetAllChanges();
            if (changedValues?.Count > 0 || templateEntities?.Count > 0)
                return new(item.TemplateEntityId,
                           changedValues?.Count > 0 ? changedValues : null,
                           templateEntities?.Count > 0 ? templateEntities : null);
            return null;
        }

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
                           changedValues.Count > 0 ? changedValues : null);
            return null;
        }

        private void ApplyTemplate(TemplateEntity template)
        {
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                var changedValue = template
                    .Values?
                    .Where(x => x.Key == property.Name)
                    .FirstOrDefault()
                    .Value;
                if (changedValue != null)
                    property.SetValue(this, Convert.ChangeType(changedValue, property.PropertyType));

                var propertyValue = property.GetValue(this, null);
                if (propertyValue is TemplateableEntity entity)
                    ApplyTemplateToEntity(template, entity);

                if (propertyValue is IEnumerable<TemplateableEntity> entityList)
                    ApplyTemplateToEntityList(template, property, entityList);
            }
        }

        private bool NeedSkip<T>(string name, T newValue) where T : IComparable<T>
        {
            if (!startValues.ContainsKey(name))
            {
                startValues.Add(name, (IComparable)newValue);
                return true;
            }
            return false;
        }
    }
}