using System;
using System.Drawing;
using System.Windows.Forms;

// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067

namespace PROG3BPOEP1.Forms
{
    public class MainForm : Form
    {
        private Button btnReportIssues;
        private Button btnEvents;
        private Button btnStatus;
        private Label lblTitle;

        public MainForm()
        {
            InitializeComponent();
            WireUpEvents();
        }

        private void InitializeComponent()
        {
            Text = "Community Services Portal";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(720, 480);
            BackColor = UIStyles.Background;

            lblTitle = new Label();
            UIStyles.ApplyHeader(lblTitle, "Welcome to Community Services", 80); //header 

            btnReportIssues = new Button();
            UIStyles.ApplyPrimaryButton(btnReportIssues, "Report an Issue"); //report issue button 

            btnEvents = new Button();
            UIStyles.ApplyPrimaryButton(btnEvents, "View Events"); //view events button 

            btnStatus = new Button();
            UIStyles.ApplyPrimaryButton(btnStatus, "Check Issue Status"); //check status button 

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(20),
                AutoScroll = true,
                WrapContents = false
            };

            buttonPanel.Controls.Add(btnReportIssues);
            buttonPanel.Controls.Add(btnEvents);
            buttonPanel.Controls.Add(btnStatus);

            Controls.Add(buttonPanel);
            Controls.Add(lblTitle);
        }

        private void WireUpEvents()
        {
            btnReportIssues.Click += (s, e) =>
            {
                var reportForm = new ReportIssueForm(); //report issue page 
                reportForm.Show();
                this.Hide();
            };

            btnEvents.Click += (s, e) =>
            {
                MessageBox.Show("Events page coming soon!"); //output message for events page 
            };

            btnStatus.Click += (s, e) =>
            {
                MessageBox.Show("Status page coming soon!"); //output message for status page 
            };
        }
    }
}
