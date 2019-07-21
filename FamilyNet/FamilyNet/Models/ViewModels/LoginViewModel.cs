﻿using System.ComponentModel.DataAnnotations;

namespace FamilyNet.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        //[Display(Name = "Email")]
        [UIHint("email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[Display(Name = "Пароль")]
        [UIHint("password")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
        // Не работает пока что, что-то с сервисами куки надо сделать

        public string ReturnUrl { get; set; }
    }
}
