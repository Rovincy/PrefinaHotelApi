using System;

namespace HotelWebApi.Dtos.AuditTrail
{
    public class AuditTrailCreateDto
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
