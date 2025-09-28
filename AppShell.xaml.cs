namespace Album_music_toma;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Album_music_toma.Views.ArtistDetailPage),
                              typeof(Album_music_toma.Views.ArtistDetailPage));
        Routing.RegisterRoute("songs", typeof(Album_music_toma.Views.SongsPage));
        Routing.RegisterRoute("video", typeof(Album_music_toma.Views.VideoPlayerPage));
    }
}
