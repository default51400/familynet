﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyNet.Models
{
    public class CharityMaker : Person
    {
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
