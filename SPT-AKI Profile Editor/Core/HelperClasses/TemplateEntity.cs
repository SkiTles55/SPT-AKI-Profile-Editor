using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class TemplateEntity
    {
        public TemplateEntity(string id,
                              Dictionary<string, IComparable> values,
                              List<TemplateEntity> templateEntities)
        {
            Id = id;
            Values = values;
            TemplateEntities = templateEntities;
        }

        public string Id { get; }
        public Dictionary<string, IComparable> Values { get; }
        public List<TemplateEntity> TemplateEntities { get; }

        [JsonIgnore]
        public bool NotEmpty => Values?.Count > 0 || TemplateEntities?.Count > 0;
    }
}