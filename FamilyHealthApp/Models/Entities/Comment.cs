using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyHealthApp.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int CommentId { get; set; }

        [Required]
        public string Text { get; set; }

        public int CarId { get; set; }

        // Navigation
        public Car Car { get; set; }


    }
} 
