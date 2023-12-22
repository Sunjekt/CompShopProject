using CompShopProject.Core;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.HashGenerators;

namespace CompShopProject.ViewModel
{
    internal class ProfileViewModel : ObservableObject
    {
        private User _user;
        private readonly IUsersService _usersRepository;
        private readonly IUserImagesService _userImagesRepository;
        public ProfileViewModel(User currentUser, object image)
        {
            _user = new User()
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Password = currentUser.Password,
                RegistrationDate = currentUser.RegistrationDate,
                Role = currentUser.Role,
                RoleId = currentUser.RoleId,
                UserImage = currentUser.UserImage
            };
            NewName = _user.Name;
            ImagePath = image;
        }

        #region Accessors
        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string _newName;
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }
        private object _imagePath;
        public object ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        #endregion

        #region Commands
        private readonly RelayCommand _saveSettings;
        public RelayCommand SaveSettings
        {
            get
            {
                return _saveSettings ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить внесенные изменения?", "Attention", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (!String.IsNullOrEmpty(NewPassword))
                        {
                            if (!String.IsNullOrEmpty(NewName))
                            {
                                var user = _usersRepository.FindUserByName(NewName);
                                if (user != null)
                                    MessageBox.Show($"Пользователь с именем {NewName} уже существует! Попробуйте другое имя.");
                                else
                                {
                                    _user.Password = MD5Generator.ProduceMD5Hash(NewPassword);
                                    _user.Name = NewName;
                                    _usersRepository.UpdateUser(_user);

                                    if (ImagePath != null)
                                    {
                                        try // If BitMap it will be exception
                                        {
                                            string imgPath = (string)ImagePath;
                                            if (imgPath.Length > 0)
                                            {
                                                byte[] buff;
                                                if (File.Exists(imgPath))
                                                {
                                                    buff = File.ReadAllBytes(imgPath);
                                                    Image img = Image.FromFile(imgPath);
                                                    Bitmap resizedImage = new Bitmap(img, new System.Drawing.Size(256, 256));
                                                    using (var stream = new MemoryStream())
                                                    {
                                                        resizedImage.Save(stream, ImageFormat.Jpeg);
                                                        byte[] bytes = stream.ToArray();
                                                        // User image
                                                        UserImage userImage = new UserImage();
                                                        userImage.FileExtension = Path.GetExtension(imgPath);
                                                        userImage.Image = bytes;
                                                        userImage.Size = bytes.Length;
                                                        userImage.UserId = _user.Id;
                                                        _userImagesRepository.UpdateImageByUserId(_user.Id, userImage);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex) { }
                                    }
                                    MessageBox.Show("Профиль был успешно изменен!");
                                }
                            }
                        }
                    }
                }));
            }
        }
        private readonly RelayCommand _deleteAccount;
        public RelayCommand DeleteAccount
        {
            get
            {
                return _deleteAccount ?? (new RelayCommand(obj =>
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить аккаунт?", "Attention", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //if (_user.UserImage?.ToList().Count > 0)
                        //    _userImagesRepository.DeleteImageByUserId(_user.Id);

                        _usersRepository.DeleteUser(_user.Id);
                        MessageBox.Show("Ваш аккаунт был успешно удалён! Спасибо, что были с нами!");
                        //AuthorizationView authorizationView = new AuthorizationView();
                        //authorizationView.Show();
                        App.Current.MainWindow.Close();
                    }
                }));
            }
        }
        private readonly RelayCommand _openFileDialog;
        public RelayCommand OpenFileDialog
        {
            get
            {
                return _openFileDialog ?? (new RelayCommand(obj =>
                {
                    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                    ofd.Multiselect = false;
                    ofd.Title = "Выберите своё фото";
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.apng;*.avif;*.gif;*.jfif;*.pjpeg";
                    ofd.ShowDialog();
                    if (ofd.FileName.Count() > 0)
                        ImagePath = ofd.FileName;
                    //_productImagesRepository.AddImage(ofd.FileNames);
                }));
            }
        }
        #endregion
    }
}
