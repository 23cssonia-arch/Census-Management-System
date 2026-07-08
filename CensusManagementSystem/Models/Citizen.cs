using System;

namespace CensusManagementSystem.Models
{
    public class Citizen
    {
        public int CitizenId { get; set; }
        public string CNIC { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string MaritalStatus { get; set; }
        public string Education { get; set; }
        public string Occupation { get; set; }
        public string RelationshipWithHead { get; set; }
        public int HouseholdId { get; set; }
        public string HouseNumber { get; set; }
    }
}
