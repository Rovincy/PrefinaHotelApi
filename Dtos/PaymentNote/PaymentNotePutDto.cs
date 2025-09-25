namespace HotelWebApi.Dtos.PaymentNote
{
    public class PaymentNotePutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPayment { get; set; }
        public bool? IsCredit { get; set; }
        public bool? IsDebit { get; set; }
    }
}
