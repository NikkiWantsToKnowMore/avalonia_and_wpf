using word_game.Services;
using System.Windows;

namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для Сonformity.xaml
    /// </summary>
    public partial class Сonformity : Window
    {
        Random random = new Random();
        CardServices cardServices = new CardServices();
        ColodServices colodServices = new ColodServices();
        List<Words> Cards = new List<Words>();
        int variant = 0;
        string[] variantsMassive = new string[4];
        int index = 0;

        public Сonformity()
        {
                InitializeComponent();

                List<Colod> allColods = colodServices.GetColod();
                Colod currentColod = new Colod();
                currentColod = allColods
                    .Where(x => x.IsChecked == true)
                    .FirstOrDefault();

            Cards = cardServices.GetCards(currentColod.Id); 

                TextBlockTranslate.Text = Cards[0].Translate;

                variant = random.Next(4);
                variantsMassive[variant] = Cards[variant].Word;

                for (int i = 0; i < variantsMassive.Length; i++)
                {

                    if (i != variant)
                    {

                        variantsMassive[i] = Cards[i].Word;
                    }
                }

                Shuffle(variantsMassive);

                BtnVar1.Content = variantsMassive[0];
                BtnVar2.Content = variantsMassive[1];
                BtnVar3.Content = variantsMassive[2];
                BtnVar4.Content = variantsMassive[3];
        }
        public static void Shuffle(string[] array)
        {
            // перетасовка массива
            Random random = new Random();

            for (int i = array.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                string tmp = array[j];
                array[j] = array[i];
                array[i] = tmp;
            }
        }
        public void FillingMassive(string[] array) {

            TextBlockTranslate.Text = Cards[index].Translate;

            variant = random.Next(4);
            array[variant] = Cards[index].Word;
            
            for (int i = 0; i < array.Length; i++)
            {

                if (i != variant)
                {
                    array[i] = Cards[random.Next(Cards.Count-1)].Word;
                    
                }
            }

            Shuffle(array);

            BtnVar1.Content = array[0];
            BtnVar2.Content = array[1];
            BtnVar3.Content = array[2];
            BtnVar4.Content = array[3];
        }
        public void BtnVar1_Click(object sender, RoutedEventArgs e)
        {
            if (index + 1 >= Cards.Count) {
                this.Close();
                MessageBox.Show("Тренировка закончена");
            }
            if (BtnVar1.Content == Cards[index].Word)
            {
                index++;
                if (index < Cards.Count) {

                    TextBlockTranslate.Text = Cards[index].Translate;
                    FillingMassive(variantsMassive);
                }

            }
        }
        public void BtnVar2_Click(object sender, RoutedEventArgs e)
        {
            if (index + 1 >= Cards.Count)
            {
                this.Close();
                MessageBox.Show("Тренировка закончена");
            }
            if (BtnVar2.Content == Cards[index].Word)
            {
                index++;
                if (index < Cards.Count)
                {

                    TextBlockTranslate.Text = Cards[index].Translate;
                    FillingMassive(variantsMassive);
                }

            }
        }
        public void BtnVar3_Click(object sender, RoutedEventArgs e)
        {
            if (index + 1 >= Cards.Count)
            {
                this.Close();
                MessageBox.Show("Тренировка закончена");
            }
            if (BtnVar3.Content == Cards[index].Word)
            {
                index++;
                if (index < Cards.Count)
                {

                    TextBlockTranslate.Text = Cards[index].Translate;
                    FillingMassive(variantsMassive);
                }

            }
        }
        public void BtnVar4_Click(object sender, RoutedEventArgs e)
        {
            if (index + 1 >= Cards.Count)
            {
                this.Close();
                MessageBox.Show("Тренировка закончена");
            }
            if (BtnVar4.Content == Cards[index].Word)
            {
                index++;
                if (index < Cards.Count)
                {
                    TextBlockTranslate.Text = Cards[index].Translate;
                    FillingMassive(variantsMassive);
                }

            }
        }

    }
}
