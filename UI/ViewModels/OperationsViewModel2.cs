using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using AIS.Warehouse.UI.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AIS.Warehouse.UI.Views;

namespace AIS.Warehouse.UI.ViewModels
{
    public class OperationsViewModel2 : BaseViewModel
    {
        private readonly OperationRepository _repository;

        public ObservableCollection<OperationDto> Operations { get; }
            = new ObservableCollection<OperationDto>();

        private OperationDto _selectedOperation;
        public OperationDto SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                LoadOperations();
            }
        }

        public ICommand AddOperationCommand { get; }
        public ICommand ResetSearchCommand { get; }
        public OperationsViewModel2(OperationRepository repository)
        {
            _repository = repository;

            AddOperationCommand = new RelayCommand(_ => OpenAddOperationWindow());
            ResetSearchCommand = new RelayCommand(_ => ResetSearch());

            LoadOperations();
        }

        private void LoadOperations()
        {
            Operations.Clear();

            List<OperationDto> list;

            if (string.IsNullOrWhiteSpace(SearchText))
                list = _repository.GetJournal();
            else
                list = FilterOperations(SearchText);

            foreach (var op in list)
                Operations.Add(op);
        }

        private void ResetSearch()
        {
            SearchText = string.Empty;
        }

        private List<OperationDto> FilterOperations(string text)
        {
            text = text.ToLower();

            return _repository.GetJournal()
                .Where(op =>
                    op.OperationType != null &&
                    op.OperationType.ToLower().Contains(text))
                .ToList();
        }
        private void OpenAddOperationWindow()
        {
            var window = new AddOperationWindow(
                _repository,
                new ItemRepository()
            );

            if (window.ShowDialog() == true)
                LoadOperations();
        }
    }
}
