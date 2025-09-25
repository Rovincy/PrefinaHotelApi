namespace HotelWebApi.Dtos.Tax
{
    public class TaxPutDto
    {
        public int? Id { get; set; }
        public string? name { get; set; }
        public decimal? rate { get; set; }
        public bool? isLevy { get; set; }
    }
}
