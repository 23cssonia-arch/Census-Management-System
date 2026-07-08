namespace CensusManagementSystem.Models
{
    public class Household
    {
        public int HouseholdId { get; set; }
        public string HouseNumber { get; set; }
        public string FamilyNumber { get; set; }
        public string Address { get; set; }
        public string HeadOfFamily { get; set; }
        public int NumberOfFamilyMembers { get; set; }
        public int CensusBlockId { get; set; }
        public string BlockName { get; set; }
    }
}
