using System.Windows;
using word_game.Services;

namespace word_game
{
    /// <summary>
    /// Логика взаимодействия для menu.xaml
    /// </summary>
    public class Colod
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsChecked { get; set; }
            public int Progress { get; set; }
            public int WordCount { get; set; }
        }

    public partial class Menu : Window
        {
            ColodServices colodServices = new ColodServices();
            Colod request = new Colod();

            public Menu()
                {
                    InitializeComponent();

                    List<Colod> requestColods = colodServices.GetColod();

                    Colods.ItemsSource = requestColods;
                    this.DataContext = this;

                }

            private void AddNewColodBtn(object sender, RoutedEventArgs e){

                AddNewColod addNewColod = new AddNewColod();
                addNewColod.Owner = this;

                addNewColod.Closed += (object s, EventArgs args) =>
                {

                    List<Colod> requestColods = colodServices.GetColod();
                    Colods.ItemsSource = requestColods;
                };

                addNewColod.Show();
            }

            public void DeleteColodBtn(object sender, RoutedEventArgs e)
            {

                if (Colods.SelectedIndex >= 0)
                {

                    Colod deleteColod = new Colod();
                    deleteColod = Colods.SelectedItem as Colod;
                    colodServices.DeleteColod(deleteColod.Id);

                    colodServices = new ColodServices();
                    List<Colod> requestColods = colodServices.GetColod();
                    Colods.ItemsSource = requestColods;

                }
            }

            public void OnTraningBtn(object sender, RoutedEventArgs e)
            {
                if (Colods.SelectedIndex >= 0){

                    Colod currrentColod = new Colod();
                    currrentColod = Colods.SelectedItem as Colod;
                    colodServices.ChandeStatus(currrentColod.Id, true);
                }

                MainWindow mainWindow = new MainWindow();
               
                mainWindow.Closed += (object s, EventArgs args) =>
                {
                    Colod changeStatusColod = new Colod();
                    List<Colod> requestColods = colodServices.GetColod();

                    changeStatusColod = requestColods
                        .Where(x => x.IsChecked == true)
                        .FirstOrDefault();
                    colodServices.ChandeStatus(changeStatusColod.Id, false);
                    Colods.ItemsSource = requestColods;
                    

                };

                mainWindow.Show();
                this.Close();
                

            }
    }

}
