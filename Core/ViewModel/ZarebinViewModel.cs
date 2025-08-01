using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class ZarebinViewModel
    {
        public class ZarebinProduct
        {
            public string title { get; set; }
            public string subtitle { get; set; }
            public int id { get; set; }
            public string current_price { get; set; }
            public string old_price { get; set; }
            public string availability { get; set; }
            public List<string> categories { get; set; }
            public string image_link { get; set; }
            public List<string> image_links { get; set; }
            public string page_url { get; set; }
            public string short_desc { get; set; }
            public Spec spec { get; set; }
            public string registry { get; set; }
            public string guarantee { get; set; }
        }

        public class Zarehbin
        {
            public string count { get; set; }
            public string total_pages_count { get; set; }
            public List<ZarebinProduct> products { get; set; }
        }

        public class Spec
        {
            public string memory { get; set; }
            public string camera { get; set; }
            public string color { get; set; }
        }
    }
}
