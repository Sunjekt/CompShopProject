using CompShopProject.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class ProducerViewModel : ObservableObject
    {
        private readonly IProducersService producersService;
        private readonly IProductsService productsService;
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Producer> Producers { get; set; }
        public ProducerViewModel(IProducersService producersService, IProductsService productsService)
        {
            this.producersService = producersService;
            this.productsService = productsService;

            Categories = new ObservableCollection<Category>();
            Producers = new ObservableCollection<Producer>();

            LoadProducers();
        }

        #region Accessors
        private string _searchedPhrase;
        public string SearchedPhrase
        {
            get { return _searchedPhrase; }
            set
            {
                _searchedPhrase = value;
                if (String.IsNullOrWhiteSpace(_searchedPhrase) && Categories.Count == 0)
                    LoadProducers();
                else
                {
                    Producers.Clear();
                    var sortedProducers = producersService.GetProducersByContaintsLetters(SearchedPhrase);
                    foreach (var producer in sortedProducers)
                        Producers.Add(producer);
                }
                OnPropertyChanged("SearchedPhrase");
            }
        }
        private string _newNameProducer;
        public string NewNameProducer
        {
            get { return _newNameProducer; }
            set
            {
                _newNameProducer = value;
                OnPropertyChanged("NewNameProducer");
            }
        }
        #endregion

        #region Selected objects
        private Producer _selectedProducer;
        public Producer SelectedProducer
        {
            get { return _selectedProducer; }
            set
            {
                _selectedProducer = value;
                OnPropertyChanged("SelectedProducer");
            }
        }
        #endregion

        #region Load data adapter
        private void LoadProducers()
        {
            Producers.Clear();
            var producers = producersService.GetAllProducers();
            foreach (var producer in producers)
                Producers.Add(producer);
        }
        #endregion

        #region Commands
        private readonly RelayCommand _saveChangedProducer;
        public RelayCommand SaveChangedProducer
        {
            get
            {
                return _saveChangedProducer ?? (new RelayCommand(obj =>
                {
                    if (String.IsNullOrWhiteSpace(SelectedProducer.Name))
                        MessageBox.Show($"Название не может быть пустым!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (producersService.UpdateProducer(_selectedProducer) == 1)
                            MessageBox.Show($"{SelectedProducer?.Name} был успешно изменён!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
            }
        }
        private readonly RelayCommand _deleteSelectedProducer;
        public RelayCommand DeleteSelectedProducer
        {
            get
            {
                return _deleteSelectedProducer ?? (new RelayCommand(obj =>
                {
                    var productsList = productsService.GetProductsByProducerId(SelectedProducer.Id);
                    string name = SelectedProducer.Name;
                    if (productsList?.Count > 0)
                    {
                        var result = MessageBox.Show($"{SelectedProducer.Name} связан с {productsList.Count} продуктами! Вы уверены что хотите удалить {SelectedProducer.Name}?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes && SelectedProducer != null)
                        {
                            productsService.DeleteProductsByProducerId(_selectedProducer.Id);
                            producersService.DeleteProducerById(_selectedProducer.Id);
                            Producers.Remove(_selectedProducer);
                            if (Producers.Count > 0)
                                SelectedProducer = Producers[0];
                            MessageBox.Show($"{name} был успешно удален со всеми связанными с ним сущностями!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        var result = MessageBox.Show($"Вы уверены что хотите удалить {SelectedProducer.Name}?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes && SelectedProducer != null)
                        {
                            producersService.DeleteProducerById(SelectedProducer.Id);
                            Producers.Remove(SelectedProducer);
                            if (Producers.Count > 0)
                                SelectedProducer = Producers[0];
                            MessageBox.Show($"{name} был успешно удалён!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }));
            }
        }
        private readonly RelayCommand _addProducer;
        public RelayCommand AddProducer
        {
            get
            {
                return _addProducer ?? (new RelayCommand(obj =>
                {
                    if (!String.IsNullOrWhiteSpace(_newNameProducer))
                    {
                        Producer newProducer = new Producer() { Name = NewNameProducer };
                        try
                        {
                            producersService.AddProducer(newProducer);
                            Producers.Add(newProducer);
                            MessageBox.Show($"{newProducer.Name} был успешно добавлен!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            NewNameProducer = String.Empty;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                        MessageBox.Show("Введите корректное название!", "Attention", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }));
            }
        }
        private readonly RelayCommand _refreshCategories;
        #endregion
    }
}
