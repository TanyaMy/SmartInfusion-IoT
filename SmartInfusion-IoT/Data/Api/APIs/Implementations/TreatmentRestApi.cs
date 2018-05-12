using SmartInfusion_IoT.Data.Api.Rest;
using SmartInfusion_IoT.Data.Entities.Treatment;
using SmartInfusion_IoT.Presentation.Models;
using System;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Data.Api.APIs.Implementations
{
    public class TreatmentRestApi : RestApiBase, ITreatmentRestApi
    {
        private const string BaseApiAddress = ApiRouting.BaseApiUrl;

        public TreatmentRestApi() : base(new Uri(BaseApiAddress))
        {
        }
        public async Task<ResponseWrapper<TreatmentListModel>> GetTreatmentListAsync(int diseaseHistoryId)
        {
            var response = await Url($"treatment/getNotCompletedTreatments/{diseaseHistoryId}")
                                 .GetAsync<ResponseWrapper<TreatmentListModel>>();
            return response;
        }
    }
}