using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExamProj.Models
{
    public class Occasion
    {
        [Key]
        public int OccasionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [FutureDate]
        public DateTime Date { get; set; }

        public string CoordinatorName {get; set;}
        public int Duration {get; set;}

        /////ASSOCIATIONS
        public int UserId {get;set;}

        
        public User Coordinator {get;set;}
        
        public List<Attend> Participants {get;set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }


    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            if (value is DateTime)
            {
                date = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid DateTime");
            }
            if (date < DateTime.Now)
            {
                return new ValidationResult("Date must be in the future!");
            }
            return ValidationResult.Success;
        }
    }
}