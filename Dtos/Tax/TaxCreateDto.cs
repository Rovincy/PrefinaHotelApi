using System;

namespace HotelWebApi.Dtos.Tax
{
    public class TaxCreateDto
    {
        public string? name { get; set; }
        public decimal? rate { get; set; }
        public bool? isLevy { get; set; }
    }
}
