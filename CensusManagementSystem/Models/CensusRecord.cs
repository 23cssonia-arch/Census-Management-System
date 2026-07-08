using System;

namespace CensusManagementSystem.Models
{
    public class CensusRecord
    {
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public int CitizenId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string UserName { get; set; }
        public string CitizenName { get; set; }
    }
}
