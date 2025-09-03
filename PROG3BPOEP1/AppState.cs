using System;
using System.Collections.Generic;
using PROG3BPOEP1.Models;

// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067

namespace PROG3BPOEP1
{
    public static class AppState
    {
        // In-memory store for reported issues
        public static readonly List<Issue> Issues = new List<Issue>();

        // In-memory store for user feedback (ratings/comments)
        public static readonly List<Feedback> Feedback = new List<Feedback>();

        // Predefined categories for consistency across forms
        public static readonly string[] IssueCategories = new[]
        {
            "Sanitation",
            "Roads",
            "Utilities",
            "Parks & Recreation",
            "Public Safety",
            "Other"
        };
    }
}
