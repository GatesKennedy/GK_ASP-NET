using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS_Recovery_Console
{
    class Page
    {
        public enum PageType { objectives, lesson, drill, video, test}
        public String Title { get; set; }
        public List<string> Content { get; set; }

    }

}
