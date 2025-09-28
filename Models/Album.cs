using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album_music_toma.Models
{
    public class Album
    {
        public string Title { get; set; } = "";
        public int Year { get; set; }
        public string CoverUrl { get; set; } = "";
        public List<Song> Songs { get; set; } = new();
    }
}
