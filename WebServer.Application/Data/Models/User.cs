using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
       
        public string Username { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public string Password { get; set; }

    }
}

