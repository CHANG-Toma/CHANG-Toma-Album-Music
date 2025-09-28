using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Album_music_toma.Models;
using Microsoft.Maui.Controls;

namespace Album_music_toma.ViewModels
{
    public class SongsViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private Album _album = new();
        public Album Album 
        { 
            get => _album; 
            private set 
            { 
                if (Set(ref _album, value))
                {
                    Songs.Clear();
                    if (value?.Songs != null)
                    {
                        foreach (var song in value.Songs) 
                            Songs.Add(song);
                    }
                }
            } 
        }
        
        private string _artistName = "";
        public string ArtistName 
        { 
            get => _artistName; 
            private set => Set(ref _artistName, value); 
        }
        
        private string _currentSongTitle = "";
        public string CurrentSongTitle 
        { 
            get => _currentSongTitle; 
            private set => Set(ref _currentSongTitle, value); 
        }
        
        public ObservableCollection<Song> Songs { get; } = new();
        public ICommand PlaySongCommand { get; }

        // Constructeur
        public SongsViewModel()
        {
        PlaySongCommand = new Command<string>(async url =>
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            
            try
            {
                // Trouver le titre de la chanson
                var song = Songs.FirstOrDefault(s => s.YoutubeUrl == url);
                if (song != null)
                {
                    CurrentSongTitle = $"🎵 {song.Title} - Ouvert dans YouTube";
                }

                // Ouvrir directement dans le navigateur
                await Launcher.OpenAsync(url);
            }
            catch (Exception ex)
            {
                CurrentSongTitle = "❌ Erreur d'ouverture";
                System.Diagnostics.Debug.WriteLine($"Erreur ouverture YouTube: {ex.Message}");
            }
        });

        }

        // Reçoit l'objet depuis Shell
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue("Album", out var albumObj) && albumObj is Album album)
                {
                    Album = album;
                    System.Diagnostics.Debug.WriteLine($"Album reçu: {album.Title}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Aucun album reçu - utilisation d'un album par défaut");
                    // Créer un album par défaut pour éviter les erreurs
                    Album = new Album
                    {
                        Title = "Sélection d'albums",
                        Year = 2024,
                        CoverUrl = "dotnet_bot.png",
                        Songs = new List<Song>
                        {
                            new Song { Title = "Chanson 1", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" },
                            new Song { Title = "Chanson 2", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" },
                            new Song { Title = "Chanson 3", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" }
                        }
                    };
                }
                
                if (query.TryGetValue("ArtistName", out var artistObj) && artistObj is string artistName)
                {
                    ArtistName = artistName;
                    System.Diagnostics.Debug.WriteLine($"Nom artiste reçu: {artistName}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Aucun nom d'artiste reçu - utilisation d'un nom par défaut");
                    ArtistName = "Artiste";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur ApplyQueryAttributes: {ex.Message}");
                // En cas d'erreur, créer des données par défaut
                Album = new Album
                {
                    Title = "Album par défaut",
                    Year = 2024,
                    CoverUrl = "dotnet_bot.png",
                    Songs = new List<Song>
                    {
                        new Song { Title = "Chanson par défaut", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" }
                    }
                };
                ArtistName = "Artiste par défaut";
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
