using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using PROG3BPOEP1;
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
    public class ReportIssueForm : Form
    {
        // Basic input controls
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox rtbDescription;
        private ListBox lstAttachments;

        // Action buttons
        private Button btnAttach;
        private Button btnSubmit;
        private Button btnBack;

        // UI elements for lightweight UX feedback
        private ProgressBar progress;
        private Label lblEngagement;

        // In-memory list of attachment file paths for the current report
        private readonly List<string> attachments = new List<string>();

        // Constructor: build UI, hook events and set initial engagement state
        public ReportIssueForm()
        {
            InitializeComponent();
            WireUpEvents();
            UpdateEngagement();
        }

        // InitializeComponent builds and lays out all UI controls
        private void InitializeComponent()
        {
            this.Text = "Report an Issue";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(800, 600);

            // Apply a project-wide UI them
            UIStyles.ApplyTheme(this);

            // Header label: large descriptive header
            var lblHeader = new Label();
            UIStyles.StyleMainHeader(lblHeader, "Help us serve you better — Report Issues Here", 64);

            // Location label + text input
            var lblLocation = new Label(); UIStyles.ApplyLabel(lblLocation, "📍 Location (Street, Suburb):");
            txtLocation = new TextBox(); UIStyles.ApplyTextBox(txtLocation, UIStyles.InputWidthLarge);

            // Category label + combobox
            var lblCategory = new Label(); //declare the label
            UIStyles.ApplyLabel(lblCategory, "📂 Category:"); //styling applied

            cmbCategory = new ComboBox();
            UIStyles.ApplyComboBox(cmbCategory, 300);

            // Provide a sensible default list of categories. These will be shown if AppState
            // does not provide IssueCategories via reflection.
            string[] categories = { "Road", "Electricity", "Water", "Sanitation", "Other" };
            cmbCategory.Items.AddRange(categories);

            // Try to read a static AppState.IssueCategories property by reflection so the
            // form will automatically use whatever categories the app provides.
            try
            {
                var appStateType = Type.GetType("PROG3BPOEP1.AppState");
                if (appStateType != null)
                {
                    var prop = appStateType.GetProperty("IssueCategories", BindingFlags.Public | BindingFlags.Static);
                    if (prop != null)
                    {
                        var value = prop.GetValue(null);
                        // If found, coerce to IEnumerable<string> or string[] before using
                        if (value is IEnumerable<string> seq)
                            categories = seq.ToArray();
                        else if (value is string[] arr)
                            categories = arr;
                    }
                }
            }
            catch
            {             
                // Reflection failures should not stop the form from working.
            }

            // If reflection supplied categories, add them to the combobox 
            if (categories.Length > 0)
                cmbCategory.Items.AddRange(categories);

            // Select the first category by default so the form is in a valid state initially.
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;

            // Description label + rich text box for multi-line input
            var lblDesc = new Label(); UIStyles.ApplyLabel(lblDesc, "📝 Description:");
            rtbDescription = new RichTextBox(); UIStyles.ApplyRichText(rtbDescription, UIStyles.InputWidthLarge, 160);

            // Attachment area: label, list box and Add button
            var lblAttach = new Label(); UIStyles.ApplyLabel(lblAttach, "📎 Attachments:");
            lstAttachments = new ListBox(); UIStyles.ApplyListBox(lstAttachments, UIStyles.InputWidthLarge, 120);
            btnAttach = new Button(); UIStyles.ApplySecondaryButton(btnAttach, "Add Files…", 120, UIStyles.ButtonHeight - 8);

            // Progress bar and engagement label give the user feedback while filling out the form
            progress = new ProgressBar { Width = UIStyles.InputWidthLarge, Height = 18, Minimum = 0, Maximum = 100, Value = 0, Margin = new Padding(3, 10, 3, 5) };
            lblEngagement = new Label(); UIStyles.ApplyLabel(lblEngagement, "Let's get started!");

            // Submit and Back buttons
            btnSubmit = new Button(); UIStyles.ApplyPrimaryButton(btnSubmit, "Submit Issue", 160, UIStyles.ButtonHeight);
            btnBack = new Button(); UIStyles.ApplySecondaryButton(btnBack, "Back to Main Menu", 140, UIStyles.ButtonHeight - 8);

            // Layout: a single-column TableLayoutPanel that stacks controls vertically
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), ColumnCount = 1, AutoScroll = true };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Helper to add controls as rows to the table layout
            void Add(Control c)
            {
                int r = layout.RowCount++;
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layout.Controls.Add(c, 0, r);
            }

            // Build the UI by adding each control in order
            Add(lblHeader);
            Add(progress);
            Add(lblEngagement);
            Add(lblLocation);
            Add(txtLocation);
            Add(lblCategory);
            Add(cmbCategory);
            Add(lblDesc);
            Add(rtbDescription);
            Add(lblAttach);
            Add(lstAttachments);

            // Attach button sits in a FlowLayoutPanel (to allow more controls later if needed)
            var attachRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            attachRow.Controls.Add(btnAttach);
            Add(attachRow);

            // Action buttons row
            var actions = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            actions.Controls.Add(btnBack);
            actions.Controls.Add(btnSubmit);
            Add(actions);

            // Finally add the constructed layout panel to the form
            this.Controls.Add(layout);
        }

        // WireUpEvents attaches handlers to UI events. 
        private void WireUpEvents()
        {
            txtLocation.TextChanged += (s, e) => UpdateEngagement();
            cmbCategory.SelectedIndexChanged += (s, e) => UpdateEngagement();
            rtbDescription.TextChanged += (s, e) => UpdateEngagement();

            // Button clicks invoke the corresponding methods
            btnAttach.Click += (s, e) => OnAttach();
            btnSubmit.Click += (s, e) => OnSubmit();
            btnBack.Click += (s, e) =>
            {
                // Close this dialog and show the main form
                this.Close();
                var main = new PROG3BPOEP1.Forms.MainForm();
                main.Show();
            };
        }

        // OnAttach lets the user pick files via OpenFileDialog and stores the chosen paths
        private void OnAttach()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select images or documents";
                dlg.Filter = "Images and Documents|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx|All Files|*.*";
                dlg.Multiselect = true;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Add every selected file to attachments, but ignore files larger than 25MB
                    foreach (var path in dlg.FileNames)
                    {
                        try
                        {
                            var fi = new FileInfo(path);
                            if (fi.Exists && fi.Length <= 25 * 1024 * 1024) attachments.Add(path);
                        }
                        catch { /* Silently ignore invalid files or IO errors */ }
                    }
                    RefreshAttachmentList();
                    UpdateEngagement();
                }
            }
        }

        // RefreshAttachmentList updates the list box to show attachment file names only
        private void RefreshAttachmentList()
        {
            lstAttachments.Items.Clear();
            foreach (var a in attachments) lstAttachments.Items.Add(Path.GetFileName(a));
        }

        // OnSubmit validates form inputs, creates an Issue object, stores it in AppState and
        // optionally shows a rating dialog. It then clears the form and returns to MainForm.
        private void OnSubmit()
        {
            var location = (txtLocation.Text ?? string.Empty).Trim();
            var category = cmbCategory.SelectedItem?.ToString() ?? string.Empty;
            var description = (rtbDescription.Text ?? string.Empty).Trim();

            // Simple validation with user-facing MessageBoxes
            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show(this, "Please enter the location of the issue.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show(this, "Please select a category.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.DroppedDown = true; // open the drop-down to nudge the user
                return;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show(this, "Please provide a brief description.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return;
            }

            // Build the Issue model and capture attachments
            var issue = new Issue
            {
                Location = location,
                Category = category,
                Description = description,
                AttachmentPaths = attachments.ToList()
            };

            // Add to the shared AppState - consumers elsewhere in the app should pick this up
            AppState.Issues.Add(issue);

            // Inform the user that submission succeeded and show a reference Id
            MessageBox.Show(this, $"Thank you! Your issue has been submitted.\n\nReference: {issue.Id}\nCategory: {issue.Category}\nLocation: {issue.Location}", "Submission Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset the form for another submission (clear fields and attachments)
            txtLocation.Clear();
            rtbDescription.Clear();
            attachments.Clear();
            RefreshAttachmentList();
            UpdateEngagement();

            // Show a RateExperienceForm modal so the user can rate their reporting experience
            using (var rateForm = new RateExperienceForm(issue.Id)) rateForm.ShowDialog(this);

            // Close this form and return to main menu
            this.Close();
            var main = new PROG3BPOEP1.Forms.MainForm();
            main.Show();
        }

        // UpdateEngagement computes a simple completeness score for the form and sets the
        // progress bar and a small prompt label guiding the user to the next step.
        private void UpdateEngagement()
        {
            int score = 0;
            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) score += 35; // location is important
            if (cmbCategory.SelectedItem != null) score += 25; // category selection matters
            if (!string.IsNullOrWhiteSpace(rtbDescription.Text)) score += 30; // description carries weight
            if (attachments.Count > 0) score += 10; // attachments are optional but add to completeness

            // Clamp the score to the progress bar range
            progress.Value = Math.Max(progress.Minimum, Math.Min(progress.Maximum, score));

            // Provide a friendly message depending on how complete the form is
            string message;
            if (score < 35) message = "Add the location.";
            else if (score < 60) message = "Choose a category.";
            else if (score < 90) message = "Add a short description.";
            else if (score < 100) message = "Attach a photo or document (optional).";
            else message = "Ready to submit! Thank you for helping your community!";

            lblEngagement.Text = message;
        }
    }
}
