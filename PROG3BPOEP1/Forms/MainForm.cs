using System;
using System.Drawing;
using System.Windows.Forms;
using MunicipalServicesApp;
using PROG3BPOEP1.Models;

// 
// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067
// 

namespace PROG3BPOEP1.Forms
{
    // The main menu form for the Municipal Services Application.
    // Provides navigation options for reporting issues, viewing events, and checking service status.
    public class MainForm : Form
    {
        private Label headerLabel;       // App title at the top of the form
        private Panel mainPanel;         // Main container for central content
        private Button btnReportIssues;  // Opens the ReportIssueForm
        private Button btnLocalEvents;   // Opens the EventsForm
        private Button btnServiceStatus; // Placeholder for viewing service request status

        public MainForm()
        {
            InitializeComponent(); // Initialize the UI components when form is created
        }

        private void InitializeComponent()
        {
            // Base Form 
            this.Text = "Municipal Services Application"; 
            this.Width = 1000;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen; 
            UIStyles.ApplyTheme(this); 

            // Header 
            headerLabel = new Label();

            // Apply a consistent header style 
            UIStyles.ApplyHeader(headerLabel, "Welcome to the Municipal Services Application", 100);
            headerLabel.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            headerLabel.ForeColor = UIStyles.PrimaryColor; // primary app color
            headerLabel.BackColor = Color.White;
            headerLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text
            headerLabel.Dock = DockStyle.Top; // Dock header to top of form

            // Main Panel 
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,           // the rest of the form below the header
                BackColor = UIStyles.Background  // app background color
            };

            // Layout for Buttons 
            var layout = new TableLayoutPanel
            {
                RowCount = 3,
                ColumnCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10), // Padding around layout
                Dock = DockStyle.None      
            };

            // three main buttons
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400)); // Fixed width for all buttons

            // Buttons 
            // Report Issues Button — opens the ReportIssueForm for users to report problems.
            btnReportIssues = new Button();
            StyleButton(btnReportIssues, "Report Issues");
            btnReportIssues.Click += (s, e) =>
            {
                var reportForm = new ReportIssueForm();
                reportForm.ShowDialog(this); // Open as modal dialog
            };

            // Local Events Button — opens the EventsForm to view events and announcements.
            btnLocalEvents = new Button();
            StyleButton(btnLocalEvents, "Local Events and Announcements");
            btnLocalEvents.Click += (s, e) =>
            {
                var eventsForm = new EventsForm();
                eventsForm.ShowDialog(this);
            };

            // Service Status Button — placeholder (to be implemented later)
            btnServiceStatus = new Button();
            StyleButton(btnServiceStatus, "Service Request Status");
            btnServiceStatus.Click += (s, e) =>
            {
                MessageBox.Show("Service Request Status - To be implemented in Task 3",
                    "Service Request Status");
            };

            // Add buttons to layout grid
            layout.Controls.Add(btnReportIssues, 0, 0);
            layout.Controls.Add(btnLocalEvents, 0, 1);
            layout.Controls.Add(btnServiceStatus, 0, 2);

            // Center layout manually within mainPanel
            mainPanel.Controls.Add(layout);
            layout.Anchor = AnchorStyles.None;
            layout.Location = new Point(
                (mainPanel.Width - layout.Width) / 2,
                (mainPanel.Height - layout.Height) / 2
            );

            // When the panel resizes, re-center the layout
            mainPanel.Resize += (s, e) =>
            {
                layout.Location = new Point(
                    (mainPanel.Width - layout.Width) / 2,
                    (mainPanel.Height - layout.Height) / 2
                );
            };

            // Add Controls to Form 
            this.Controls.Add(mainPanel);
            this.Controls.Add(headerLabel);
        }
 
        /// apply consistent style to main menu buttons.
        /// Adds bold fonts, app colors, rounded feel, and hover effects.
        private void StyleButton(Button btn, string text)
        {
            btn.Text = text; // Set displayed button text
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.Width = 400;
            btn.Height = 60;
            btn.BackColor = UIStyles.PrimaryColor; // Primary color for branding
            btn.ForeColor = Color.White; // White text for contrast
            btn.FlatStyle = FlatStyle.Flat; // Clean modern look
            btn.FlatAppearance.BorderSize = 0; // Remove border
            btn.Margin = new Padding(10); // Add spacing between buttons
            btn.Cursor = Cursors.Hand; // Show pointer cursor on hover

            // Add hover color change for better interactivity
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 100, 190);
            btn.MouseLeave += (s, e) => btn.BackColor = UIStyles.PrimaryColor;
        }
    }
}
