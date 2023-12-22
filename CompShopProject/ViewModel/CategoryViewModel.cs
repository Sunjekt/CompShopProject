using CompShopProject.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class CategoryViewModel : ObservableObject
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;
        public ObservableCollection<Category> Categories { get; set; }
        public CategoryViewModel(ICategoriesService categoriesService, IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;

            Categories = new ObservableCollection<Category>();
            _selectedCategory = new Category();

            LoadCategories();
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
                    LoadCategories();
                else
                {
                    Categories.Clear();
                    var sortedCategories = categoriesService.GetCategoriesByContaintsLetters(_searchedPhrase);
                    foreach (var category in sortedCategories)
                        Categories.Add(category);
                }
                OnPropertyChanged("SearchedCategoryText");
            }
        }
        private string _newNameCategory;
        public string NewNameCategory
        {
            get { return _newNameCategory; }
            set
            {
                _newNameCategory = value;
                OnPropertyChanged("NewNameCategory");
            }
        }
        #endregion

        #region Selected objects
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        #endregion

        #region Load data adapter
        private void LoadCategories()
        {
            Categories.Clear();
            var categories = categoriesService.GetAllCategories();
            foreach (var category in categories)
                Categories.Add(category);
        }
        #endregion

        #region Commands
        private readonly RelayCommand _saveChangedCategory;
        public RelayCommand SaveChangedCategory
        {
            get
            {
                return _saveChangedCategory ?? (new RelayCommand(obj =>
                {
                    if (String.IsNullOrWhiteSpace(_selectedCategory.Name))
                        MessageBox.Show($"Имя не может быть пустым!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (categoriesService.UpdateCategory(_selectedCategory) == 1)
                            MessageBox.Show($"{_selectedCategory?.Name} был успешно изменен!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
            }
        }
        private readonly RelayCommand _deleteSelectedCategory;
        public RelayCommand DeleteSelectedCategory
        {
            get
            {
                return _deleteSelectedCategory ?? (new RelayCommand(obj =>
                {
                    var productsList = productsService.GetProductsByCategoryId(SelectedCategory.Id);
                    string name = SelectedCategory.Name;
                    if (productsList?.Count > 0)
                    {
                        var result = MessageBox.Show($"{_selectedCategory.Name} связан с {productsList.Count} товарами! Вы уверены что хотите удалить {_selectedCategory.Name}?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes && _selectedCategory != null)
                        {
                            productsService.DeleteProductsByCategoryId(_selectedCategory.Id);
                            categoriesService.DeleteCategory(_selectedCategory.Id);
                            Categories.Remove(_selectedCategory);
                            if (Categories.Count > 0)
                                SelectedCategory = Categories[0];
                            MessageBox.Show($"{name} был успешно удален со всеми связанными с ним товарами!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        var result = MessageBox.Show($"Вы уверены что хотите удалить {_selectedCategory.Name}?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes && _selectedCategory != null)
                        {
                            categoriesService.DeleteCategory(_selectedCategory.Id);
                            Categories.Remove(_selectedCategory);
                            if (Categories.Count > 0)
                                SelectedCategory = Categories[0];
                            MessageBox.Show($"{name} был успешно удален!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }));
            }
        }
        private readonly RelayCommand _addCategory;
        public RelayCommand AddCategory
        {
            get
            {
                return _addCategory ?? (new RelayCommand(obj =>
                {
                    if (!String.IsNullOrWhiteSpace(_newNameCategory))
                    {
                        Category newCategory = new Category() { Name = _newNameCategory, Popularity = 0 };
                        try
                        {
                            categoriesService.AddCategory(newCategory);
                            Categories.Add(newCategory);
                            MessageBox.Show($"{newCategory.Name} был успешно добавлен!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            NewNameCategory = String.Empty;
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
        #endregion
    }
}
