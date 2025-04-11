using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using word_game.Services;
namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для AddWord.xaml
    /// </summary>
   
    public partial class AddWord : Window
    {
        CardServices cardServices = new CardServices();
        ColodServices colodServices = new ColodServices();
        ObservableCollection<Words> requestCards = new ObservableCollection<Words>();
        List<Colod> colods = new List<Colod>();

        Words request = new Words();

        public bool Confirm { get; set; }

        public AddWord()
        {
            InitializeComponent();
            Confirm = false;

        }

        public ObservableCollection<Words> AddWordForRequest() {

            return requestCards;
        }

        public void AddWordInDictionary(object sender, RoutedEventArgs e) {

            Colod currentColod = new Colod();
            colods = colodServices.GetColod();

            currentColod = colods
                           .Where(x => x.IsChecked == true)
                           .FirstOrDefault();

            if (currentColod.IsChecked == true)
            {
                if (wordBox.Text != "" && translateBox.Text != "")
                {
                    if (wordBox.Text != "cлово" && translateBox.Text != "перевод")
                    {    //(слово, перевод, прогресс, к какой колоде относится слово, isChecked)
                        cardServices.AddCard(wordBox.Text, translateBox.Text, 0, currentColod.Id, false);
                        if (MessageBox.Show("Добавлено слово для изучения","в словарь", MessageBoxButton.OK) == MessageBoxResult.OK) {
                            wordBox.Clear();
                            translateBox.Clear();

                        };
                        colodServices.AddWordCount(currentColod.Id);
                    }
                    else btnCancel.IsEnabled = true;
                }
                else btnCancel.IsEnabled = true;
            }
            else MessageBox.Show("Колода не выбрана");

            
        }
        public class AddCardEventArgs : EventArgs
        {
            public bool AddInfo { get; private set; }

            public AddCardEventArgs(bool info)
            {
                this.AddInfo = info;
            }
        }

        void clearBoxWord(object sender, MouseButtonEventArgs e)
        {
            wordBox.Clear();
        }

        void clearBoxTranslate(object sender, MouseButtonEventArgs e)
        {
            translateBox.Clear();
        }
        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            Confirm = true;
            this.Close();

        }

    }
}
