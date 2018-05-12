using SmartInfusion_IoT.Data.Entities.Treatment;
using SmartInfusion_IoT.Presentation.Models;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Business.Services
{
    public interface ITreatmentService
    {
        Task<ResponseWrapper<TreatmentListModel>> GetTreatmentListAsync(int diseaseHistoryId);
    }
}
