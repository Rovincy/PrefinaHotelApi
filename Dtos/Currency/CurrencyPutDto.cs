namespace HotelWebApi.Dtos.Currency
{
    public class CurrencyPutDto
    {
        public int? Id { get; set; }
        public string? name { get; set; }
        public decimal? rate { get; set; }
        public bool? isBase { get; set; }
        public string? symbol { get; set; }
    }
}
