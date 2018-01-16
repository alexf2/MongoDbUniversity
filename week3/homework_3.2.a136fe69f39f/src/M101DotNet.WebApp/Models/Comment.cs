using System;

namespace M101DotNet.WebApp.Models
{
    public class Comment
    {        
        public string Author { get; set; }

        public string Content { get; set; }        

        public DateTime CreatedAtUtc { get; set; }
    }
}