using System.Windows;
using GameDashboard.ViewModels;


namespace GameDashboard;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}