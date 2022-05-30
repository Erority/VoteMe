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
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class SurveryPage : Page, INotifyPropertyChanged
    {

        private List<Survery> surveries = new List<Survery>();
        public SurveryPage()
        {
            InitializeComponent();

            DataContext = this;

            StartVisibility = Visibility.Visible;
            UIToQuestionVisibility = Visibility.Hidden;

            using (VoteMeDBEntities db = new VoteMeDBEntities())
            {
                surveries = db.Survery.ToList();
            }

        }

        private object currentSurvery;

        public object CurrentSurvery
        {
            get { return currentSurvery; }
            set 
            {
                currentSurvery = value;
                
                Survery vote = (Survery)value;  
                
                TBTitle.Text = vote.Title.ToString();
                tbAnswer1.Text = vote.FirstAnswer;
                tbAnswer2.Text = vote.SecondAnswer;
                tbAnswer3.Text = vote.ThirdAnswer;
                tbAnswer4.Text = vote.FourthAnswer;


                OnPropertyChanged();
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

        private string answer;

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }


        private void btAnswer4_Click(object sender, RoutedEventArgs e)
        {
            Answer = tbAnswer4.Text;
            SaveAnswerToDB();
            NextQuestion();
        }

        private void btAnswer3_Click(object sender, RoutedEventArgs e)
        {
            Answer = tbAnswer3.Text;
            SaveAnswerToDB();
            NextQuestion(); 
        }

        private void btAnswer2_Click(object sender, RoutedEventArgs e)
        {

            Answer = tbAnswer2.Text;
            SaveAnswerToDB();
            NextQuestion();
        }

        private void btAnswer1_Click(object sender, RoutedEventArgs e)
        {
            Answer = tbAnswer1.Text;
            SaveAnswerToDB();
            NextQuestion();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            UIToQuestionVisibility = Visibility.Visible;
            StartVisibility = Visibility.Hidden;

            NextQuestion();
        }

        int currentPos = 0;
        private void NextQuestion()
        {
            if (currentPos < surveries.Count)
            {
                CurrentSurvery = surveries[currentPos];
               
                currentPos += 1;
            } 
            else
            {
                MessageBox.Show("Вы прошли все голосования, спасибо !", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DataSaver.MainFrame.Content = null;
            }
        }

        private void SaveAnswerToDB()
        {
            using (VoteMeDBEntities db = new VoteMeDBEntities())
            {
                SurveryResult surveryResult = new SurveryResult();
                surveryResult.Answer = Answer;
                surveryResult.UserID = DataSaver.CurrentUserID;
                surveryResult.SurveryID = ((Survery)CurrentSurvery).ID;

                db.SurveryResult.Add(surveryResult);
                db.SaveChanges();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
