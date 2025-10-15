using System;
using System.Drawing;
using System.Windows.Forms;
using PROG3BPOEP1.Models;

// 
// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067
//

namespace PROG3BPOEP1
{
    public class RateExperienceForm : Form
    {
        // Stores the unique ID of the issue being rated
        private readonly Guid issueId;

        // UI components for the rating selection and submission
        private ComboBox cmbRating;
        private Button btnSubmit;


        // Constructor that accepts the issue ID being rated.
        public RateExperienceForm(Guid issueId)
        {
            this.issueId = issueId;
            InitializeComponent(); // Initialize UI elements
        }

        // Initializes and configures the UI components on the form.
        private void InitializeComponent()
        {
            // Set basic form properties
            Text = "Rate Your Experience";
            StartPosition = FormStartPosition.CenterParent; // Center form within parent
            Size = new Size(450, 250); // Form size
            BackColor = UIStyles.Background; // Use theme background color

            // Header Label
            var lblHeader = new Label();
            UIStyles.ApplyHeader(lblHeader, "Rate your experience", 60);
            // Displays the title text at the top of the form

            // Rating ComboBox
            cmbRating = new ComboBox();
            UIStyles.ApplyComboBox(cmbRating, 220); // Apply consistent styling from UIStyles

            // Add rating options to dropdown
            cmbRating.Items.AddRange(new string[]
            {
                "Very Poor",
                "Poor",
                "Average",
                "Good",
                "Excellent"
            });

            // Default selection is "Average"
            cmbRating.SelectedIndex = 2;

            // Submit Button 
            btnSubmit = new Button();
            UIStyles.ApplyPrimaryButton(btnSubmit, "Submit Feedback");
            // Apply button styling and text
            btnSubmit.Click += OnSubmit; // Attach event handler for button click

            // Layout Panel 
            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,             // Fills entire form
                FlowDirection = FlowDirection.TopDown, // Stack controls vertically
                Padding = new Padding(20),         // Add space around edges
                AutoScroll = true                  // Enable scrolling if needed
            };

            // Add controls to layout panel in order
            layout.Controls.Add(lblHeader);
            layout.Controls.Add(cmbRating);
            layout.Controls.Add(btnSubmit);

            // Add layout panel to form controls
            Controls.Add(layout);
        }

        // Handles the click event for the Submit button.
        // Saves the user's feedback and closes the form.
        private void OnSubmit(object sender, EventArgs e)
        {
            // Retrieve the selected rating (default to "No Rating" if none selected)
            string rating = cmbRating.SelectedItem?.ToString() ?? "No Rating";

            // Store the feedback in the global application state
            AppState.Feedback.Add(new Feedback
            {
                IssueId = issueId,          // Link feedback to the issue
                Rating = rating,            // Store the selected rating
                DateSubmitted = DateTime.Now // Record submission date
            });

            // Show confirmation message to user
            MessageBox.Show(
                this,
                "Thank you for your feedback!",
                "Feedback Received",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Close the feedback form
            Close();
        }
    }
}
