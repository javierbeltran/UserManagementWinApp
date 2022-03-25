using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;
namespace UserManagement.Library.Models
{
    [Dapper.Contrib.Extensions.Table("User")]
    public class User
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
    }
}
