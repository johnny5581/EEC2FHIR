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
        public static ResourceReference GetReference(this Resource resource, bool fullUrl = false)
        {
            var reference = new ResourceReference();

            reference.Type = resource.TypeName;
            if(fullUrl)
                reference.Reference = resource.GetFullUrl();
            else
                reference.Reference = $"{resource.TypeName}/{resource.Id}";            
            return reference;
        }

        public static string GetFullUrl(this Resource resource)
        {
            return $"{resource.ResourceBase}{resource.TypeName}/{resource.Id}";
        }

        public static Bundle AppendEntryResource(this Bundle bundle, Resource res)
        {
            var entry = new Bundle.EntryComponent();
            entry.FullUrl = res.GetFullUrl();
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
}
