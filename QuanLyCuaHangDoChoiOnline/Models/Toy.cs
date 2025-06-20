
using System.ComponentModel.DataAnnotations;


namespace QuanLyCuaHangDoChoiOnline.Models
{
    public class Toy
    {
        public string ToyID { get; set; }
        [Required]
        public string ToyName { get; set; }
        [Required]
        public string ToyTypeID { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int OrderedQuantity { get; set; }
    }
}
