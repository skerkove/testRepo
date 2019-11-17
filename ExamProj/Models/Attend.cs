using System.ComponentModel.DataAnnotations;

namespace ExamProj.Models
{
    public class Attend
    {
        [Key]
        public int AttendId {get;set;}
        public int UserId {get;set;}
        public int OccasionId {get;set;}
        public User Participant {get;set;}
        public Occasion Attending {get;set;} 
    }
}