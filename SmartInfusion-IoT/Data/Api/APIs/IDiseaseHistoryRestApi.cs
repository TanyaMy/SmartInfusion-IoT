using SmartInfusion_IoT.Data.Entities.DiseaseHistory;
using SmartInfusion_IoT.Presentation.Models;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Data.Api.APIs
{
    public interface IDiseaseHistoryRestApi
    {
        Task<ResponseWrapper<DiseaseHistoryListModel>> GetDiseaseHisoryListAsync();

        Task<ResponseWrapper<DiseaseHistoryDetailsModel>> GetDiseaseHisoryDetailsAsync(int diseaseHistoryId);
    }
}
