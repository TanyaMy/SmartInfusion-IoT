using ReactiveUI;
using SmartInfusion_IoT.Business.Services;
using SmartInfusion_IoT.Data.Entities.DiseaseHistory;
using SmartInfusion_IoT.Data.Entities.Treatment;
using SmartInfusion_IoT.Infrastructure;
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
        private StepperMotorHelper _stepperMotorHelper;

        private ReactiveList<DiseaseHistoryListItemModel> _diseaseHistoryList;
        private DiseaseHistoryListItemModel _selectedDiseaseHistory;
        private ReactiveList<TreatmentListItemModel> _treatmentList;
        private TreatmentListItemModel _selectedTreatment;
        private bool _infusionIsInProgress;
        private bool _isTreatmentSelected;
        private double _infusionSpeed;
        private double _dosage;
        private double _solutionVolume;
        private double _patientWeight;

        public StartInfusionProcessViewModel(
            IDiseaseHistoryService diseaseHistoryService,
            ITreatmentService treatmentService)
        {
            _diseaseHistoryService = diseaseHistoryService;
            _treatmentService = treatmentService;

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
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedTreatment, value);
                if (_selectedTreatment != null)
                {
                    IsTreatmentSelected = true;
                    InfusionSpeed = _selectedTreatment.InfusionSpeed;
                    Dosage = _selectedTreatment.Dosage;
                    SolutionVolume = _selectedTreatment.SolutionVolume;
                    PatientWeight = _selectedTreatment.PatientWeight;
                }
                else
                {
                    IsTreatmentSelected = false;
                    InfusionSpeed = 0;
                    Dosage = 0;
                    SolutionVolume = 0;
                    PatientWeight = 0;
                }
            }
        }

        public bool InfusionIsInProgress
        {
            get => _infusionIsInProgress;
            set => this.RaiseAndSetIfChanged(ref _infusionIsInProgress, value);
        }

        public bool IsTreatmentSelected
        {
            get => _isTreatmentSelected;
            set => this.RaiseAndSetIfChanged(ref _isTreatmentSelected, value);
        }

        public double InfusionSpeed
        {
            get => _infusionSpeed;
            set => this.RaiseAndSetIfChanged(ref _infusionSpeed, value);
        }

        public double Dosage
        {
            get => _dosage;
            set => this.RaiseAndSetIfChanged(ref _dosage, value);
        }

        public double SolutionVolume
        {
            get => _solutionVolume;
            set => this.RaiseAndSetIfChanged(ref _solutionVolume, value);
        }

        public double PatientWeight
        {
            get => _patientWeight;
            set => this.RaiseAndSetIfChanged(ref _patientWeight, value);
        }

        public ReactiveCommand StartInfusionCommand { get; set; }

        public ReactiveCommand StopInfusionCommand { get; set; }

        public ReactiveCommand RestartInfusionCommand { get; set; }

        public ReactiveCommand RefreshDiseaseHistoryList { get; set; }

        public ReactiveCommand RefreshTreatmentList { get; set; }

        private async void Init()
        {
            StartInfusionCommand = ReactiveCommand.CreateFromTask(StartInfusionCommandExecuted);
            StopInfusionCommand = ReactiveCommand.Create(StopInfusionCommandExecuted);
            RestartInfusionCommand = ReactiveCommand.Create(RestartInfusionCommandExecuted);
            RefreshDiseaseHistoryList = ReactiveCommand.CreateFromTask(RefreshHistoriesListCommandExecuted);
            RefreshTreatmentList = ReactiveCommand.CreateFromTask(RefreshTreatmentListCommandExecuted);

            DiseaseHistoryList = new ReactiveList<DiseaseHistoryListItemModel>(
                await LoadDiseaseHistoryListAsync());

            TreatmentList = new ReactiveList<TreatmentListItemModel>();
        }

        private async Task StartInfusionProcess()
        {
            if (IsBusy) return;

            if (_stepperMotorHelper == null || SelectedTreatment == null)
            {
                _stepperMotorHelper = new StepperMotorHelper(SelectedTreatment);
            }
            
            await _stepperMotorHelper.StartInfusion();

            OnIsInProgressChanges(false);
        }

        private void StopInfusionProcess()
        {
            _stepperMotorHelper.StopInfusion();
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

        private void StopInfusionCommandExecuted()
        {
            InfusionIsInProgress = false;
            StopInfusionProcess();
        }

        private async Task RestartInfusionCommandExecuted()
        {
            if (_stepperMotorHelper == null)
            {
                _stepperMotorHelper = new StepperMotorHelper();
            }
            await _stepperMotorHelper.ReturnToStart();
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
