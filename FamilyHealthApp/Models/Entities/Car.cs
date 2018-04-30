using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyHealthApp.Models
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int CarId { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        // Navigation 
        public ICollection<Comment> Comments { get; set; }

        [ForeignKey("AspNetUserId")]
        public ApplicationUser User { get; set; }
        public string AspNetUserId { get; set; }
    }
}
 