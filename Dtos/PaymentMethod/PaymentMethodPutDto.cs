namespace HotelWebApi.Dtos.PaymentMethod
{
    public class PaymentMethodPutDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
