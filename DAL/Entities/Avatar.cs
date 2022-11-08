﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Avatar : Attach
    {
        public virtual User Owner { get; set; } = null!;
        public Guid OwnerId { get; set; }
    }
}
