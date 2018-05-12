using SmartInfusion_IoT.Data.Entities.Treatment;
using SmartInfusion_IoT.Presentation.Models;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Data.Api.APIs
{
    public interface ITreatmentRestApi
    {
        Task<ResponseWrapper<TreatmentListModel>> GetTreatmentListAsync(int diseaseHistoryId);
    }
}
