using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECViewer.Laboratory
{
    public class SearchForm : SearchFormBase
    {
        public SearchForm()
        {
            //DocumentTypeValue = "11503-0";
            Text = "檢驗報告查詢";
        }

        protected override void ShowDetailView(ViewModel item)
        {
            using (var d = new DetailForm(client))
            {
                var model = new DetailForm.ViewData();
                var composition = item.Data["root"] as Composition;
                model.Composition = composition;
                model.Patient = item.Data[composition.Subject.Reference] as Patient;
                model.Encounter = item.Data[composition.Encounter.Reference] as Encounter;
                if (composition.Custodian?.Reference != null)
                    model.Organization = item.Data[composition.Custodian?.Reference] as Organization;
                if (composition.Author.Count > 1)
                    model.Authors = item.Data["authors"] as Practitioner[];
                if (model.Encounter != null && model.Encounter.Participant.Count > 0)
                    model.EncounterPractitioner = item.Data[model.Encounter.Participant[0].Individual.Reference] as Practitioner;
                d.LoadData(model);
                d.ShowDialog();
            }
        }
    }
}
