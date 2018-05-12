using System.Collections.Generic;

namespace SmartInfusion_IoT.Data.Entities.Treatment
{
    public class TreatmentListModel
    {
        public ICollection<TreatmentListItemModel> Treatments { get; set; }
    }
}
