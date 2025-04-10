using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFForExperiment.Properties;

namespace WPFForExperiment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            LoadApplicationSettings();

            // Загрузка настроек
            this.Width = AppSettings.Default.WindowWidth;
            this.Height = AppSettings.Default.WindowHeight;

            // Применение темы
            ApplyTheme(AppSettings.Default.Theme);

            // Сохранение при закрытии
            this.Closed += (s, e) =>
            {
                AppSettings.Default.WindowWidth = this.Width;
                AppSettings.Default.Save();
            };
        }
        private void LoadApplicationSettings()
        {
            // Восстановление размера окна
            if (AppSettings.Default.WindowWidth > 0)
            {
                this.Width = AppSettings.Default.WindowWidth;
            }

            // Применение темы
            ApplyTheme(AppSettings.Default.Theme);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Сохранение настроек при закрытии
            AppSettings.Default.WindowWidth = this.ActualWidth;
            // Сохраняем настройки ПЕРЕД закрытием окна
            SaveSettings();
            base.OnClosing(e);
        }
        private void SaveSettings()
        {
            AppSettings.Default.WindowWidth = this.ActualWidth;
            AppSettings.Default.WindowHeight = this.ActualHeight;

            // Явно сохраняем текущую тему
            AppSettings.Default.Save();
        }
        private void ApplyTheme(string theme)
        {
            // Пример смены темы
            var darkBrush = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            var lightBrush = new SolidColorBrush(Colors.White);

            this.Background = theme == "Dark" ? darkBrush : lightBrush;
            AppSettings.Default.Theme = theme;
        }

        // Пример обработчика кнопки для смены темы
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            var currentTheme = "";
            if (AppSettings.Default.Theme == "light")
                currentTheme = "Dark";
            else
                currentTheme = "light";
            ApplyTheme(currentTheme);
        }
    }
}