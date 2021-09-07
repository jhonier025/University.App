using System;
using Newtonsoft.Json;

namespace University.BL.DTOs
{
    public class InstructorDTO
    {       
        public int ID { get; set; }        
        public string LastName { get; set; }        
        public string FirstMidName { get; set; }       
        public DateTime HireDate { get; set; }
        
        [JsonProperty("Full Name")]
        public string FullName { get; set; }

        
    }
}
