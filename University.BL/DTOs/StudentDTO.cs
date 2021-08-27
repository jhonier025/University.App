using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace University.BL.DTOs
{
   public class StudentDTO
    {
        [Required(ErrorMessage = "The ID is required")]
        public int ID { get; set; }


        [Required(ErrorMessage = "The LASTNAME is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The FIRSTMIDName is required")]
        public string FirstMidName { get; set; }

        [Required(ErrorMessage = "The EnrollmentDate is required")]
        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "The FullName is required")]
        public string FullName { get; set; }
    }
}
