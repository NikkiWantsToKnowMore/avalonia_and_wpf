using System.Windows;
using word_game.Services;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public class Words
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Translate { get; set; }
        public bool IsChecked { get; set; }
        public int Progress { get; set; }
        public int Colods { get; set; }
    }


    public partial class MainWindow : Window
    {
        CardServices cardServices = new CardServices();
        ColodServices colodServices = new ColodServices();

        Words request = new Words();

        public MainWindow()
        {
            InitializeComponent();
            List<Colod> allColods = colodServices.GetColod();
            Colod currentColod = new Colod();
            currentColod = allColods
                .Where(x => x.IsChecked == true)
                .FirstOrDefault();
            List<Words> requestCards = cardServices.GetCards(currentColod.Id);
            AllWords.ItemsSource = requestCards;
            this.DataContext = this;
        }

        public void LearnNewWordsClick(object sender, RoutedEventArgs e)
        {
            LearnNewWords learnNewWords = new LearnNewWords();
            learnNewWords.Owner = this;
            learnNewWords.Show();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void AddWord(object sender, RoutedEventArgs e)
        {

            AddWord addWord = new AddWord();
            addWord.Owner = this;

            addWord.Closed += (object s, EventArgs args) =>
            {
                List<Colod> allColods = colodServices.GetColod();
                Colod currentColod = new Colod();
                currentColod = allColods
                    .Where(x => x.IsChecked == true)
                    .FirstOrDefault();
                List<Words> requestCards = cardServices.GetCards(currentColod.Id);
                AllWords.ItemsSource = requestCards;
            };

            addWord.Show();


        }

        private void AccselerationClick(object sender, RoutedEventArgs e)
        {
            AccelerationWindow accelerationWindow = new AccelerationWindow();
            accelerationWindow.Show();
        }

        public void DeleteWordClick(object sender, RoutedEventArgs e)
        {

            if (AllWords.SelectedIndex >= 0)
            {

                Words deleteWord = new Words();
                deleteWord = AllWords.SelectedItem as Words;
                cardServices.DeleteCard(deleteWord.Id);

                List<Colod> allColods = colodServices.GetColod();
                Colod currentColod = new Colod();
                currentColod = allColods
                    .Where(x => x.IsChecked == true)
                    .FirstOrDefault();

                cardServices = new CardServices();
                List<Words> requestCards = cardServices.GetCards(currentColod.Id);
                AllWords.ItemsSource = requestCards;
                colodServices.DeleteWordCount(currentColod.Id);

            }
        }
        public void TranslateWordClick(object sender, RoutedEventArgs e)
        {

            TranslateWord translateWord = new TranslateWord();
            translateWord.Show();
        }
        public void WordTranslateClick(object sender, RoutedEventArgs e)
        {

            WordTranslate wordTranslate = new WordTranslate();
            wordTranslate.Show();
        }
        public void ConformityClick(object sender, RoutedEventArgs e)
        {
            List<Colod> allColods = colodServices.GetColod();
            Colod currentColod = new Colod();
            currentColod = allColods
                .Where(x => x.IsChecked == true)
                .FirstOrDefault();

            List<Words> requestCards = cardServices.GetCards(currentColod.Id);

            if (requestCards.Count > 4)
            {
                Сonformity сonformity = new Сonformity();
                сonformity.Show();
            }
            else
            {
                MessageBox.Show("Упражнение доступно, когда в словаре больше 4-х слов");
            }
        }
        public void ConformityTranslateClick(object sender, RoutedEventArgs e)
        {
            List<Colod> allColods = colodServices.GetColod();
            Colod currentColod = new Colod();
            currentColod = allColods
                .Where(x => x.IsChecked == true)
                .FirstOrDefault();

            List<Words> requestCards = cardServices.GetCards(currentColod.Id);

            if (requestCards.Count > 4)
            {
                ConformityTranslate conformityTranslate = new ConformityTranslate();
                conformityTranslate.Show();
            }
            else
            {
                MessageBox.Show("Упражнение доступно, когда в словаре больше 4-х слов");
            }
        }

        public void MenuClick(object sender, RoutedEventArgs e)
        {

            Menu menuProgram = new Menu();
            menuProgram.Show();
            this.Close();
        }


    }
}
