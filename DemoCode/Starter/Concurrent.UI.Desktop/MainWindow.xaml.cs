using System.Windows;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI;

public partial class MainWindow : Window
{
    PersonReader reader = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    // Using Task
    private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
    {
        ClearListBox();

    }

    // Using Await
    private void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
    {
        ClearListBox();

    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ClearListBox()
    {
        PersonListBox.Items.Clear();
    }
}
