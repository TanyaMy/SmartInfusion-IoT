using SmartInfusion_IoT.Data.Entities.Treatment;
using System.Collections.Generic;


namespace SmartInfusion_IoT.Data.Entities.DiseaseHistory
{
    public class DiseaseHistoryDetailsModel
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public List<TreatmentListItemModel> Treatments { get; set; }

    }
}
