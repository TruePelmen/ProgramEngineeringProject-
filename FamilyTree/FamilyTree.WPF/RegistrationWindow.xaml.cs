﻿using FamilyTree.BLL.Interfaces;
using FamilyTree.BLL.Interfeces;
using FamilyTree.BLL.Services;
using FamilyTree.WPF.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FamilyTree.WPF
{
    public partial class RegistrationWindow : Window
    {
        private readonly IUserService _userService;
        private readonly ITreePersonService _treePersonService;
        private readonly ITreeService _treeService;
        private readonly IUserTreeService _userTreeService;
        private readonly IPersonService _personService;
        private const string ErrorMessageRequiredField = "Заповніть усі обов'язкові поля";
        private const string ErrorMessagePasswordMismatch = "Паролі не співпадають";
        private const string ErrorMessageDuplicateLogin = "Користувач з таким логіном вже існує";

        public RegistrationWindow(IUserService userService, ITreeService treeService, IUserTreeService userTreeService,
            IPersonService personService, ITreePersonService treePersonService)
        {
            InitializeComponent();
            InitializeUI();
            _userService = userService;
            _treeService = treeService;
            _userTreeService = userTreeService;
            _personService = personService;
            _treePersonService = treePersonService;
        }

        private void InitializeUI()
        {
            maleOption.IsSelected = true;
            loginTextBox.textBox.TextChanged += LoginTextBox_TextChanged;
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckLoginUniqueness())
            {
                loginTextBox.SuccessBorder();
                messageField.Visibility = Visibility.Hidden;
            }
            else
            {
                loginTextBox.ErrorBorder();
                ShowMessage(ErrorMessageDuplicateLogin);
            }
        }

        private bool CheckLoginUniqueness()
        {
            return ! _userService.FindUserByLogin(loginTextBox.Text);
        }

        private void ShowMessage(string message)
        {
            messageField.Text = message;
            messageField.Visibility = Visibility.Visible;
        }
        private string DetermineGender()
        {
            if (maleOption.IsSelected) { return "male"; }
            else { return "female"; }
        }
        private DateOnly ParseDate()
        {
            string dateString = dateOfBirth.SelectedDate.ToString();
            string dateWithoutTime = dateString.Split(' ')[0];
            return DateOnly.ParseExact(dateWithoutTime, "dd.MM.yyyy", CultureInfo.InvariantCulture);

        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFormValidity())
            {
                _userService.AddUser(loginTextBox.Text, passwordBox.Password);
                int treeId = _treeService.AddTree("Дерево " + lastNameTextBox.Text);
                _userTreeService.AddUserTree(loginTextBox.Text, treeId, "edit");
                int personId = _personService.AddPerson(true, lastNameTextBox.Text, DetermineGender(), null, firstNameTextBox.Text, null, ParseDate(), null);
                _treePersonService.AddTreePerson(treeId, personId);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }
        private bool CheckFieldEmpty()
        {
            return string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Password) ||
                string.IsNullOrWhiteSpace(confirmPasswordBox.Password);
        }
        private bool CheckFormValidity()
        {
            bool isValid = true;

            if (CheckFieldEmpty())
            {
                isValid = false;
                ShowMessage(ErrorMessageRequiredField);
            }

            if (!string.Equals(passwordBox.Password, confirmPasswordBox.Password))
            {
                isValid = false;
                ShowMessage(ErrorMessagePasswordMismatch);
            }

            if(!CheckLoginUniqueness())
            {
                return false;
            }

            return isValid;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(_userService);

            loginWindow.Show();
            Close();
        }

        private void lastNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxLostFocus(lastNameTextBox);
        }

        private void firstNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxLostFocus(firstNameTextBox);
        }

        private void loginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxLostFocus(loginTextBox);
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HandlePasswordBoxLostFocus(passwordBox);
        }

        private void confirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HandlePasswordBoxLostFocus(confirmPasswordBox);
            passwordBox.PasswordChanged += confirmPasswordBox_PasswordChanged;
        }

        private void HandleTextBoxLostFocus(MyTextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ErrorBorder();
                ShowMessage(ErrorMessageRequiredField);
            }
        }

        private void HandlePasswordBoxLostFocus(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                ShowMessage(ErrorMessageRequiredField);
            }
        }

        private void lastNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxGotFocus(lastNameTextBox);
        }

        private void firstNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxGotFocus(firstNameTextBox);
        }

        private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HandleTextBoxGotFocus(loginTextBox);
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HandlePasswordBoxGotFocus(passwordBox);
        }

        private void confirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HandlePasswordBoxGotFocus(confirmPasswordBox);
        }

        private void HandleTextBoxGotFocus(MyTextBox textBox)
        {
            textBox.NormalBorder();
            if (!CheckFieldEmpty())
            {
                messageField.Visibility = Visibility.Hidden;
            }
        }

        private void HandlePasswordBoxGotFocus(PasswordBox passwordBox)
        {
            passwordBox.BorderBrush = new SolidColorBrush(Color.FromRgb(238, 240, 232));
            if (!CheckFieldEmpty())
            {
                messageField.Visibility = Visibility.Hidden;
            }
        }

        private void confirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.Equals(passwordBox.Password, confirmPasswordBox.Password))
            {
                ShowMessage(ErrorMessagePasswordMismatch);
            }
            else
            {
                messageField.Visibility = Visibility.Hidden;
            }
        }
    }
}
