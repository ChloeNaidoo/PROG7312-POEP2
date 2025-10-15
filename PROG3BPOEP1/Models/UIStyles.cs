using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

// 
// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067
// 

namespace PROG3BPOEP1.Models
{
    public static class UIStyles
    {
        //  Fonts 
        // Font used for the main app title.
        public static readonly Font AppTitleFont = new Font("Segoe UI Semibold", 20, FontStyle.Bold);
        //Primary header font for page/section titles.
        public static readonly Font HeaderFont = new Font("Segoe UI", 18, FontStyle.Bold);
        //Smaller section header font for sub-sections.
        public static readonly Font SectionHeaderFont = new Font("Segoe UI", 12, FontStyle.Bold);
        //Standard label font used for form field labels.
        public static readonly Font LabelFont = new Font("Segoe UI", 10, FontStyle.Regular);
        //Default body font used for most text controls.
        public static readonly Font BodyFont = new Font("Segoe UI", 10, FontStyle.Regular);
        //Smaller italic font for helper text, hints or footnotes.
        public static readonly Font SmallItalic = new Font("Segoe UI", 9, FontStyle.Italic);
        //Font used on primary buttons.
        public static readonly Font ButtonFont = new Font("Segoe UI", 10, FontStyle.Bold);

        // ---------- Colors ----------
        //Primary brand color (used for accents and primary buttons).
        public static readonly Color PrimaryColor = Color.FromArgb(0, 120, 215);
        //Color used when hovering primary controls.
        public static readonly Color PrimaryHover = Color.FromArgb(0, 100, 190);
        //Accent color for section headers and secondary emphasis.
        public static readonly Color AccentColor = Color.FromArgb(40, 40, 60);
        //Global background color for most forms/panels.
        public static readonly Color Background = Color.White;
        //Muted text color for less prominent labels or placeholder text.
        public static readonly Color MutedText = Color.FromArgb(100, 100, 100);
        //Light gray, useful for subtle backgrounds and separators.
        public static readonly Color LightGray = Color.FromArgb(240, 240, 240);
        //Border gray for dividing lines and control borders.
        public static readonly Color BorderGray = Color.FromArgb(220, 220, 220);

        // Standard sizes 
        //Recommended main form width.
        public const int MainFormWidth = 1000;
        //Recommended main form height.
        public const int MainFormHeight = 700;
        //Standard button height used across the UI.
        public const int ButtonHeight = 48;
        //Large button width for wide call-to-action buttons.
        public const int ButtonWidthLarge = 420;
        //Normal button width for typical actions.
        public const int ButtonWidthNormal = 140;
        //Large input width for multi-column forms or long text inputs.
        public const int InputWidthLarge = 600;
        //Medium input width for standard inputs.
        public const int InputWidthMedium = 300;
        //Small input width for compact fields (e.g., small numbers, codes).
        public const int InputWidthSmall = 160;

        // Global helpers 
        public static void ApplyHeader(Label lbl, string text, int height = 70, ContentAlignment align = ContentAlignment.MiddleCenter)
        {
            if (lbl == null) return;
            lbl.Text = text;
            lbl.AutoSize = false;
            lbl.Dock = DockStyle.Top;
            lbl.Height = height;
            lbl.Padding = new Padding(0, 10, 0, 10);
            lbl.TextAlign = align;
            lbl.Font = HeaderFont;
            lbl.ForeColor = AccentColor;
            lbl.Margin = new Padding(0, 0, 0, 12);
        }
        public static void ApplyTheme(Form form, bool center = true)
        {
            if (form == null) return;
            form.BackColor = Background;
            form.Font = BodyFont;
            form.ForeColor = MutedText;
            if (center) form.StartPosition = FormStartPosition.CenterScreen;

            // Enable double buffering on the form and its child controls where possible
            try
            {
                var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                prop?.SetValue(form, true, null);

                // Recursively enable double-buffering for child controls to reduce redraw flicker.
                void SetRecursive(Control parent)
                {
                    foreach (Control c in parent.Controls)
                    {
                        try { prop?.SetValue(c, true, null); } catch { }
                        if (c.HasChildren) SetRecursive(c);
                    }
                }
                SetRecursive(form);
            }
            catch
            {
                //double-buffering is a best-effort optimization.
            }
        }

        public static void CenterPanel(Panel panel)
        {
            if (panel == null) return;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);
            panel.BackColor = Background;
            panel.AutoScroll = true;
        }

