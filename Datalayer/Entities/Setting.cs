using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Setting
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Key { get; set; }

        [MaxLength(5000)]
        public string? Value { get; set; }
    }
}
