using System;
using System.Drawing;
using System.Windows.Forms;
using PROG3BPOEP1.Models;

// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067

namespace PROG3BPOEP1
{
    public class RateExperienceForm : Form
    {
        private readonly Guid issueId;
        private ComboBox cmbRating;
        private Button btnSubmit;

        public RateExperienceForm(Guid issueId)
        {
            this.issueId = issueId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Rate Your Experience";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(450, 250);
            BackColor = UIStyles.Background;

            var lblHeader = new Label(); 
            UIStyles.ApplyHeader(lblHeader, "How was your experience submitting this issue?", 60); //question

            cmbRating = new ComboBox();
            UIStyles.ApplyComboBox(cmbRating, 220);  //ratings 
            cmbRating.Items.AddRange(new string[]
            {
                "Very Poor",
                "Poor",
                "Average",
                "Good",
                "Excellent"
            });
            cmbRating.SelectedIndex = 2;

            btnSubmit = new Button();
            UIStyles.ApplyPrimaryButton(btnSubmit, "Submit Feedback"); //submit button 
            btnSubmit.Click += OnSubmit;

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(20),
                AutoScroll = true
            };

            layout.Controls.Add(lblHeader);
            layout.Controls.Add(cmbRating);
            layout.Controls.Add(btnSubmit);

            Controls.Add(layout);
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            string rating = cmbRating.SelectedItem?.ToString() ?? "No Rating";

            AppState.Feedback.Add(new Feedback
            {
                IssueId = issueId,
                Rating = rating,
                DateSubmitted = DateTime.Now
            });

            MessageBox.Show(this, "Thank you for your feedback!", "Feedback Received",
                MessageBoxButtons.OK, MessageBoxIcon.Information); //output message 

            Close();
        }
    }
}
