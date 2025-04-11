using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using word_game.Services;

namespace word_game
{
    public partial class WordTranslate : Window
    {
        List<Words> Cards = new List<Words>();
        CardServices cardServices = new CardServices();
        ColodServices colodServices = new ColodServices();
        int index = 0;

        public WordTranslate()
        {
            List<Colod> allColods = colodServices.GetColod();
            Colod currentColod = new Colod();
            currentColod = allColods
                .Where(x => x.IsChecked == true)
                .FirstOrDefault();

            InitializeComponent();
            Cards = cardServices.GetCards(currentColod.Id);
            TextBlockTranslate.Text = Cards[index].Word;
        }
        void ClearBoxWord(object sender, MouseButtonEventArgs e)
        {
            TextBoxWord.Clear();
        }
        public void NextWordClick(object sender, RoutedEventArgs args)
        {

            if ((index >= 0) && (index < Cards.Count))
            {

                TextBlockTranslate.Text = Cards[index].Word;

                if (TextBoxWord.Text == Cards[index].Translate)
                {

                    index++;

                    if (index >= Cards.Count)
                    {
                        this.Close();
                        this.Close();
                        MessageBox.Show("Тренировка закончена");
                    }
                    else
                    {

                        TextBlockTranslate.Text = Cards[index].Word;
                    }

                }
                else
                {

                    MessageBox.Show("Неверно");
                }
            }
        }
        public void SkipWordClick(object sender, RoutedEventArgs args)
        {
            if ((index >= 0) && (index < (Cards.Count - 1)))
            {
                index++;
                TextBlockTranslate.Text = Cards[index].Word;

            }
            if (index >= (Cards.Count - 1))
            {
                this.Close();
                MessageBox.Show("Тренировка закончена");
            }
        }
    }
}
