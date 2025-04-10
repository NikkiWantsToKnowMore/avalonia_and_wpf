using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using ExperimentAvaloniaApp.Properties;
using System.ComponentModel;

namespace ExperimentAvaloniaApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadApplicationSettings();

        // �������� ��������
        this.Width = AppSettings.Default.WindowWidth;
        this.Height = AppSettings.Default.WindowHeight;

        // ���������� ����
        ApplyTheme(AppSettings.Default.Theme);

        // ���������� ��� ��������
        this.Closed += (s, e) =>
        {
            AppSettings.Default.WindowWidth = this.Width;
            AppSettings.Default.Save();
        };
    }
    private void LoadApplicationSettings()
    {
        // �������������� ������� ����
        if (AppSettings.Default.WindowWidth > 0)
        {
            this.Width = AppSettings.Default.WindowWidth;
        }

        // ���������� ����
        ApplyTheme(AppSettings.Default.Theme);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        // ���������� �������� ��� ��������
        AppSettings.Default.WindowWidth = this.Width;
        // ��������� ��������� ����� ��������� ����
        SaveSettings();
        base.OnClosing(e);
    }
    private void SaveSettings()
    {
        AppSettings.Default.WindowWidth = this.Width;
        AppSettings.Default.WindowHeight = this.Height;

        // ���� ��������� ������� ����
        AppSettings.Default.Save();
    }
    private void ApplyTheme(string theme)
    {
        // ������ ����� ����
        var darkBrush = new SolidColorBrush(Color.FromRgb(30, 30, 30));
        var lightBrush = new SolidColorBrush(Colors.White);

        this.Background = theme == "Dark" ? darkBrush : lightBrush;
        AppSettings.Default.Theme = theme;
    }
    private void ChangeThemeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var currentTheme = "";
        if (AppSettings.Default.Theme == "light")
            currentTheme = "Dark";
        else
            currentTheme = "light";
        ApplyTheme(currentTheme);
    }
}