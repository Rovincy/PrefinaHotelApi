using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class PaymentMethod
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
