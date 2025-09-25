using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.Currency
{
    public class CurrencyCreateDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string Symbol { get; set; }
        public bool isBase { get; set; }

    }
}
