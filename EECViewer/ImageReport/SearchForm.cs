using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECViewer.ImageReport
{
    public class SearchForm : SearchFormBase
    {
        public SearchForm()
        {
            DocumentTypeValue = "18782-3";
            Text = "影像報告查詢";
        }

        protected override void ShowDetailView(ViewModel model)
        {
            base.ShowDetailView(model);
        }
    }
}
