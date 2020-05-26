﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    class MaterialFile
    {
        public string Author { get; }
        public string Description { get; }
        public string Link { get; }
        public DateTime Date { get; }

        public MaterialFile(string author, string description, string link, DateTime date)
        {
            Author = author;
            Description = description;
            Link = link;
            Date = date;
        }
    }
}
