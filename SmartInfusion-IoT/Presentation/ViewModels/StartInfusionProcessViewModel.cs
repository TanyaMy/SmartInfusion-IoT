using ReactiveUI;
using SmartInfusion_IoT.Business.Services;
using SmartInfusion_IoT.Data.Entities.DiseaseHistory;
using SmartInfusion_IoT.Data.Entities.Treatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Presentation.ViewModels
{
    public class StartInfusionProcessViewModel : ViewModelBase
    {
        private readonly IDiseaseHistoryService _diseaseHistoryService;
        private readonly ITreatmentService _treatmentService;
        private readonly Uln2003Driver _uln2003Driver;

        private ReactiveList<DiseaseHistoryListItemModel> _diseaseHistoryList;
        private DiseaseHistoryListItemModel _selectedDiseaseHistory;
        private ReactiveList<TreatmentListItemModel> _treatmentList;
        private TreatmentListItemModel _selectedTreatment;
        private bool _infusionIsInProgress;

        public StartInfusionProcessViewModel(
            IDiseaseHistoryService diseaseHistoryService,
            ITreatmentService treatmentService)
        {
            _diseaseHistoryService = diseaseHistoryService;
            _treatmentService = treatmentService;
            // _uln2003Driver = new Uln2003Driver(26, 13, 6, 5);

            Init();
        }

        public ReactiveList<DiseaseHistoryListItemModel> DiseaseHistoryList
        {
            get => _diseaseHistoryList;
            set => this.RaiseAndSetIfChanged(ref _diseaseHistoryList, value);
        }

        public DiseaseHistoryListItemModel SelectedDiseaseHistory
        {
            get => _selectedDiseaseHistory;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDiseaseHistory, value);
                if (_selectedDiseaseHistory != null)
                {
                    RefreshTreatmentListCommandExecuted();
                }
            }
        }

        public ReactiveList<TreatmentListItemModel> TreatmentList
        {
            get => _treatmentList;
            set => this.RaiseAndSetIfChanged(ref _treatmentList, value);
        }

        public TreatmentListItemModel SelectedTreatment
        {
            get => _selectedTreatment;
            set => this.RaiseAndSetIfChanged(ref _selectedTreatment, value);
        }

        public bool InfusionIsInProgress
        {
            get => _infusionIsInProgress;
            set => this.RaiseAndSetIfChanged(ref _infusionIsInProgress, value);
        }

        public ReactiveCommand StartInfusionCommand { get; set; }

        public ReactiveCommand StopInfusionCommand { get; set; }

        public ReactiveCommand RefreshDiseaseHistoryList { get; set; }

        public ReactiveCommand RefreshTreatmentList { get; set; }

        private async void Init()
        {
            StartInfusionCommand = ReactiveCommand.CreateFromTask(StartInfusionCommandExecuted);
            StopInfusionCommand = ReactiveCommand.CreateFromTask(StopInfusionCommandExecuted);
            RefreshDiseaseHistoryList = ReactiveCommand.CreateFromTask(RefreshHistoriesListCommandExecuted);
            RefreshTreatmentList = ReactiveCommand.CreateFromTask(RefreshTreatmentListCommandExecuted);

            DiseaseHistoryList = new ReactiveList<DiseaseHistoryListItemModel>(
                await LoadDiseaseHistoryListAsync());

            TreatmentList = new ReactiveList<TreatmentListItemModel>();
        }

        private async Task StartInfusionProcess()
        {
            if (IsBusy) return;

            OnIsInProgressChanges(false);
        }

        private async Task StopInfusionProcess()
        {
            OnIsInProgressChanges(false);
        }

        private async Task StartInfusionCommandExecuted()
        {
            if (await Validate())
            {
                InfusionIsInProgress = true;
                await StartInfusionProcess();
            }
        }

        private async Task StopInfusionCommandExecuted()
        {
            InfusionIsInProgress = false;
            await StopInfusionProcess();
        }

        private async Task RefreshHistoriesListCommandExecuted()
        {
            SelectedTreatment = null;
            DiseaseHistoryList = new ReactiveList<DiseaseHistoryListItemModel>(
                await LoadDiseaseHistoryListAsync());
        }

        private async Task RefreshTreatmentListCommandExecuted()
        {
            if (await Validate())
            {
                TreatmentList = new ReactiveList<TreatmentListItemModel>(
                await LoadTreatmentListAsync());
            }
        }

        private async Task<bool> Validate()
        {
            if (SelectedDiseaseHistory == null)
            {
                await ShowErrorAsync("Firstly, choose correct disease history.");
                return false;
            }

            return true;
        }

        private async Task<List<DiseaseHistoryListItemModel>> LoadDiseaseHistoryListAsync()
        {
            OnIsInProgressChanges(true);

            try
            {
                var diseaseHistoryListResponse = await _diseaseHistoryService.GetDiseaseHistoryListAsync();

                if (!diseaseHistoryListResponse.IsValid)
                {
                    await ShowErrorAsync(string.IsNullOrEmpty(diseaseHistoryListResponse.ErrorMessage)
                        ? "Load Disease Histories Failed."
                        : diseaseHistoryListResponse.ErrorMessage);
                    return null;
                }

                return diseaseHistoryListResponse.Content.DiseaseHistoryList.ToList();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);
            }
            finally
            {
                OnIsInProgressChanges(false);
            }

            return Enumerable.Empty<DiseaseHistoryListItemModel>().ToList();
        }

        private async Task<List<TreatmentListItemModel>> LoadTreatmentListAsync()
        {
            OnIsInProgressChanges(true);

            try
            {
                var treatmentListResponse = await _treatmentService.GetTreatmentListAsync(_selectedDiseaseHistory.Id);

                if (!treatmentListResponse.IsValid)
                {
                    await ShowErrorAsync(string.IsNullOrEmpty(treatmentListResponse.ErrorMessage)
                        ? "Load Treatments Failed."
                        : treatmentListResponse.ErrorMessage);
                    return null;
                }

                return treatmentListResponse.Content.Treatments.ToList();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);
            }
            finally
            {
                OnIsInProgressChanges(false);
            }

            return Enumerable.Empty<TreatmentListItemModel>().ToList();
        }
    }
}
