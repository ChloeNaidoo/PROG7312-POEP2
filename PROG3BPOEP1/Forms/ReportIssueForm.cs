using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PROG3BPOEP1.Models;

// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067

namespace PROG3BPOEP1
    {
        public class ReportIssueForm : Form
        {
            private TextBox txtLocation;
            private ComboBox cmbCategory;
            private RichTextBox rtbDescription;
            private ListBox lstAttachments;

            private Button btnAttach;
            private Button btnSubmit;
            private Button btnBack;

            private ProgressBar progress;
            private Label lblEngagement;

            private readonly List<string> attachments = new List<string>();

            public ReportIssueForm()
            {
                InitializeComponent();
                WireUpEvents();
                UpdateEngagement();
            }

            private void InitializeComponent()
            {
                Text = "Report an Issue";
                StartPosition = FormStartPosition.CenterScreen;
                MinimumSize = new Size(800, 600);
                BackColor = UIStyles.Background;

                // Header
                var lblHeader = new Label();
                UIStyles.ApplyHeader(lblHeader, "Help us serve you better — Report Issues Here", 60);

                // Location
                var lblLocation = new Label();
                UIStyles.ApplyLabel(lblLocation, "📍 Location (Street, Suburb):");
                txtLocation = new TextBox();
                UIStyles.ApplyTextBox(txtLocation);

                // Category
                var lblCategory = new Label();
                UIStyles.ApplyLabel(lblCategory, "📂 Category:");
                cmbCategory = new ComboBox();
                UIStyles.ApplyComboBox(cmbCategory);
                cmbCategory.Items.AddRange(AppState.IssueCategories);
                if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;

                // Description
                var lblDescription = new Label();
                UIStyles.ApplyLabel(lblDescription, "📝 Description:");
                rtbDescription = new RichTextBox();
                UIStyles.ApplyRichText(rtbDescription);

                // Attachments
                var lblMedia = new Label();
                UIStyles.ApplyLabel(lblMedia, "📎 Attachments:");
                lstAttachments = new ListBox();
                UIStyles.ApplyListBox(lstAttachments);
                btnAttach = new Button();
                UIStyles.ApplySecondaryButton(btnAttach, "Add Files…");

                // Progress + engagement
                progress = new ProgressBar
                {
                    Width = 500,
                    Height = 18,
                    Minimum = 0,
                    Maximum = 100,
                    Value = 0,
                    Margin = new Padding(3, 10, 3, 5)
                };
                lblEngagement = new Label();
                UIStyles.ApplyLabel(lblEngagement, "Let's get started!", italic: true);

                // Buttons
                btnSubmit = new Button();
                UIStyles.ApplyPrimaryButton(btnSubmit, "Submit Issue");
                btnBack = new Button();
                UIStyles.ApplySecondaryButton(btnBack, "Back to Main Menu");

                // Layout: stack everything vertically
                var layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20),
                    ColumnCount = 1,
                    AutoScroll = true
                };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                void AddRow(Control ctrl)
                {
                    int row = layout.RowCount++;
                    layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    layout.Controls.Add(ctrl, 0, row);
                }

                AddRow(lblHeader);
                AddRow(progress);
                AddRow(lblEngagement);
                AddRow(lblLocation);
                AddRow(txtLocation);
                AddRow(lblCategory);
                AddRow(cmbCategory);
                AddRow(lblDescription);
                AddRow(rtbDescription);
                AddRow(lblMedia);
                AddRow(lstAttachments);
                AddRow(btnAttach);
                

                // Button row
                var actionPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(0, 10, 0, 0)
                };
                actionPanel.Controls.Add(btnBack);
                actionPanel.Controls.Add(btnSubmit);
                AddRow(actionPanel);

                Controls.Add(layout);
            }

            private void WireUpEvents()
            {
                txtLocation.TextChanged += (s, e) => UpdateEngagement();
                cmbCategory.SelectedIndexChanged += (s, e) => UpdateEngagement();
                rtbDescription.TextChanged += (s, e) => UpdateEngagement();

                btnAttach.Click += (s, e) => OnAttach();
                btnSubmit.Click += (s, e) => OnSubmit();
                btnBack.Click += (s, e) =>
                {
                    this.Close();
                    var main = new Forms.MainForm();
                    main.Show();
                };
            }

            private void OnAttach()
            {
                using (var dlg = new OpenFileDialog())
                {
                    dlg.Title = "Select images or documents";
                    dlg.Filter = "Images and Documents|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx|All Files|*.*";
                    dlg.Multiselect = true;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        foreach (var path in dlg.FileNames)
                        {
                            try
                            {
                                var fi = new FileInfo(path);
                                if (fi.Exists && fi.Length <= 25 * 1024 * 1024)
                                {
                                    attachments.Add(path);
                                }
                            }
                            catch { }
                        }
                        RefreshAttachmentList();
                        UpdateEngagement();
                    }
                }
            }

            private void RefreshAttachmentList()
            {
                lstAttachments.Items.Clear();
                foreach (var a in attachments)
                {
                    lstAttachments.Items.Add(Path.GetFileName(a));
                }
            }

            private void OnSubmit()
            {
                var location = (txtLocation.Text ?? string.Empty).Trim();
                var category = cmbCategory.SelectedItem?.ToString() ?? string.Empty;
                var description = (rtbDescription.Text ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(location))
                {
                    MessageBox.Show(this, "Please enter the location of the issue.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLocation.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(category))
                {
                    MessageBox.Show(this, "Please select a category.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCategory.DroppedDown = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show(this, "Please provide a brief description.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rtbDescription.Focus();
                    return;
                }

                var issue = new Issue
                {
                    Location = location,
                    Category = category,
                    Description = description,
                    AttachmentPaths = attachments.ToList()
                };

                AppState.Issues.Add(issue);

                MessageBox.Show(this,
                    $"Thank you! Your issue has been submitted.\n\nReference: {issue.Id}\nCategory: {issue.Category}\nLocation: {issue.Location}",
                    "Submission Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtLocation.Clear();
                rtbDescription.Clear();
                attachments.Clear();
                RefreshAttachmentList();
                UpdateEngagement();

                using (var rateForm = new RateExperienceForm(issue.Id))
                {
                    rateForm.ShowDialog(this);
                }

                this.Close();
                var main = new Forms.MainForm();
                main.Show();
            }

            private void UpdateEngagement()
            {
                int score = 0;
                if (!string.IsNullOrWhiteSpace(txtLocation.Text)) score += 35;
                if (cmbCategory.SelectedItem != null) score += 25;
                if (!string.IsNullOrWhiteSpace(rtbDescription.Text)) score += 30;
                if (attachments.Count > 0) score += 10;

                progress.Value = Math.Max(progress.Minimum, Math.Min(progress.Maximum, score));

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
