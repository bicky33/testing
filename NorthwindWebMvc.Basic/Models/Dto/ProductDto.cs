using NorthwindWebMvc.Basic.Models.Base;
using NorthwindWebMvc.Basic.Models.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindWebMvc.Basic.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(EntityConstantModel.MIN_PRICE, EntityConstantModel.MAX_PRICE)]
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string? Photo { get; set; }

        [Column("CategoryId")]
        public int CategoryId { get; set; }

        public CategoryDto Category { get; set; }
    }
}
