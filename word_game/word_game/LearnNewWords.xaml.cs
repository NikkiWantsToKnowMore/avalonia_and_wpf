using System.Collections.Generic;
using System.Linq;
using System.Windows;
using word_game.Services;

namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для LearnNewWords.xaml
    /// </summary>

    public partial class LearnNewWords : Window
    {
        CardServices cardServices = new CardServices();
        ColodServices colodServices = new ColodServices();
        List<Words> cardsCollection = new List<Words>();
        int index = 0;

        public LearnNewWords()
        {
            InitializeComponent();
            List<Words> prepareToCollection = new List<Words>();

            List<Colod> allColods = colodServices.GetColod();
            Colod currentColod = new Colod();
            currentColod = allColods
                .Where(x => x.IsChecked == true)
                .FirstOrDefault();

            prepareToCollection = cardServices.GetCards(currentColod.Id);
            
            foreach (var item in prepareToCollection) {

                Words addElem = new Words();
                addElem.Word = item.Word;
                addElem.Translate = item.Translate;
                addElem.Progress = item.Progress;
                cardsCollection.Add(addElem);
            }
            TextBlockWord.Text = cardsCollection[index].Word;
            TextBlockTranslate.Text = cardsCollection[index].Translate;
        }
        
        public void Click_NextCard(object sender, RoutedEventArgs args) {

            if ((index >= 0) && (index < (cardsCollection.Count - 1)))
            {
                index++;
                TextBlockWord.Text = cardsCollection[index].Word;
                TextBlockTranslate.Text = cardsCollection[index].Translate;

            }
            else {
               var messageBox =  MessageBox.Show("пора переходить к тренировкам)");
                if (messageBox == MessageBoxResult.OK) {
                    this.Close();
                    this.Close();
                }
            }
        }
        public void Click_PreviousCard(object sender, RoutedEventArgs args)
        {
            
            if ((index > 0) &&(index < cardsCollection.Count))
            {
                index--;
                TextBlockWord.Text = cardsCollection[index].Word;
                TextBlockTranslate.Text = cardsCollection[index].Translate;
               
            }
           
        }
    }
}
