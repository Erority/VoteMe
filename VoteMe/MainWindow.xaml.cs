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
using VoteMe.Pages;
using VoteMe.EF;
using VoteMe.Utils;

namespace VoteMe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isAdmin = false;
        public MainWindow()
        {
            InitializeComponent();

            DataSaver.MainFrame = main;

            using (VoteMeDBEntities db = new VoteMeDBEntities())
            {
                isAdmin = (db.User.Where(u => u.ID == DataSaver.CurrentUserID).FirstOrDefault()).isAdmin;
            }

            if (isAdmin)
            {
                FirstButton.Text = "Результаты опросов";
                SecondButton.Text = "Результаты голосований";
                buttonReg.Visibility = Visibility.Visible;
            }
            else
            {
                FirstButton.Text = "Начать опрос";
                SecondButton.Text = "Начать голосование";
                buttonReg.Visibility = Visibility.Hidden;
            }
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
                DataSaver.MainFrame.Content = new ResultPage();
            else
            {
                using (VoteMeDBEntities db = new VoteMeDBEntities())
                {
                    User user = db.User.Where(u => u.ID == DataSaver.CurrentUserID).FirstOrDefault();

                    if (user.SurveryResult.ToList().Count == 0)
                    {
                        DataSaver.MainFrame.Content = new SurveryPage();
                        return;
                    }

                    bool haveUserNotCompleteVote = false;

                    foreach (var res in db.SurveryResult.Where(s => s.UserID == DataSaver.CurrentUserID).ToList())
                    {
                        foreach (var item in db.Survery.ToList())
                        {
                            if (item.ID != res.SurveryID)
                            {
                                haveUserNotCompleteVote = true;
                                break;
                            }
                        }
                    }

                    if (haveUserNotCompleteVote)
                    {
                        DataSaver.MainFrame.Content = new SurveryPage();
                    }
                    else
                    {
                        MessageBox.Show("Вы уже прошли все голосования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void btRez_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
                DataSaver.MainFrame.Content = new ResultSurveryPage();
            else
            {

                using (VoteMeDBEntities db = new VoteMeDBEntities())
                {
                    User user = db.User.Where(u => u.ID == DataSaver.CurrentUserID).FirstOrDefault();

                    if (user.VoteResult.Count == 0)
                    {
                        DataSaver.MainFrame.Content = new VotePage();
                        return;
                    }

                    bool haveUserNotCompleteVote = false;

                    foreach (var res in db.VoteResult.Where(s => s.UserID == DataSaver.CurrentUserID).ToList())
                    {
                        foreach (var item in db.Vote.ToList())
                        {
                            if (item.ID != res.VoteID)
                            {
                                haveUserNotCompleteVote = true;
                                break;
                            }
                        }
                    }

                    if (haveUserNotCompleteVote)
                    {
                        DataSaver.MainFrame.Content = new VotePage();
                    }
                    else
                    {
                        MessageBox.Show("Вы уже прошли все опросы.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void buttonReg_Click(object sender, RoutedEventArgs e)
        {
            DataSaver.MainFrame.Content = new RegistrationPage();
        }
    }
}