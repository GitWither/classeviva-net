using System;

namespace ClassevivaNet
{
    public class MaterialFile
    {
        /// <summary>
        /// Author of the file
        /// </summary>
        public string Author { get; }
        /// <summary>
        /// Description of the file
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Link to the file
        /// </summary>
        public string Link { get; }
        /// <summary>
        /// The name of the folder containing the file
        /// </summary>
        public string RootFolder { get; }
        /// <summary>
        /// The date on which this file was created
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// Type of the file (File, Link)
        /// </summary>
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
