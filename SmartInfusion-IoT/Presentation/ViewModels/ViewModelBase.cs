using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ReactiveUI;

namespace SmartInfusion_IoT.Presentation.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        private bool _isBusy;
        private ContentDialog _contentDialog;

        protected ViewModelBase()
        {
            _contentDialog = new ContentDialog();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        protected async Task ShowErrorAsync(string message)
        {
            _contentDialog.Title = "Error";
            var textBlock = new TextBlock { Text = message };
            _contentDialog.Content = textBlock;

            _contentDialog.CloseButtonText = "OK";
            
            await _contentDialog.ShowAsync();
        }

        protected void OnIsInProgressChanges(bool isBusy)
        {
            IsBusy = isBusy;
        }
    }
}
