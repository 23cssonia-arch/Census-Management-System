namespace CensusManagementSystem.Models
{
    public class CensusBlock
    {
        public int CensusBlockId { get; set; }
        public string BlockCode { get; set; }
        public string BlockName { get; set; }
        public int UnionCouncilId { get; set; }
        public string UnionCouncilName { get; set; }
    }
}
