using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

// Code Attribution:
// Author: Andrew Troelsen & Philip Japikse (2017)
// Book: "Pro C# 7: With .NET and .NET Core", 8th Edition, Apress.
// Student: Chloe Monique Naidoo
// ST10145067

namespace PROG3BPOEP1
{
    public static class UIStyles
    {
        // Fonts
        public static readonly Font TitleFont = new Font("Segoe UI", 16, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Segoe UI", 14, FontStyle.Bold);
        public static readonly Font LabelFont = new Font("Segoe UI", 10);
        public static readonly Font BodyFont = new Font("Segoe UI", 10);
        public static readonly Font ItalicFont = new Font("Segoe UI", 9, FontStyle.Italic);

        // Colors
        public static readonly Color PrimaryColor = Color.FromArgb(0, 120, 215);
        public static readonly Color AccentColor = Color.FromArgb(40, 40, 60);
        public static readonly Color LightGray = Color.LightGray;
        public static readonly Color Background = Color.White;

        //General form styling
        public static void CenterForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = Background;
        }

        // Center content 
        public static void CenterPanel(Panel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            panel.Padding = new Padding(20);
        }

        // header label
        public static void ApplyHeader(Label lbl, string text, int height = 70, ContentAlignment align = ContentAlignment.MiddleCenter)
        {
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


        // Apply section label
        public static void ApplyLabel(Label lbl, string text, bool italic = false)
        {
            lbl.Text = text;
            lbl.AutoSize = true;
            lbl.Font = italic ? ItalicFont : LabelFont;
            lbl.ForeColor = Color.Black;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Margin = new Padding(3, 10, 3, 3);
        }

        // main button style
        public static void ApplyPrimaryButton(Button btn, string text)
        {
            btn.Text = text;
            btn.Width = 180;
            btn.Height = 40;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Margin = new Padding(6);

            // Center horizontally 
            btn.Anchor = AnchorStyles.None;
            btn.AutoSize = false;
            btn.TextAlign = ContentAlignment.MiddleCenter;
        }

        //secondary button style
        public static void ApplySecondaryButton(Button btn, string text)
        {
            btn.Text = text;
            btn.Width = 160;
            btn.Height = 36;
            btn.Font = BodyFont;
            btn.BackColor = LightGray;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Margin = new Padding(6);
            btn.Anchor = AnchorStyles.None;
        }

        // text input
        public static void ApplyTextBox(TextBox txt, int width = 400)
        {
            txt.Width = width;
            txt.Font = BodyFont;
            txt.Margin = new Padding(3, 3, 3, 10);
            txt.Anchor = AnchorStyles.None;
        }

        // combo box
        public static void ApplyComboBox(ComboBox cmb, int width = 400)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Width = width;
            cmb.Font = BodyFont;
            cmb.Margin = new Padding(3, 3, 3, 10);
            cmb.Anchor = AnchorStyles.None;
        }

        // rich text
        public static void ApplyRichText(RichTextBox rtb, int width = 400, int height = 100)
        {
            rtb.Width = width;
            rtb.Height = height;
            rtb.Font = BodyFont;
            rtb.Margin = new Padding(3, 3, 3, 10);
            rtb.Anchor = AnchorStyles.None;
        }

        // list box
        public static void ApplyListBox(ListBox lst, int width = 400, int height = 80)
        {
            lst.Width = width;
            lst.Height = height;
            lst.Font = BodyFont;
            lst.Margin = new Padding(3, 3, 3, 10);
            lst.Anchor = AnchorStyles.None;
        }
    }
}
