using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class HoursExtension
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hours { get; set; }
        public string IsCharged { get; set; }
        public string Tariff { get; set; }
    }
}
