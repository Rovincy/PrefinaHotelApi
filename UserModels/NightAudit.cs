using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class NightAudit
    {
        public int Id { get; set; }
        public int? NumberOfTransaction { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
