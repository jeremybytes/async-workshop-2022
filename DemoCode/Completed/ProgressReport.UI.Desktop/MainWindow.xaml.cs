using System.Windows;

namespace ProgressReport.UI;

public partial class MainWindow : Window
{
    PeopleViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = new PeopleViewModel();
        this.DataContext = viewModel;

        viewModel.NewUserMessage += (_, e) =>
            MessageBox.Show($"{e.Title}\n\n{e.Message}");
    }

    private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.FetchPeopleWithTask();
    }


    private async void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
    {
        await viewModel.FetchPeopleWithAwait();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Cancel();
    }

}
