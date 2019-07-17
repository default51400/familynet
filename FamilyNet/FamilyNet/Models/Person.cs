﻿using FamilyNet.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyNet.Models
{
    public class Person : IEntity
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter Full Name")]
        public virtual FullName FullName { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual  Adress Address { get; set; }
        [Required]
        public virtual Contacts Contacts { get; set; }
        public float Rating { get; set; }
        //public byte[] Avatar { get; set; }

        public virtual void CopyState(Person sender)
        {
            FullName.Name = sender.FullName.Name;
            FullName.Surname = sender.FullName.Surname;
            FullName.Patronymic = sender.FullName.Patronymic;
            Address.City = sender.Address.City;
            Address.Country = sender.Address.Country;
            Address.House = sender.Address.House;
            Address.Region = sender.Address.Region;
            Address.Street = sender.Address.Street;
            Contacts.Email = sender.Contacts.Email;
            Contacts.Phone = sender.Contacts.Phone;
            Birthday = sender.Birthday;

        }
    }
}
