using System;

namespace ClassevivaNet
{
    public class MaterialFile
    {
        public string Author { get; }
        public string Description { get; }
        public string Link { get; }
        public string RootFolder { get; }
        public DateTime Date { get; }
        public MaterialFileType MaterialFileType { get; }

        public MaterialFile(string author, string description, string link, string rootFolder, MaterialFileType materialFileType, DateTime date)
        {
            Author = author;
            Description = description;
            Link = link;
            RootFolder = rootFolder;
            MaterialFileType = materialFileType;
            Date = date;
        }
    }
}
