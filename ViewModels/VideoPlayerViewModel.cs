using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Album_music_toma.ViewModels
{
    public class VideoPlayerViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private string _videoUrl = "";
        public string VideoUrl 
        { 
            get => _videoUrl; 
            private set => Set(ref _videoUrl, value); 
        }

        // Constructeur
        public VideoPlayerViewModel()
        {
        }

        // Reçoit l'URL depuis Shell
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("url", out var urlObj) && urlObj is string url)
            {
                // Convertir l'URL YouTube en URL d'embed pour lecture directe
                var embedUrl = ConvertToEmbedUrl(url);
                VideoUrl = embedUrl;
            }
        }

        private string ConvertToEmbedUrl(string youtubeUrl)
        {
            try
            {
                // Extraire l'ID de la vidéo YouTube
                var videoId = ExtractVideoId(youtubeUrl);
                if (!string.IsNullOrEmpty(videoId))
                {
                    // Créer l'URL d'embed pour lecture directe
                    return $"https://www.youtube.com/embed/{videoId}?autoplay=1&rel=0&modestbranding=1";
                }
            }
            catch
            {
                // En cas d'erreur, retourner l'URL originale
            }
            
            return youtubeUrl;
        }

        private string ExtractVideoId(string youtubeUrl)
        {
            try
            {
                var uri = new Uri(youtubeUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                
                // Essayer de récupérer l'ID depuis le paramètre 'v'
                var videoId = query["v"];
                if (!string.IsNullOrEmpty(videoId))
                {
                    return videoId;
                }
                
                // Si c'est un format court (youtu.be/ID)
                if (uri.Host.Contains("youtu.be"))
                {
                    return uri.Segments.Last().Trim('/');
                }
            }
            catch
            {
                // En cas d'erreur, essayer d'extraire manuellement
                var match = System.Text.RegularExpressions.Regex.Match(youtubeUrl, @"(?:youtube\.com\/watch\?v=|youtu\.be\/)([^&\n?#]+)");
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            
            return "";
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
