using word_game.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using word_game;
namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AccelerationWindow : Window
    {
        ColodServices colodServices = new ColodServices();

        DispatcherTimer timer;
        word_game.Services.CardServices cardServices = new word_game.Services.CardServices();
        List<Words> listWords = new List<Words>();
        string[] arrayRandomTranslate = new string[5];
        Random random = new Random();
        int next = 0;

        public AccelerationWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) }; // 1 секунда
            timer.Tick += Timer_Tick;
            timer.Start();

            List<Colod> allColods = colodServices.GetColod();
            Colod currentColod = new Colod();
            currentColod = allColods
                .Where(x => x.IsChecked == true)
                .FirstOrDefault();

            listWords = cardServices.GetCards(currentColod.Id);
            SetMassive(next);

            TranslateBlock.Text = arrayRandomTranslate[random.Next(arrayRandomTranslate.Length-1)];
            WordBlock.Text = listWords[next].Word;
        }
        private void Timer_Tick(object sender, object e)
        {
            progressBar.Value++;
            if (progressBar.Value == 5)
                timer.Stop();
        }

        private void progressBar_ValueChanged(object sender, EventArgs e)
        {
            if (next < listWords.Count - 1)
            {
                if (progressBar.Value >= 5)
                {
                    //progressBar.Value++;
                    next++;
                    WordBlock.Text = listWords[next].Word;
                    SetMassive(next);
                    TranslateBlock.Text = arrayRandomTranslate[random.Next(arrayRandomTranslate.Length - 1)];
                    progressBar.Value = 0;
                    timer.Start();
                }
            }
            else this.Close();
        }
        public void SetMassive(int index) {
            //заполнение массива arrayRandomTranslate
            arrayRandomTranslate[0] = listWords[index].Translate;

            for (int i = 1; i < arrayRandomTranslate.Length; i++) {

                arrayRandomTranslate[i] = listWords[random.Next(listWords.Count - 1)].Translate;

            }
            Shuffle(arrayRandomTranslate);
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
        public void CorrectClick(object sender, RoutedEventArgs args)
        {
            if (next < listWords.Count - 2)
            {

                if (TranslateBlock.Text == listWords[next].Translate)
                {
                   
                }
                else
                {
                    MessageBox.Show("incorrect");
                }
                progressBar.Value = 0;
                next++;
                WordBlock.Text = listWords[next].Word;
                SetMassive(next);
                TranslateBlock.Text = arrayRandomTranslate[random.Next(arrayRandomTranslate.Length - 1)];
            }
            else this.Close();

        }
        public void InCorrectClick(object sender, RoutedEventArgs args)
        {
            if (next < listWords.Count - 2)
            {
                if (TranslateBlock.Text != listWords[next].Translate)
                {

                }
                else
                {
                    MessageBox.Show("incorrect");

                }

                progressBar.Value = 0;
                next++;
                WordBlock.Text = listWords[next].Word;
                SetMassive(next);
                TranslateBlock.Text = arrayRandomTranslate[random.Next(arrayRandomTranslate.Length - 1)];
            }
            else
            {
                this.Close();
                MessageBox.Show("Тренировка закончена");
            }

        }
    }
}
