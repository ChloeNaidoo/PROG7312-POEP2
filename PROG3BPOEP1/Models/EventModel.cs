using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace PROG3BPOEP1.Models
    {
        [Serializable]
        public class EventModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Location { get; set; }


            public override string ToString()
            {
                return $"{Title} ({Category}) — {StartDate:d} to {EndDate:d} @ {Location}";
            }
        }
    }
