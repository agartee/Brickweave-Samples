using System;
using System.ComponentModel.DataAnnotations;

namespace BasicCqrs.WebApp.Models
{
    /// <summary>
    /// This class must exist because currently System.Text.Json deserialization does
    /// not support immutable objects. Since we do not want to compromise our domain 
    /// libraries by making command and query object mutable, a view model must be created.
    /// </summary>
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required] 
        public string LastName { get; set; }
    }
}
