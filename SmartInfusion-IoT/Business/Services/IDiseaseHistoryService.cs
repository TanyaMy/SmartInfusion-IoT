using SmartInfusion_IoT.Data.Entities.DiseaseHistory;
using SmartInfusion_IoT.Presentation.Models;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Business.Services
{
    public interface IDiseaseHistoryService
    {
        Task<ResponseWrapper<DiseaseHistoryListModel>> GetDiseaseHistoryListAsync();

        Task<ResponseWrapper<DiseaseHistoryDetailsModel>> GetDiseaseHistoryDetailsAsync(int diseaseHistoryId);
    }
}
