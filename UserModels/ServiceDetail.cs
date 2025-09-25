using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class ServiceDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ServiceCategoryId { get; set; }
        public string Price { get; set; }

        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
