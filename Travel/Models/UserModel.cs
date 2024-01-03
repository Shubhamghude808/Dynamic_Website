using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Consult.Models
{
    public class UserModel
    {
        public string? Id { get; set; }
       
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone {  get; set; }

        public string? State { get; set; }

        public string? Place { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public string? Member { get; set; }

        public string? Room { get; set; }

        public string? From { get; set; }

        public string? To { get; set; }

        public string? Email2 { get; set; }

    }
}
