using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Entities.product
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<Photo> Photos { get; set; } = new List<Photo>();

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
