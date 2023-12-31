﻿using CompShopProject.Core;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Interfaces.Services;
using DomainModel.Models;
using CompShopProject.HashGenerators;

namespace CompShopProject.ViewModel
{
    internal class RegistrationViewModel : ObservableObject
    {
        private readonly IUsersService usersService;
        private readonly IUserImagesService userImagesService;
        private readonly Regex _regexForUsername;
        private readonly Regex _regexForPasswordWeak;
        private readonly Regex _regexForPasswordAverage;
        private readonly Regex _regexForPasswordStrong;
        public RegistrationViewModel()
        {

            _regexForUsername = new Regex("^[a-zA-Z0-9]+$");
            _regexForPasswordWeak = new Regex("^[a-zA-Z0-9]+$");
            _regexForPasswordAverage = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
            _regexForPasswordStrong = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            _foregroundColor = System.Windows.Media.Brushes.Black;
            // Set default user photo
            ImagePath = @"../../Images/defUser.png";
        }

        #region Accessors (helpers for ui design)
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                if (!String.IsNullOrEmpty(value))
                {
                    if (_regexForPasswordStrong.IsMatch(value))
                    {
                        PasswordLineWidth = 300;
                        ForegroundColor = System.Windows.Media.Brushes.Green;
                        PasswordCheckerLabel = "Сильный";
                    }
                    else if (_regexForPasswordAverage.IsMatch(value))
                    {
                        PasswordLineWidth = 150;
                        ForegroundColor = System.Windows.Media.Brushes.Orange;
                        PasswordCheckerLabel = "Средний";
                    }
                    else if (_regexForPasswordWeak.IsMatch(value))
                    {
                        PasswordLineWidth = 80;
                        ForegroundColor = System.Windows.Media.Brushes.Red;
                        PasswordCheckerLabel = "Слабый";
                    }
                }
                else
                    PasswordCheckerLabel = String.Empty;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _password2;
        public string Password2
        {
            get { return _password2; }
            set
            {
                _password2 = value;
                OnPropertyChanged(nameof(Password2));
            }
        }
        private string _secretPhrase;
        public string SecretPhrase
        {
            get { return _secretPhrase; }
            set
            {
                _secretPhrase = value;
                OnPropertyChanged(nameof(SecretPhrase));
            }
        }
        private string _passwordCheckerLabel;
        public string PasswordCheckerLabel
        {
            get { return _passwordCheckerLabel; }
            set
            {
                _passwordCheckerLabel = value;
                OnPropertyChanged(nameof(PasswordCheckerLabel));
            }
        }
        private System.Windows.Media.Brush _foregroundColor;
        public System.Windows.Media.Brush ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                OnPropertyChanged("ForegroundColor");
            }
        }
        private int _passwordLineWidth;
        public int PasswordLineWidth
        {
            get { return _passwordLineWidth; }
            set
            {
                _passwordLineWidth = value;
                OnPropertyChanged(nameof(PasswordLineWidth));
            }
        }
        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
        private string _imagePath;
        public string ImagePath
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
        private readonly RelayCommand _signUp;
        public RelayCommand SignUp
        {
            get
            {
                return _signUp ?? (new RelayCommand(async obj =>
                {
                    if (!_regexForUsername.IsMatch(Name) || String.IsNullOrWhiteSpace(Name))
                        MessageBox.Show("Введите корректное имя!");
                    else if (String.IsNullOrWhiteSpace(Password))
                        MessageBox.Show("Please, input correct password!");
                    else if (Password != Password2)
                        MessageBox.Show("Пароли не сходятся!");
                    else if (!_regexForPasswordAverage.IsMatch(Password))
                        MessageBox.Show("Пароль слишком слабый!");
                    else
                    {
                        User userWithEqualName = usersService.FindUserByName(Name);
                        if (userWithEqualName != null)
                            MessageBox.Show($"Пользователь с именем {Name} уже существует! Попробуйте другое имя.");
                        else
                        {
                            User newUser = new User()
                            {
                                Name = Name,
                                Password = MD5Generator.ProduceMD5Hash(Password),
                                RegistrationDate = DateTime.Now
                            };
                            if (IsAdmin && SecretPhrase.ToUpper() == "SUNJEKT")
                                newUser.RoleId = 1;
                            else if (IsAdmin && SecretPhrase.ToUpper() != "SUNJEKT")
                            {
                                MessageBox.Show("Вы не администратор!");
                                newUser.RoleId = 2;
                            }
                            else
                                newUser.RoleId = 2;
                            usersService.AddUser(newUser);
                            User addedUser = usersService.FindUserByName(Name);
                            if (ImagePath != @"../../Images/defUser.png")
                            {
                                userImagesService.AddImageByUserId(ImagePath, addedUser.Id);
                            }
                            if (newUser.RoleId == 2)
                            {
                                MessageBox.Show($"{newUser.Name} был успешно добавлен!");
                            }
                            else
                                MessageBox.Show($"{newUser.Name} с ролью администратора был успешно добавлен!");
                            App.Current.Windows[1].Close();
                        }

                    }
                }));
            }
        }
        private readonly RelayCommand _backToSignIn;
        public RelayCommand BackToSignIn
        {
            get
            {
                return _backToSignIn ?? (new RelayCommand(obj =>
                {
                    App.Current.Windows[1].Close();
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
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = false;
                    ofd.Title = "Выберите ваше фото";
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.apng;*.avif;*.gif;*.jfif;*.pjpeg";
                    ofd.ShowDialog();
                    ImagePath = ofd.FileName;
                }));
            }
        }
        #endregion
    }
}
