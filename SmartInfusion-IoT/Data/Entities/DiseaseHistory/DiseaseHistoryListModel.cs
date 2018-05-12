using System;
using System.Collections.Generic;

namespace SmartInfusion_IoT.Data.Entities.DiseaseHistory
{
    public class DiseaseHistoryListModel
    {
        public ICollection<DiseaseHistoryListItemModel> DiseaseHistoryList { get; set; }
    }
}
