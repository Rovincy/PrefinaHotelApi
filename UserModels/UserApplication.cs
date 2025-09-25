﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HotelWebApi.UserModels
{
    public partial class UserApplication
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ApplicationId { get; set; }

        public virtual Application Application { get; set; }
        public virtual User User { get; set; }
    }
}
