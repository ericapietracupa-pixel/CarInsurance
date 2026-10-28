using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarInsurance.Models
{
    public class Insuree
    {
        public Guid Id { get; set; }
        
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Display(Name = "Email Address")]
        public required string EmailAddress { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public required DateTime DateOfBirth { get; set; }

        [Display(Name = "Car Year")]
        public required int CarYear { get; set; }

        [Display(Name = "Car Make")]
        public required string CarMake { get; set; }

        [Display(Name = "Car Model")]
        public required string CarModel { get; set; }
        public required bool DUI { get; set; }

        [Display(Name = "Speeding Tickets")]
        public required int SpeedingTickets { get; set; }

        [Display(Name = "Coverage Type")]
        public required bool CoverageType { get; set; }
        public decimal? Quote { get; set; }
    }
}
