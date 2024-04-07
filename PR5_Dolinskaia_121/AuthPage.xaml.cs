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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        MainWindow win = MainWindow.Instance;
        int tries = 0;
        public AuthPage()
        {      
            InitializeComponent();
        }

        private void TextBox_Enter(object sender, MouseEventArgs e)
        {
            password.Password = "";
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            Auth(login.Text, password.Password, captcha.Visibility == Visibility.Visible ? captcha_input.Text : null);

        }

        public void CaptchaSwitch()
        {
            Thickness margin = submit.Margin;
            Thickness margin_reg = register.Margin;
            switch (captcha.Visibility)
            {
                case Visibility.Visible:
                    margin.Top = 10;
                    submit.Margin = margin;
                    margin_reg.Top = 70;
                    register.Margin = margin_reg;
                    captcha.Visibility = Visibility.Hidden;
                    captcha_input.Visibility = Visibility.Hidden;
                    return;
                case Visibility.Hidden:
                    margin.Top = 58;
                    submit.Margin = margin;
                    margin_reg.Top = 138;
                    register.Margin = margin_reg;
                    captcha.Visibility = Visibility.Visible;
                    captcha_input.Visibility = Visibility.Visible;
                    return;
            }      
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        public void CaptchaChange()
        {
          
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            captcha.Text = pwd;
        }
        public bool Auth(string login, string password, string _captcha = null)
        {
            if (captcha.Visibility == Visibility.Visible)
            {
                if (_captcha != captcha.Text)
                {
                    MessageBox.Show("Неверно введена каптча", "Ошибка");
                    CaptchaChange();
                    return false;
                }

            }

            if (password == "" || login == "")
            {
                MessageBox.Show("Проверьте заполненность полей", "Ошибка");
                return false;
            }
            
            using (var db = new BaseEntities())
            {
                string l = login.Trim();
                string p = password.Trim(); 

                var user = db.User.FirstOrDefault(t => t.Login.Equals(l) && t.Password.Equals(p));
                
                if (user == null || user.Role == "Удален")
                {
                    MessageBox.Show("Пользователь с такими данными не найден", "Ошибка");
                    
                    tries++;
                    if (tries >= 3)
                    {
                        if (captcha.Visibility != Visibility.Visible) {
                            CaptchaSwitch();
                        }
                        CaptchaChange();
                    }
                    return false;
                }
                else
                {
                    MessageBox.Show($"Добро пожаловать, {user.FIO.Split(' ')[1]}. ({user.Role})", "Успех");
                    if (captcha.Visibility == Visibility.Visible)
                    {
                        CaptchaSwitch();
                        captcha_input.Clear();
                    }
                    tries = 0;
                    return true;
                }
            }
            
        }
 
        public string get_captcha()
        {
            return captcha.Text;
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            win.main.Navigate(new RegPage());
        }
    }
}
