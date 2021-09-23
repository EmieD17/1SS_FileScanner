using System;
using System.Collections.Generic;
using System.Text;

namespace FileScanner.Models
{
    class FolderItem
    {
        public string TheItemDir { get; set; }
        public string TheImage { get; set; }
        public FolderItem(string theItemDir, string theimage)
        {
            TheItemDir = theItemDir;
            TheImage = theimage;
        }
    }
}
