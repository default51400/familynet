﻿using FamilyNet.Models.Interfaces;
using System;

namespace FamilyNet.Models
{
    public class Donation : IEntity
    {
        public int ID { get; set; }
        public virtual DonationItem DonationItem { get; set; }
        public DateTime Date { get; set; }
    }
}