        public static void StyleMainHeader(Label lbl, string text, int height = 90)
        {
            if (lbl == null) return;
            lbl.Text = text;
            lbl.Font = AppTitleFont;
            lbl.ForeColor = PrimaryColor;
            lbl.AutoSize = false;
            lbl.Height = height;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Dock = DockStyle.Top;
            lbl.Padding = new Padding(10, 12, 10, 12);
            lbl.BackColor = Background;
        }

        // Styles a label as a section header with the section header font and spacing.
        public static void StyleSectionHeader(Label lbl, string text)
        {
            if (lbl == null) return;
            lbl.Text = text;
            lbl.Font = SectionHeaderFont;
            lbl.ForeColor = AccentColor;
            lbl.AutoSize = true;
            lbl.Padding = new Padding(0, 6, 0, 6);
        }

        // Standard label styling for form field labels (black text, standard spacing).
        public static void ApplyLabel(Label lbl, string text)
        {
            if (lbl == null) return;
            lbl.Text = text;
            lbl.Font = LabelFont;
            lbl.ForeColor = Color.Black;
            lbl.AutoSize = true;
            lbl.Margin = new Padding(3, 6, 3, 3);
        }

        public static void ApplyButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Color.FromArgb(70, 130, 180); // SteelBlue
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;

            // Hover effect
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(100, 149, 237); // CornflowerBlue
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(70, 130, 180);
        }


        // Styles a button as the primary (call-to-action) button.
        public static void ApplyPrimaryButton(Button btn, string text, int width = ButtonWidthNormal, int height = ButtonHeight)
        {
            if (btn == null) return;
            btn.Text = text;
            btn.Font = ButtonFont;
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
            btn.Width = width;
            btn.Height = height;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Margin = new Padding(6);
            btn.TextAlign = ContentAlignment.MiddleCenter;

            // Ensure we don't attach duplicate event handlers
            btn.MouseEnter -= PrimaryButton_MouseEnter;
            btn.MouseLeave -= PrimaryButton_MouseLeave;
            btn.MouseEnter += PrimaryButton_MouseEnter;
            btn.MouseLeave += PrimaryButton_MouseLeave;
        }

        // Hover effects for primary buttons
        private static void PrimaryButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn) btn.BackColor = PrimaryHover;
        }
        private static void PrimaryButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn) btn.BackColor = PrimaryColor;
        }

        // Styles a button as a secondary action button (neutral appearance).
        public static void ApplySecondaryButton(Button btn, string text, int width = ButtonWidthNormal, int height = ButtonHeight - 8)
        {
            if (btn == null) return;
            btn.Text = text;
            btn.Font = BodyFont;
            btn.BackColor = LightGray;
            btn.ForeColor = AccentColor;
            btn.Width = width;
            btn.Height = height;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Margin = new Padding(6);
        }

        // Applies standard sizing and font to a TextBox control.
        public static void ApplyTextBox(TextBox txt, int width = InputWidthMedium)
        {
            if (txt == null) return;
            txt.Width = width;
            txt.Font = BodyFont;
            txt.Margin = new Padding(3, 6, 3, 10);
        }

        // Configures a ComboBox for use as a dropdown selector (read-only selection).
        public static void ApplyComboBox(ComboBox cmb, int width = InputWidthMedium)
        {
            if (cmb == null) return;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Width = width;
            cmb.Font = BodyFont;
            cmb.Margin = new Padding(3, 6, 3, 10);
        }

        // Styles a DateTimePicker with consistent font, width and short date format.
        public static void ApplyDatePicker(DateTimePicker dp, int width = InputWidthSmall)
        {
            if (dp == null) return;
            dp.Width = width;
            dp.Font = BodyFont;
            dp.Margin = new Padding(3, 6, 3, 10);
            dp.Format = DateTimePickerFormat.Short;
        }

        // Styles a RichTextBox for longer text input (default width/height provided).
        public static void ApplyRichText(RichTextBox rtb, int width = InputWidthLarge, int height = 120)
        {
            if (rtb == null) return;
            rtb.Width = width;
            rtb.Height = height;
            rtb.Font = BodyFont;
            rtb.Margin = new Padding(3, 6, 3, 10);
        }

        // Applies consistent sizing, font, margin and border style to a ListBox.
        public static void ApplyListBox(ListBox lst, int width = 400, int height = 120)
        {
            if (lst == null) return;
            lst.Width = width;
            lst.Height = height;
            lst.Font = BodyFont;
            lst.Margin = new Padding(3, 6, 3, 10);
            lst.BorderStyle = BorderStyle.FixedSingle;
        }

        // Creates and returns a simple "card" styled panel with padding and background set.
        public static Panel MakeCardPanel()
        {
            var p = new Panel
            {
                BackColor = Background,
                Padding = new Padding(8),
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };
            return p;
        }
    }
}
