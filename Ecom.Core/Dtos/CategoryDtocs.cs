using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dtos
{
    public class CategoryDtocs
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
