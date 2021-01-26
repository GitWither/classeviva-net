using ClassevivaNet.Objects;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Internal
{
    internal struct DidacticsResponse
    {
        [JsonProperty("didacticts")]
        public Teacher[] Didactics { get; set; }
    }

    internal struct Teacher
    {
        [JsonProperty("teacherName")]
        public string TeacherName { get; set; }
        [JsonProperty("folders")]
        public Folder[] Folders { get; set; }
    }

    internal struct Folder
    {
        [JsonProperty("folderName")]
        public string FolderName { get; set; }
        [JsonProperty("contents")]
        public Content[] Contents { get; set; }
    }
}
