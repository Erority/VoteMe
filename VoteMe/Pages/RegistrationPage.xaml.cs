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
using VoteMe.EF;
using VoteMe.Utils;

namespace VoteMe.Pages
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(tbFName.Text.Trim()))
            {
                MessageBox.Show("Заполните имя", "Предпреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(tbLName.Text.Trim()))
            {
                MessageBox.Show("Заполните фамилию", "Предпреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(tbLogin.Text.Trim()))
            {
                MessageBox.Show("Заполните логин", "Предпреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(tbPassword.Text.Trim()))
            {
                MessageBox.Show("Заполните пароль", "Предпреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (tbPassword.Text.Trim().Length < 8)
            {
                MessageBox.Show("Пароль слишком слабый", "Предпреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            using(VoteMeDBEntities db = new VoteMeDBEntities())
            {
                foreach (var item in db.User.ToList())
                {
                    if (tbLogin.Text.Trim() == item.Login)
                    {
                        MessageBox.Show("Пользователь с таким логином уже есть", "Предпреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

            }

            return true;


        }

        private void buttonReg_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation())
                return;

            User user = new User()
            {
                FirstName = tbFName.Text.Trim(),
                LastName = tbLName.Text.Trim(),
                MiddleName = tbMName.Text.Trim().Length == 0 ? null : tbMName.Text.Trim(),
                isAdmin = toggleButton.IsChecked.Value,
                Login = tbLogin.Text.Trim(),
                Password = tbPassword.Text.Trim()
            };

            using (VoteMeDBEntities db = new VoteMeDBEntities())
            {
                db.User.Add(user);
                db.SaveChanges();


                MessageBox.Show("Пользователь успешно зарегестрирован", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                DataSaver.MainFrame.Content = null;
            }
        }
    }
}
