using System.Threading.Channels;
using System.Windows;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ParallelUI;

public partial class MainWindow : Window
{
    PersonReader reader = new();
    CancellationTokenSource? tokenSource;

    public MainWindow()
    {
        InitializeComponent();
    }

    // OPTION 1: Await (runs sequentially)
    private async void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
    {
        tokenSource = new CancellationTokenSource();
        FetchWithAwaitButton.IsEnabled = false;
        try
        {
            ClearListBox();

            List<int> ids = await reader.GetIdsAsync();

            foreach (int id in ids)
            {
                // The next iteration of the loop will not
                // run until this one is complete
                var person = await reader.GetPersonAsync(id, tokenSource.Token);
                PersonListBox.Items.Add(person);
            }
        }
        catch (OperationCanceledException ex)
        {
            MessageBox.Show($"CANCELED\n{ex.GetType()}\n{ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ERROR\n{ex.GetType()}\n{ex.Message}");
        }
        finally
        {
            tokenSource.Dispose();
            FetchWithAwaitButton.IsEnabled = true;
        }
    }

    // OPTION 2: Task w/ Continuation (runs parallel)
    private async void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
    {
        using (tokenSource = new CancellationTokenSource())
        {
            FetchWithTaskButton.IsEnabled = false;
            ClearListBox();

            var taskList = new List<Task>();

            try
            {
                var ids = await reader.GetIdsAsync();

                foreach (int id in ids)
                {
                    Task<Person> personTask = reader.GetPersonAsync(id, tokenSource.Token);
                    Task continuation = personTask.ContinueWith(task =>
                    {
                        Person person = task.Result;
                        PersonListBox.Items.Add(person);
                    },
                    tokenSource.Token,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());

                    taskList.Add(continuation);
                }

                await Task.WhenAll(taskList);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"CANCELED\n{ex.GetType()}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR\n{ex.GetType()}\n{ex.Message}");
            }
            finally
            {
                FetchWithTaskButton.IsEnabled = true;
            }
        }
    }

    // OPTION 3: Task w/ Channel (runs parallel)
    private async void FetchWithChannelButton_Click(object sender, RoutedEventArgs e)
    {
        using (tokenSource = new CancellationTokenSource())
        {
            FetchWithChannelButton.IsEnabled = false;
            ClearListBox();

            try
            {
                var ids = await reader.GetIdsAsync();

                var channel = Channel.CreateUnbounded<Person>();
                Task consumer = ShowData(channel.Reader);
                Task producer = ProduceData(ids, channel.Writer, tokenSource.Token);
                await producer;
                await consumer;
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"CANCELED\n{ex.GetType()}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR\n{ex.GetType()}\n{ex.Message}");
            }
            finally
            {
                FetchWithChannelButton.IsEnabled = true;
            }
        }
    }

    private async Task ShowData(ChannelReader<Person> channelReader)
    {
        await foreach (var person in channelReader.ReadAllAsync())
        {
            PersonListBox.Items.Add(person);
        }
    }

    private async Task ProduceData(List<int> ids, ChannelWriter<Person> channelWriter,
        CancellationToken cancelToken = new())
    {
        var taskList = new List<Task>();
        foreach (var id in ids)
        {
            taskList.Add(FetchPerson(id, channelWriter, cancelToken));
        }
        await Task.WhenAll(taskList);
        channelWriter.Complete();
    }

    private async Task FetchPerson(int id, ChannelWriter<Person> channelWriter,
        CancellationToken cancelToken)
    {
        var person = await reader.GetPersonAsync(id, cancelToken);
        await channelWriter.WriteAsync(person);
    }

    // OPTION 4: Parallel.ForEachAsync (runs parallel)
    // Note: This code needs some additional work to get it
    // to work here. We need to marshall back to the UI thread.
    // There is no automatic way to do this with Parallel.ForEachAsync,
    // so we would need to introduce a Dispatcher or something like that.
    private async void FetchWithForEachAsyncButton_Click(object sender, RoutedEventArgs e)
    {
        using (tokenSource = new CancellationTokenSource())
        {
            FetchWithForEachAsyncButton.IsEnabled = false;
            ClearListBox();

            try
            {
                var ids = await reader.GetIdsAsync();

                await Parallel.ForEachAsync(
                    ids,
                    tokenSource.Token,
                    async (id, token) =>
                    {
                        var person = await reader.GetPersonAsync(id, token);
                        PersonListBox.Items.Add(person);
                    });
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"CANCELED\n{ex.GetType()}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR\n{ex.GetType()}\n{ex.Message}");
            }
            finally
            {
                FetchWithForEachAsyncButton.IsEnabled = true;
            }
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            tokenSource?.Cancel();
        }
        catch (Exception)
        {
            // if there's something wrong with the 
            // cancellation token source, just ignore it
            // (it may be disposed because nothing is running)
        }
    }

    private void ClearListBox()
    {
        PersonListBox.Items.Clear();
    }
}
