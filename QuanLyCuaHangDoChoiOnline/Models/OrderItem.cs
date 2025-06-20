
namespace QuanLyCuaHangDoChoiOnline.Models
{
    public class OrderItem
    {
        public string ItemID { get; set; }
        public string OrderID { get; set; }
        public string ToyID { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public int Discount { get; set; }

    }
}
