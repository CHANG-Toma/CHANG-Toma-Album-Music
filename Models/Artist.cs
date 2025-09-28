using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album_music_toma.Models
{
    public class Artist
    {
        public string Name { get; set; } = "";
        public string PhotoUrl { get; set; } = ""; // URL ou ressource locale (Resources/Images)
        public string Genre { get; set; } = "";
        public string Bio { get; set; } = ""; // Biographie de l'artiste
        public List<Album> Albums { get; set; } = new();
    }
}
