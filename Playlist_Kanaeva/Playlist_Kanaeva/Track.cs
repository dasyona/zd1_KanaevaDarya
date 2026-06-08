using System;

namespace Playlist_Kanaeva
{
    internal struct Track
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }

        public Track(string author, string title, string filename)
        {
            Author = author;
            Title = title;
            Filename = filename;
        }

        public override string ToString()
        {
            return $"{Author} - {Title}";
        }
    }
}