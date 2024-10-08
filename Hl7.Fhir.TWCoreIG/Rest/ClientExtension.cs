﻿using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Rest
{
    public static class ClientExtension
    {
        public static T[] Search<T>(this FhirClient client, params string[] criteria)
            where T : Resource, new()
        {
            return Search<T>(client, null, criteria);
        }
        public static T[] Search<T>(this FhirClient client, int? pageSize, params string[] criteria)
            where T : Resource, new()
        {
            var bundle = criteria.Length == 0 ? client.Search<T>(pageSize: pageSize) : client.Search<T>(criteria, pageSize: pageSize);
            return ToResources<T>(bundle);
        }

        public static T Get<T>(this FhirClient client, string id)
            where T : Resource, new()
        {
            var bundle = client.SearchById<T>(id);
            return bundle.Entry.FirstOrDefault() as T;
        }

        public static T[] ToResources<T>(this Bundle bundle)
            where T : Resource, new()
        {
            return bundle.Entry.Select(entry => (T)entry.Resource).ToArray();
        }
        public static T GetEntryResource<T>(this Bundle bundle)
            where T : Resource
        {
            var resource = bundle.Entry.FirstOrDefault(r => r.Resource is T);
            return resource?.Resource as T;
        }
        public static T GetEntryResource<T>(this Bundle bundle, string reference)
            where T : Resource
        {
            var resources = bundle.Entry.Where(r => r.Resource is T);
            foreach(var resource in resources)
            {
                if (reference.StartsWith("#"))
                {
                    var id = reference.Substring(1);
                    if (resource.Resource.Id == id)
                        return resource.Resource as T;
                }
                else if(reference.Contains("/"))
                {
                    var id = reference.Substring(reference.LastIndexOf("/") + 1);
                    if (resource.Resource.Id == id)
                        return resource.Resource as T;
                }
            }
            return null;
        }
    }
}
