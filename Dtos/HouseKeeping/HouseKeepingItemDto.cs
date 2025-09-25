namespace HotelWebApi.Dtos.HouseKeeping
{
    public class HouseKeepingItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int ServiceCategoryId { get; set; }
    }
}
