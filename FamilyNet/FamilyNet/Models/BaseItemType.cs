﻿using FamilyNet.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamilyNet.Models
{
    public class BaseItemType : IEntity
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<BaseItemType> Childs { get; set; }
        public ICollection<BaseItemType> Parent { get; set; }
    }
}