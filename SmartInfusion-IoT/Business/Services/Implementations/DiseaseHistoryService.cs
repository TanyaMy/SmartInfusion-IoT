using System.Threading.Tasks;
using SmartInfusion_IoT.Data.Api.APIs;
using SmartInfusion_IoT.Data.Entities.DiseaseHistory;
using SmartInfusion_IoT.Presentation.Models;

namespace SmartInfusion_IoT.Business.Services.Implementations
{
    public class DiseaseHistoryService : IDiseaseHistoryService
    {
        private readonly IDiseaseHistoryRestApi _requestRestApi;

        public DiseaseHistoryService(IDiseaseHistoryRestApi requestRestApi)
        {
            _requestRestApi = requestRestApi;
        }

        public async Task<ResponseWrapper<DiseaseHistoryDetailsModel>> GetDiseaseHistoryDetailsAsync(int diseaseHistoryId)
        {
            return await _requestRestApi.GetDiseaseHisoryDetailsAsync(diseaseHistoryId);
        }

        public async Task<ResponseWrapper<DiseaseHistoryListModel>> GetDiseaseHistoryListAsync()
        {
            return await _requestRestApi.GetDiseaseHisoryListAsync();
        }
    }
}
