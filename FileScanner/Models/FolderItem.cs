using System;
using System.Collections.Generic;
using System.Text;

namespace FileScanner.Models
{
    public class FolderItem
    {
        public string TheItemDir { get; set; }
        public string TheImage { get; set; }
        public FolderItem(string theItemDir, string theImage)
        {
            TheItemDir = theItemDir;
            TheImage = theImage;
        }
    }
}
