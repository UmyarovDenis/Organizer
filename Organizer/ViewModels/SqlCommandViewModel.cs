using ContractLibrary.DataTransfer.Dto;
using ContractLibrary.Exceptions;
using Organizer.Enums;
using Organizer.Services.FileServises;
using Organizer.Services.Local;
using Organizer.Services.Net;
using Organizer.Utils;
using Organizer.ViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.ViewModels
{
    internal sealed class SqlCommandViewModel : BaseViewModel
    {
        private readonly ISqlServiceProxy _sqlProxy;
        private readonly IFileDialogService<string> _fileDialogService;

        private string _sqlExpression;
        private DatabaseDto _databaseInfo;
        private string _sourceName;

        private ObservableCollection<TableDto> _tables;
        private ObservableCollection<FileInfo> _queries;

        private TableDto _selectedTable;
        private DataTable _dataResponse;
        private FileInfo _selectedQuery;

        private ICommand _windowOnLoadCommand;
        private ICommand _executeSqlCommand;
        private ICommand _saveSqlExpressionCommand;
        private ICommand _loadSqlExpressionCommand;
        private ICommand _selectQueryCommand;

        private int _activeTabIndex = 0;
        private string _errorMessage;

        public SqlCommandViewModel(IMessageService<bool> messageService, ISqlServiceProxy sqlServiceProxy,
            IFileDialogService<string> fileDialogService) : base(messageService)
        {
            _sqlProxy = sqlServiceProxy;
            _fileDialogService = fileDialogService; 
        }

        #region BindingProperties

        public string SqlExpression
        {
            get { return _sqlExpression; }
            set
            {
                _sqlExpression = value;
                RaisePropertyChanged();
            }
        }
        public FileInfo SelectedQuery
        {
            get { return _selectedQuery; }
            set
            {
                _selectedQuery = value;
                RaisePropertyChanged();
            }
        }
        public string SourceName
        {
            get { return _sourceName; }
            set
            {
                _sourceName = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<FileInfo> Queries
        {
            get { return _queries; }
            set
            {
                _queries = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<TableDto> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                RaisePropertyChanged();
            }
        }
        public DataTable DataResponse
        {
            get { return _dataResponse; }
            set
            {
                _dataResponse = value;
                RaisePropertyChanged();
            }
        }
        public TableDto SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                RaisePropertyChanged();
            }
        }
        public int ActiveTabIndex
        {
            get { return _activeTabIndex; }
            set
            {
                _activeTabIndex = value;
                RaisePropertyChanged();
            }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand WindowOnLoadCommand
        {
            get
            {
                return _windowOnLoadCommand ??
                    (_windowOnLoadCommand = new RelayCommand(arg => WindowOnLoad()));
            }
        }
        public ICommand ExecuteSqlCommand
        {
            get
            {
                return _executeSqlCommand ??
                    (_executeSqlCommand = new RelayCommand(arg => ExecuteSql()));
            }
        }
        public ICommand SafeSqlExpressionCommand
        {
            get
            {
                return _saveSqlExpressionCommand ??
                    (_saveSqlExpressionCommand = new RelayCommand(arg => SafeSqlExpression()));
            }
        }
        public ICommand LoadSqlExpressionCommand
        {
            get
            {
                return _loadSqlExpressionCommand ??
                    (_loadSqlExpressionCommand = new RelayCommand(arg => LoadSqlExpression()));
            }
        }
        public ICommand SelectQueryCommand
        {
            get
            {
                return _selectQueryCommand ??
                    (_selectQueryCommand = new RelayCommand(arg => SelectQuery()));
            }
        }

        #endregion

        #region CommandImplementations

        private async void WindowOnLoad()
        {
            _databaseInfo = await Task.Run(() => _sqlProxy.GetDatabaseData());
            SourceName = _databaseInfo.SourceName;
            Tables = new ObservableCollection<TableDto>(_databaseInfo.Tables);
            Queries = new ObservableCollection<FileInfo>(FileManager.GetFileInfos("SQLQueries"));
            SelectedTable = Tables.FirstOrDefault();
        }
        private async void ExecuteSql()
        {
            if (string.IsNullOrEmpty(SqlExpression))
                return;

            try
            {
                ActiveTabIndex = (int)SqlResponseTabType.SqlResponse;
                DataResponse = await Task.Run<DataTable>(() => _sqlProxy.ExecuteSqlQuery(SqlExpression));
                ErrorMessage = string.Empty;
            }
            catch(FaultException<SqlFault> ex)
            {
                ActiveTabIndex = (int)SqlResponseTabType.SqlMessage;
                ErrorMessage = ex.Message;
                DataResponse = null;
            }
            catch(Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void SafeSqlExpression()
        {
            try
            {
                if (string.IsNullOrEmpty(SqlExpression))
                    throw new ArgumentNullException("Напишите SQL выражение");

                using (_fileDialogService)
                {
                    _fileDialogService.SafeFileExtension = "SQL запрос (*.sql)|*.sql";

                    string path = _fileDialogService.SaveFileDialog();

                    if (!string.IsNullOrEmpty(path))
                    {
                        Formatter.SaveTextFile(path, SqlExpression);
                    }
                }
            }
            catch(ArgumentNullException ex)
            {
                MessageService.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void LoadSqlExpression()
        {
            try
            {
                using (_fileDialogService)
                {
                    _fileDialogService.OpenFileExtension = "SQL запрос (*.sql)|*.sql";

                    string path = _fileDialogService.OpenFileDialog();

                    if (!string.IsNullOrEmpty(path))
                    {
                        SqlExpression = Formatter.GetFileContent(path);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void SelectQuery()
        {
            try
            {
                SqlExpression = Formatter.GetFileContent(SelectedQuery.FullName);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        #endregion
    }
}
