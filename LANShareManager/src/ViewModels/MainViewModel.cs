using System; // Required for ArgumentNullException, EventHandler
using System.ComponentModel;
using System.Windows.Input; // Required for ICommand, CommandManager
using LANShareManager.Views; // Required for view types

namespace LANShareManager.ViewModels
{
    // Basic ICommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowFolderSharingCommand { get; }
        public ICommand ShowFilePermissionsCommand { get; }
        public ICommand ShowPrinterSharingCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public MainViewModel()
        {
            // Initialize commands
            ShowDashboardCommand = new RelayCommand(p => CurrentView = new DashboardView());
            ShowFolderSharingCommand = new RelayCommand(p => CurrentView = new FolderSharingView());
            ShowFilePermissionsCommand = new RelayCommand(p => CurrentView = new FilePermissionsView());
            ShowPrinterSharingCommand = new RelayCommand(p => CurrentView = new PrinterSharingView());
            ShowSettingsCommand = new RelayCommand(p => CurrentView = new SettingsView());

            // Set initial view
            CurrentView = new DashboardView();
        }
    }
}
