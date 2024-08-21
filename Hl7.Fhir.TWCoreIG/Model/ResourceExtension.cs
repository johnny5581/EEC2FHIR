using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Model
{
    public static class ResourceExtension
    {
        /// <summary>
        /// 取得reference
        /// </summary>        
        public static ResourceReference GetReference(this Resource resource, ResourceReferenceType refType = ResourceReferenceType.Normal)
        {
            var reference = new ResourceReference();

            reference.Type = resource.TypeName;
            switch(refType)
            {
                case ResourceReferenceType.Normal:
                    reference.Reference = $"{resource.TypeName}/{resource.Id}";
                    break;
                case ResourceReferenceType.FullUri:
                    reference.Reference = $"{resource.ResourceBase}{resource.TypeName}/{resource.Id}";
                    break;
                case ResourceReferenceType.IdOnly:
                    reference.Reference = $"#{resource.Id}";
                    break;
            }
                
            return reference;
        }

        public static Bundle AppendEntryResource(this Bundle bundle, Resource res)
        {
            var entry = new Bundle.EntryComponent();
            entry.FullUrl = res.GetReference(ResourceReferenceType.FullUri).Reference;
            entry.Resource = res;
            bundle.Entry.Add(entry);
            return bundle;
        }

        public static void SetMetaProfile(this Resource res, string profile)
        {
            res.Meta = new Meta();
            res.Meta.Profile = new string[] { profile };
        }

        public static string GetIdentifier(this Identifier identifier, bool full = false)
        {            
            if (full)
                return $"{identifier.Value} ({identifier.System})";
            return identifier.Value;
        }
        public static string GetIdentifier(this List<Identifier> identifiers, string codeSystem, bool full = false)
        {
            var identifier = identifiers.FirstOrDefault(r => r.System == codeSystem);
            return GetIdentifier(identifier, full);
        }

        public static string GetIdentifier(this Resource resource, string codeSystem, bool full = false)
        {
            var p = resource.GetType().GetProperty("Identifier");
            if (p != null)
            {
                var value = p.GetValue(resource, null);                
                if (value is Identifier)
                {
                    var identifier = value as Identifier;
                    if(identifier.System == codeSystem)
                        return GetIdentifier(identifier, full);
                }
                else if (value is List<Identifier>)
                    return GetIdentifier(value as List<Identifier>, codeSystem, full);
            }
            return null;
        }

        
        public static string ToText(this HumanName humanName)
        {
            return humanName.Family + humanName.Given.FirstOrDefault();
        }

        public static string ToText(this List<HumanName> humanNames, HumanName.NameUse use = HumanName.NameUse.Official)
        {
            var humanName = humanNames.FirstOrDefault(r => r.Use == use);
            return ToText(humanName);
        }

        public static string ToText(this Coding coding)
        {
            return coding.Display ?? $"{coding.Code} ({coding.System})";
        }

        public static string ToText(this CodeableConcept codeableConcept)
        {
            return codeableConcept.Text ?? codeableConcept.Coding.FirstOrDefault()?.ToText();
        }
    }

    public enum ResourceReferenceType
    {
        FullUri,
        Normal,
        IdOnly,
    }
}
