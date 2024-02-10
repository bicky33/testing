using NorthwindWebMvc.Basic.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindWebMvc.Basic.Models.Dto
{
    public class ProductDtoCreate
    {
        public int Id { get; set; }


        [Required]
        public string ProductName { get; set; }

        [Range(EntityConstantModel.MIN_PRICE, EntityConstantModel.MAX_PRICE)]
        public decimal Price { get; set; }
        public int Stock { get; set; }

        [Required(ErrorMessage = "Please select image")]
        public IFormFile Photo { get; set; }
        public int CategoryId { get; set; }
    }
}
