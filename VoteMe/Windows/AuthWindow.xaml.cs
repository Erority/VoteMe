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
using System.Windows.Shapes;
using VoteMe.EF;
using VoteMe.Utils;

namespace VoteMe.Windows
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            using(VoteMeDBEntities db = new VoteMeDBEntities())
            {
                var user = db.User.Where(u => u.Password == textBoxPassword.Text.Trim() && u.Login == textBoxLogin.Text.Trim()).FirstOrDefault();

                if (user != null)
                {
                    this.Hide();
                    DataSaver.CurrentUserID = user.ID;
                    new MainWindow().ShowDialog();
                    this.Show();
                }
            }
        }
    }
}
