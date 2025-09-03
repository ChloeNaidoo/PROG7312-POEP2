using System;

namespace PROG3BPOEP1.Models
{
    public class Feedback
    {
        public Guid IssueId { get; set; }         // Linking feedback to an issue 
        public string Rating { get; set; }        // feedback rating 
        public string Comment { get; set; }       // add comment 
        public DateTime DateSubmitted { get; set; } // show date submitted 
    }
}
