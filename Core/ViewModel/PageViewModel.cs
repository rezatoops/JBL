using Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class PageViewModel
    {
        public List<Page> Pages { get; set; }

        public int CurrentPage { get; set; }

        public int CountPerPage { get; set; }

        public int AllPage { get; set; }

    }
}
