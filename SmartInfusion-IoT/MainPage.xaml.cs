using Autofac;
using ReactiveUI;
using SmartInfusion_IoT.Presentation.ViewModels;
using System;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartInfusion_IoT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : IViewFor<StartInfusionProcessViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel",
                typeof(StartInfusionProcessViewModel),
                typeof(MainPage),
                new PropertyMetadata(default(StartInfusionProcessViewModel)));

        public MainPage()
        {
            InitializeComponent();
            ViewModel = App.Container.Resolve<StartInfusionProcessViewModel>();

            this.WhenActivated(CreateBindings);
        }

        private void CreateBindings(Action<IDisposable> d)
        {
            d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.Preloader.IsLoading));

            d(this.BindCommand(ViewModel, vm => vm.StartInfusionCommand, v => v.StartButton));
            d(this.BindCommand(ViewModel, vm => vm.StopInfusionCommand, v => v.StopButton));
            d(this.BindCommand(ViewModel, vm => vm.RestartInfusionCommand, v => v.RestartButton));
            d(this.BindCommand(ViewModel, vm => vm.RefreshDiseaseHistoryList, v => v.RefreshHistoriesButton));
            d(this.BindCommand(ViewModel, vm => vm.RefreshTreatmentList, v => v.RefreshTreatmentsButton));

            d(this.OneWayBind(ViewModel, vm => vm.InfusionIsInProgress, v => v.StartButton.Visibility,
                isInProgress => isInProgress ? Visibility.Collapsed : Visibility.Visible));
            d(this.OneWayBind(ViewModel, vm => vm.InfusionIsInProgress, v => v.StopButton.Visibility,
            isInProgress => isInProgress ? Visibility.Visible : Visibility.Collapsed));
            d(this.OneWayBind(ViewModel, vm => vm.IsTreatmentSelected, v => v.TreatmentInfo.Visibility,
            isSelected => isSelected ? Visibility.Visible : Visibility.Collapsed));

            d(this.OneWayBind(ViewModel, vm => vm.DiseaseHistoryList, v => v.DiseaseHistoryComboBox.ItemsSource));
            d(this.Bind(ViewModel, vm => vm.SelectedDiseaseHistory, v => v.DiseaseHistoryComboBox.SelectedItem));
            d(this.OneWayBind(ViewModel, vm => vm.TreatmentList, v => v.TreatmentComboBox.ItemsSource));
            d(this.Bind(ViewModel, vm => vm.SelectedTreatment, v => v.TreatmentComboBox.SelectedItem));
            
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StartInfusionProcessViewModel)value;
        }

        public StartInfusionProcessViewModel ViewModel
        {
            get => (StartInfusionProcessViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}
