using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067

namespace PROG3BPOEP1
{
    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //issue id 
        public string Location { get; set; } //issue location 
        public string Category { get; set; } //category of issue 
        public string Description { get; set; } //description of the issue 
        public List<string> AttachmentPaths { get; set; } = new List<string>(); 
        public DateTime CreatedAt { get; set; } = DateTime.Now; //time stamp 
        public string Status { get; set; } = "Submitted"; // placeholder for future status tracking


        public override string ToString()
        {
            return $"[{CreatedAt:g}] {Category} at {Location} — {Description?.Substring(0, Math.Min(40, Description.Length))}...";
        }
    }
}
