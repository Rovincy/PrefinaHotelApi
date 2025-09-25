namespace HotelWebApi.Dtos.PaymentNote
{
    public class PaymentNoteCreateDto
    {
        public string Name { get; set; }
        public bool? IsPayment { get; set; }
        public bool? IsCredit { get; set; }
        public bool? IsDebit { get; set; }
    }
}
