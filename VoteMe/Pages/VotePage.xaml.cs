using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for SurveryPage.xaml
    /// </summary>
    public partial class VotePage : Page, INotifyPropertyChanged
    {
        public VotePage()
        {
            InitializeComponent();

            DataContext = this;

            StartVisibility = Visibility.Visible;
            UIToQuestionVisibility = Visibility.Hidden;


            using(VoteMeDBEntities db = new VoteMeDBEntities())
            {
                votes = db.Vote.ToList();
            }

        }


        private List<Vote> votes = new List<Vote>();

        private Vote currentVote;
        public Vote CurrentVote
        {
            get { return currentVote; }
            set 
            { 
                currentVote = value;
                OnPropertyChanged();

                TBTitle.Text = currentVote.Title;
            }
        }


        private Visibility startVisibility;

        public Visibility StartVisibility
        {
            get { return startVisibility; }
            set
            {
                startVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility uiToQuestionVisibility;

        public Visibility UIToQuestionVisibility
        {
            get { return uiToQuestionVisibility; }
            set
            {
                uiToQuestionVisibility = value;
                OnPropertyChanged();
            }
        }

        private string answer = "";

        private void SaveAnswer()
        {
            using (VoteMeDBEntities db = new VoteMeDBEntities())
            {
                VoteResult voteResult = new VoteResult();
                voteResult.Answer = answer;
                voteResult.UserID = DataSaver.CurrentUserID;
                voteResult.VoteID = CurrentVote.ID;
                db.VoteResult.Add(voteResult);
                db.SaveChanges();
            }
        }

        private int currentPos = 0;
        private void GoNextAnswer()
        {
            if (currentPos < votes.Count)
            {
                CurrentVote = votes[currentPos];

                currentPos += 1;
            }
            else
            {
                MessageBox.Show("Вы прошли все опросы, спасибо !", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DataSaver.MainFrame.Content = null;
            }
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            answer = TextBoxAnswer.Text;

            SaveAnswer();
            GoNextAnswer();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            StartVisibility = Visibility.Hidden;
            UIToQuestionVisibility = Visibility.Visible;

            GoNextAnswer();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
