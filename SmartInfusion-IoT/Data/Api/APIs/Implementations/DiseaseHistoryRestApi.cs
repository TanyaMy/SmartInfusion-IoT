using SmartInfusion_IoT.Data.Api.Rest;
using SmartInfusion_IoT.Data.Entities.DiseaseHistory;
using SmartInfusion_IoT.Presentation.Models;
using System;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Data.Api.APIs.Implementations
{
    public class DiseaseHistoryRestApi : RestApiBase, IDiseaseHistoryRestApi
    {
        private const string BaseApiAddress = ApiRouting.BaseApiUrl;

        public DiseaseHistoryRestApi() : base(new Uri(BaseApiAddress))
        {
        }

        public async Task<ResponseWrapper<DiseaseHistoryDetailsModel>> GetDiseaseHisoryDetailsAsync(int diseaseHistoryId)
        {
            var response = await Url($"diseaseHistory/getDiseaseHistoryDetails/{diseaseHistoryId}")
               .GetAsync<ResponseWrapper<DiseaseHistoryDetailsModel>>();
            return response;
        }

        public async Task<ResponseWrapper<DiseaseHistoryListModel>> GetDiseaseHisoryListAsync()
        {
            var response = await Url("diseaseHistory/getDiseaseHistoriesAnonymus")
                   .GetAsync<ResponseWrapper<DiseaseHistoryListModel>>();
            return response;
        }
    }
}
