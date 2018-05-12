using Autofac;
using SmartInfusion_IoT.Business.Services;
using SmartInfusion_IoT.Business.Services.Implementations;
using SmartInfusion_IoT.Data.Api.APIs;
using SmartInfusion_IoT.Data.Api.APIs.Implementations;
using SmartInfusion_IoT.Presentation.ViewModels;

namespace SmartInfusion_IoT.Infrastructure
{
    public static class AutofacRegistrator
    {
        public static void RegisterTypes(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterApis(builder);
            RegisterViewModels(builder);
        }

        private static void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<StartInfusionProcessViewModel>().AsSelf().AsImplementedInterfaces();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<DiseaseHistoryService>().As<IDiseaseHistoryService>();
            builder.RegisterType<TreatmentService>().As<ITreatmentService>();
        }

        private static void RegisterApis(ContainerBuilder builder)
        {
            builder.RegisterType<DiseaseHistoryRestApi>().As<IDiseaseHistoryRestApi>();
            builder.RegisterType<TreatmentRestApi>().As<ITreatmentRestApi>();
        }
    }
}
