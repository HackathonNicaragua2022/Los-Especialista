namespace Hackathon2022;

public partial class MainPage : ContentPage
{
    private int Count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object Sender, EventArgs Args)
    {
        Count++;

        if (Count == 1)
            CounterBtn.Text = $"Clicked {Count} time";
        else
            CounterBtn.Text = $"Clicked {Count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

