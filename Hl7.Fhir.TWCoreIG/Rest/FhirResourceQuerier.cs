using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Rest
{
    public abstract class FhirResourceQuerier
    {
        public static string GetCriteriaIdentifier(string codeSystem, string code)
        {
            return $"identifier={codeSystem}|{code}";
        }
        public static SearchParams CreateSearchParams()
        {
            var q = new SearchParams();
            return q;
        }

    }
    /// <summary>
    /// FHIR資源查詢類別
    /// </summary>
    public class FhirResourceQuerier<T> : FhirResourceQuerier
        where T : Resource, new()
    {
        protected readonly FhirClient client;

        public FhirResourceQuerier(FhirClient client)
        {
            this.client = client;
        }

        public T[] Search(params string[] criteria)
        {
            return ClientExtension.Search<T>(client, criteria);
        }

        public T[] Search(int? pageSize, params string[] criteria)
        {
            return ClientExtension.Search<T>(client, pageSize, criteria);
        }

        public T[] Search(SearchParams q)
        {
            return client.Search<T>(q).ToResources<T>();
        }

        public T Get(string id)
        {
            return ClientExtension.Get<T>(client, id);
        }
        public T[] SearchByIdentifier(string codeSystem, string code)
        {
            var q = CreateSearchParams().Where(GetCriteriaIdentifier(codeSystem, code));
            return Search(q);
        }

        public T GetByIdentifier(string codeSystem, string code)
        {
            //return SearchByIdentifier(codeSystem, code).SingleOrDefault();
            return SearchByIdentifier(codeSystem, code).FirstOrDefault();
        }

    }
}
