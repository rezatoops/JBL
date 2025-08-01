using Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class CityViewModel
    {
        public List<City> Cities { get; set; }

        public int SelectedCity { get; set; }
    }
}
