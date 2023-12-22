using CompShopProject.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;

namespace CompShopProject.ViewModel
{
    internal class UsersViewModel : ObservableObject
    {
        private readonly IUsersService usersService;
        private readonly IUserImagesService userImagesService;
        public ObservableCollection<User> Users { get; set; }
        public UsersViewModel(IUsersService usersService, IUserImagesService userImagesService)
        {
            this.usersService = usersService;
            this.userImagesService = userImagesService;

            Users = new ObservableCollection<User>();
            LoadUsers();
            if (Users.Count > 0)
                SelectedUser = Users[0];
        }

        #region Selected Objects
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        private string _searchedPhrase;
        public string SearchedPhrase
        {
            get { return _searchedPhrase; }
            set
            {
                _searchedPhrase = value;
                if (String.IsNullOrWhiteSpace(SearchedPhrase))
                    LoadUsers();
                else
                {
                    Users.Clear();
                    var sortedUsers = usersService.GetUsersByContaintsLetters(SearchedPhrase);
                    foreach (var user in sortedUsers)
                    {
                        user.ImageBytes = userImagesService.GetImageByUserId(user.Id).Image;
                        Users.Add(user);
                    }
                    if (Users.Count > 0)
                        SelectedUser = Users[0];
                }
            }
        }
        #endregion

        #region Load data adapter
        private void LoadUsers()
        {
            Users.Clear();
            var users = usersService.GetAllUsers();
            foreach (var user in users)
            {
                user.ImageBytes = userImagesService.GetImageByUserId(user.Id).Image;
                Users.Add(user);
                if (user.Deleted_at != null)
                    user.Color = "#ff3a31";
                else
                    user.Color = "White";
            }
            if (Users.Count > 0)
                SelectedUser = Users[0];
        }
        #endregion

        #region Commands
        private readonly RelayCommand _deleteUser;
        public RelayCommand DeleteUser
        {
            get
            {
                return _deleteUser ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить пользователя: {SelectedUser.Name}?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        usersService.DeleteUser(SelectedUser.Id);
                        MessageBox.Show("Пользователь был успешно удален!");
                        LoadUsers();
                        if (Users.Count > 0)
                            SelectedUser = Users[0];
                    }
                }));
            }
        }
        #endregion
    }
}
