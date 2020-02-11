using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikipediaSearchEngine.Models
{
    public class Document
    {
        public string Title { get; set; }  
        public string Content { get; set; }  
        public DateTime Timestamp { get; set; }  
    }
}
