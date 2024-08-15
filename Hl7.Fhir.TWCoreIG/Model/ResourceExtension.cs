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
    }

    public enum ResourceReferenceType
    {
        FullUri,
        Normal,
        IdOnly,
    }
}
