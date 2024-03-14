﻿using System.ComponentModel.DataAnnotations;

namespace KitchenStory.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "EmailId cannot be empty")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid Email address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,16}$", ErrorMessage = "Invalid Password")]
        public string Password { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
    }
}