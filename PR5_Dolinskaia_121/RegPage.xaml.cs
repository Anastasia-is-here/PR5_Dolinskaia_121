using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR5_Dolinskaia_121
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        MainWindow win = MainWindow.Instance;
        public RegPage()
        {
            InitializeComponent();
        }

        private void clear_inputs()
        {
            foreach (Control item in grid.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    ((TextBox)item).Text = String.Empty;
                }
                else if (item.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)item).IsChecked = false;
                }
                else if (item.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)item).Items.Clear();
                }
                else if (item.GetType() == typeof(PasswordBox))
                {
                    ((PasswordBox)item).Password = String.Empty;
                }

            }
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            clear_inputs();
            win.main.Navigate(new AuthPage());
        }

        private void Reg(object sender, RoutedEventArgs e)
        {
            registration(FIO.Text, Login.Text, Password.Password, (bool)female.IsChecked, (bool)male.IsChecked, 
                (bool)Phone.IsMaskCompleted, Phone.Text, Role.Text, Photo.Text);
        }

        public bool registration(string _FIO, string _login, string _password, bool _female, bool _male, bool _phonemask, 
            string _phone, string _role, string _photo)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_FIO))
            {
                errors.AppendLine("Введите ФИО");
            }
            if (string.IsNullOrWhiteSpace(_login))
            {
                errors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(_password))
            {
                errors.AppendLine("Введите пароль");
            }
            if (!_female && !_male)
            {
                errors.AppendLine("Выберите свой пол");
            }
            if (!_phonemask)
            {
                errors.AppendLine("Введите телефон");
            }
            if (string.IsNullOrWhiteSpace(_role))
            {
                errors.AppendLine("Выберите свою роль");
            }
            if (string.IsNullOrWhiteSpace(_photo))
            {
                errors.AppendLine("Введите ссылку на фото");
            }
                if (errors.Length != 0)
            {
                MessageBox.Show(errors.ToString());
                return false;
            }

            using (var db = new BaseEntities())
            {
                var user = db.User.FirstOrDefault(t => t.Login.Equals(_login));
                var user2 = db.User.FirstOrDefault(t => t.PhoneNumber.Equals(_phone));
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return false;
                }
                else if (user2 != null)
                {
                    MessageBox.Show("Пользователь с таким телефоном уже существует");
                    return false;
                }
                else 
                {
                    User userObject = new User { 
                    FIO = _FIO,
                    Login = _login,
                    Password = _password,
                    Sex = _female ? "Ж" : "М",
                    Role = _role,
                    PhoneNumber = _phone,
                    Photo = _photo
                };

                    if (userObject.ID == 0)
                    {
                        db.User.Add(userObject);
                    }
                    try
                    {
                       db.SaveChanges();
                        MessageBox.Show("Регистрация прошла успешно!");
                        clear_inputs();
                        win.main.Navigate(new AuthPage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    
                    return true;
                }
            }

           
            
        }
    }
}
