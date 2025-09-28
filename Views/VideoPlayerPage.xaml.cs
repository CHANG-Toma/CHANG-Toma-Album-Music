using Microsoft.Maui.Controls;

namespace Album_music_toma.Views;

public partial class VideoPlayerPage : ContentPage
{
    public VideoPlayerPage() 
    { 
        InitializeComponent(); 
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
