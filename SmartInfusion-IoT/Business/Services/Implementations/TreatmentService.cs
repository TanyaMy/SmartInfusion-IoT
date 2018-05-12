using System.Linq;
using System.Threading.Tasks;
using SmartInfusion_IoT.Data.Api.APIs;
using SmartInfusion_IoT.Data.Entities.Treatment;
using SmartInfusion_IoT.Presentation.Models;

namespace SmartInfusion_IoT.Business.Services.Implementations
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ITreatmentRestApi _requestRestApi;

        public TreatmentService(ITreatmentRestApi requestRestApi)
        {
            _requestRestApi = requestRestApi;
        }

        public async Task<ResponseWrapper<TreatmentListModel>> GetTreatmentListAsync(int diseaseHistoryId)
        {
            // только не из комплитед
            return await _requestRestApi.GetTreatmentListAsync(diseaseHistoryId);
        }
    }
}
