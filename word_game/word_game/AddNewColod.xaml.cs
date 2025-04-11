using System.Windows;
using word_game.Services;

namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class AddNewColod : Window
    {
        ColodServices colodServices = new ColodServices();

        public AddNewColod()
        {
            InitializeComponent();
        }

        private void AddColodBtn(object sender, RoutedEventArgs e)
        {
            if (colodName.Text != "" || colodName.Text != "Название" || colodName.Text != null)
            {

                colodServices.AddColod(colodName.Text, false, 0);
                if (MessageBox.Show("Колода успешно создана", "уведомление о добавлении колоды", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    colodName.Clear();

                };
            }
            else MessageBox.Show("Поле 'Название' не может быть пустым");

        }

        private void CanselBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearBoxWord(object sender, RoutedEventArgs e)
        {
            colodName.Clear();
        }
        
    }
}
