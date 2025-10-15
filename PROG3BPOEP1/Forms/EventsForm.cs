using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public class EventsForm : Form
    {
        // UI COMPONENTS 
        private ListBox lstEvents;                 // Displays list of events
        private ListBox lstRecommendations;        // Displays recommended events based on user searches
        private ComboBox cmbCategories;            // Dropdown for category filtering
        private Label lblEvents;                   // Label for event list
        private Label lblRecommendations;          // Label for recommendations section
        private Label lblCategoryFilter;           // Label for category filter section

        // New: date filters and search buttons
        private Label lblDateFrom;
        private Label lblDateTo;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnSearch;
        private Button btnClearFilters;

        // DATA STRUCTURES 
        private Dictionary<Guid, EventModel> eventsById = new Dictionary<Guid, EventModel>();
        private HashSet<string> categories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> searchCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public EventsForm()
        {
            // BASIC FORM SETUP 
            this.Text = "Local Events and Recommendations";
            this.Width = 800;
            this.Height = 600;
            UIStyles.ApplyTheme(this); // Apply consistent visual theme

            InitializeComponents();

            // Add a few pre-defined sample events to populate the UI
            AddSampleEvents();
        }

        // Initializes all UI components and lays them out on the form.
        private void InitializeComponents()
        {
            // Header label
            var header = new Label();
            UIStyles.StyleMainHeader(header, "Local Events and Recommendations", 60);
            this.Controls.Add(header);

            // Category filter label
            lblCategoryFilter = new Label();
            lblCategoryFilter.Text = "Filter by Category:";
            lblCategoryFilter.Location = new Point(20, 80);
            lblCategoryFilter.AutoSize = true;
            this.Controls.Add(lblCategoryFilter);

            // Category ComboBox
            cmbCategories = new ComboBox();
            cmbCategories.Location = new Point(150, 75);
            cmbCategories.Width = 200;
            UIStyles.ApplyComboBox(cmbCategories);
            cmbCategories.SelectedIndexChanged += (s, e) => RefreshEventsList(); // When selection changes, update event list
            this.Controls.Add(cmbCategories);

            // Date range labels & pickers
            lblDateFrom = new Label();
            lblDateFrom.Text = "From:";
            lblDateFrom.Location = new Point(370, 80);
            lblDateFrom.AutoSize = true;
            this.Controls.Add(lblDateFrom);

            dtpFrom = new DateTimePicker();
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(410, 75);
            dtpFrom.Width = 110;
            dtpFrom.Value = DateTime.Today;
            dtpFrom.ValueChanged += (s, e) => { /* optional immediate behavior */ };
            this.Controls.Add(dtpFrom);

            lblDateTo = new Label();
            lblDateTo.Text = "To:";
            lblDateTo.Location = new Point(530, 80);
            lblDateTo.AutoSize = true;
            this.Controls.Add(lblDateTo);

            dtpTo = new DateTimePicker();
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(560, 75);
            dtpTo.Width = 110;
            dtpTo.Value = DateTime.Today.AddMonths(6); // default "to" range
            dtpTo.ValueChanged += (s, e) => { /* optional immediate behavior */ };
            this.Controls.Add(dtpTo);

            // Search button
            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(680, 73);
            btnSearch.Width = 80;
            UIStyles.ApplyButton(btnSearch);
            btnSearch.Click += (s, e) => RefreshEventsList();
            this.Controls.Add(btnSearch);

            // Clear filters button
            btnClearFilters = new Button();
            btnClearFilters.Text = "Clear";
            btnClearFilters.Location = new Point(680, 103);
            btnClearFilters.Width = 80;
            UIStyles.ApplyButton(btnClearFilters);
            btnClearFilters.Click += (s, e) => ClearFilters();
            this.Controls.Add(btnClearFilters);

            // Events label
            lblEvents = new Label();
            lblEvents.Text = "Events:";
            lblEvents.Location = new Point(20, 120);
            lblEvents.AutoSize = true;
            this.Controls.Add(lblEvents);

            // ListBox showing events 
            lstEvents = new ListBox();
            lstEvents.Location = new Point(20, 145);
            lstEvents.Width = 740;
            lstEvents.Height = 300;
            UIStyles.ApplyListBox(lstEvents);
            this.Controls.Add(lstEvents);


            // Recommendations label
            lblRecommendations = new Label();
            lblRecommendations.Text = "Recommended for You:";
            lblRecommendations.Location = new Point(20, 360);
            lblRecommendations.AutoSize = true;
            this.Controls.Add(lblRecommendations);

            // ListBox showing recommendations
            lstRecommendations = new ListBox();
            lstRecommendations.Location = new Point(20, 385);
            lstRecommendations.Width = 740;
            lstRecommendations.Height = 150;
            UIStyles.ApplyListBox(lstRecommendations);
            this.Controls.Add(lstRecommendations);
        }

        // Adds a new event to the internal dictionary and updates the UI accordingly.
        public void AddEvent(EventModel ev)
        {
            if (ev == null) return;

            // Store the event using its unique ID
            eventsById[ev.Id] = ev;

            // Add its category to the category filter list if it's new
            if (!string.IsNullOrWhiteSpace(ev.Category))
                categories.Add(ev.Category.Trim());

            // Update UI controls
            UpdateCategoryCombo();
            RefreshEventsList();
            RefreshRecommendations();
        }

        // Updates the category dropdown with all available categories.
        private void UpdateCategoryCombo()
        {
            // Preserve selection if possible
            var previous = cmbCategories.SelectedItem?.ToString();

            cmbCategories.Items.Clear();
            cmbCategories.Items.Add("All"); // Default option to show all events

            // Add categories alphabetically
            foreach (var cat in categories.OrderBy(c => c))
                cmbCategories.Items.Add(cat);

            // Restore selection if possible
            if (!string.IsNullOrEmpty(previous) && cmbCategories.Items.Contains(previous))
                cmbCategories.SelectedItem = previous;
            else if (cmbCategories.SelectedIndex < 0)
                cmbCategories.SelectedIndex = 0;
        }

        // Refreshes the main event list based on the selected category and date range.
        private void RefreshEventsList()
        {
            lstEvents.Items.Clear();

            string selectedCategory = cmbCategories.SelectedItem?.ToString();
            DateTime dateFrom = dtpFrom?.Value.Date ?? DateTime.MinValue;
            DateTime dateTo = dtpTo?.Value.Date ?? DateTime.MaxValue;

            // Ensure from <= to
            if (dateTo < dateFrom)
            {
                // swap to keep logical range
                var tmp = dateFrom;
                dateFrom = dateTo;
                dateTo = tmp;
            }

            IEnumerable<EventModel> events = eventsById.Values.OrderBy(ev => ev.StartDate);

            // Filter by selected category (unless "All" is chosen)
            if (!string.IsNullOrWhiteSpace(selectedCategory) && selectedCategory != "All")
                events = events.Where(ev => string.Equals(ev.Category, selectedCategory, StringComparison.OrdinalIgnoreCase));

            // Filter by date range (inclusive)
            events = events.Where(ev => ev.StartDate.Date >= dateFrom && ev.StartDate.Date <= dateTo);

            // Add events to ListBox
            foreach (var ev in events)
                lstEvents.Items.Add($"{ev.StartDate.ToShortDateString()} - {ev.Title} ({ev.Category})");
        }

        // Clears filters and refreshes lists
        private void ClearFilters()
        {
            if (cmbCategories.Items.Contains("All"))
                cmbCategories.SelectedItem = "All";
            else if (cmbCategories.Items.Count > 0)
                cmbCategories.SelectedIndex = 0;

            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today.AddMonths(6);

            RefreshEventsList();
        }

        // Generates event recommendations based on search history.
        private void RefreshRecommendations()
        {
            lstRecommendations.Items.Clear();

            // Get the top 3 most frequently searched categories
            var topCategories = searchCounts.OrderByDescending(kv => kv.Value)
                                            .Take(3)
                                            .Select(kv => kv.Key)
                                            .ToList();

            var recommendedEvents = new List<EventModel>();

            // Collect events that match these categories
            foreach (var cat in topCategories)
            {
                var matches = eventsById.Values.Where(e => string.Equals(e.Category, cat, StringComparison.OrdinalIgnoreCase));
                foreach (var e in matches)
                    if (!recommendedEvents.Contains(e))
                        recommendedEvents.Add(e);
            }

            // If no searches recorded, recommend first 5 upcoming events
            if (!recommendedEvents.Any())
                recommendedEvents = eventsById.Values.OrderBy(ev => ev.StartDate).Take(5).ToList();

            // Display recommendations
            foreach (var ev in recommendedEvents)
                lstRecommendations.Items.Add($"{ev.StartDate.ToShortDateString()} - {ev.Title} ({ev.Category})");
        }

        // Records search queries to track user interests and adjust recommendations.
        public void RecordSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            // Split search terms by spaces, commas, or semicolons
            var tokens = query.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Increment count for each search term
            foreach (var t in tokens)
            {
                if (!searchCounts.ContainsKey(t)) searchCounts[t] = 0;
                searchCounts[t]++;
            }

            // Update recommendations based on new search data
            RefreshRecommendations();
        }

        // Adds pre-defined sample events and mock search data for testing or demo.
        private void AddSampleEvents()
        {
            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Farmers Market",
                Category = "Food",
                StartDate = DateTime.Today.AddDays(2)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Music Concert",
                Category = "Music",
                StartDate = DateTime.Today.AddDays(5)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Art Exhibition",
                Category = "Art",
                StartDate = DateTime.Today.AddDays(3)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Tech Meetup",
                Category = "Technology",
                StartDate = DateTime.Today.AddDays(7)
            });

            // More events
            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Jazz Night at The Loft",
                Category = "Music",
                StartDate = DateTime.Today.AddDays(9)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Food Truck Rally",
                Category = "Food",
                StartDate = DateTime.Today.AddDays(11)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Street Art Fair",
                Category = "Art",
                StartDate = DateTime.Today.AddDays(14)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Community Clean-Up",
                Category = "Community",
                StartDate = DateTime.Today.AddDays(4)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Charity Fun Run",
                Category = "Charity",
                StartDate = DateTime.Today.AddDays(20)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Beginner's Coding Workshop",
                Category = "Workshops",
                StartDate = DateTime.Today.AddDays(16)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Sunday Soccer League",
                Category = "Sports",
                StartDate = DateTime.Today.AddDays(6)
            });

            AddEvent(new EventModel
            {
                Id = Guid.NewGuid(),
                Title = "Virtual Book Club: October",
                Category = "Virtual",
                StartDate = DateTime.Today.AddDays(8)
            });

            // Seed searchCounts so recommendations have something useful to show.
            // (these strings will be split into tokens in RecordSearch)
            RecordSearch("Music Music Music");     // makes Music the top category
            RecordSearch("Food Food");             // Food becomes second
            RecordSearch("Art");                   // Art becomes third
            RecordSearch("Community Tech");        // extra tokens for variety

            // Optionally refresh to ensure UI shows the seeded data immediately
            RefreshEventsList();
            RefreshRecommendations();
        }
    }
}
