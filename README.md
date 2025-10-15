# PROG3BPOEP2 - Municipal Services Application

**Author:** Chloe Monique Naidoo  
**Student Number:** ST10145067  
**Course:** PROG7312  
**Project:** Municipal Services Application

## Overview
This is a **Windows Forms C# application** built for a municipal services management system.  
The application provides residents with a way to:

- Report municipal service issues.
- View local events and announcements.
- Check the status of service requests (planned feature).

The app demonstrates **object-oriented programming**, **UI styling with reusable components**, and **data management**.

## Requirements 
Requirements:
-To build and run the project, you will need:
-Windows 10/11 (or any version that supports .NET desktop applications)
-Visual Studio 2019/2022 (Community Edition or higher)
-.NET Framework 4.7.2 (or later)

## How to Compile:
- Clone or download the repository: git clone https://github.com/ChloeNaidoo/PROG7312-POEP2.git
- Open the solution file PROG3BPOEP1.sln in Visual Studio.
- Set PROG3BPOEP1 as the startup project.
- Build the solution by pressing Ctrl+Shift+B or going to: Build > Build Solution.

## How to Run:
- Press F5 (Run with Debug) or Ctrl+F5 (Run without Debugging).
- The application will open on the Main Menu (if implemented). From here, you can:
1.	Report a new issue
2.	View events announcements 
3.	Service request Status

## Features

### 1. **Main Menu**
- Central for navigation.
- Buttons for:
  - **Report Issues**
  - **Local Events & Announcements**
  - **Service Request Status** (placeholder)

### 2. **Report Issue Form**
1. Using the Application (Part 1):
- Reporting an Issue
- Enter the location of the problem.
- Select a category (e.g., Sanitation, Roads, Utilities, Parks, Public Safety, or Other).
- Provide a brief description.
- (Optional) Upload images or documents to support your report.
- Watch the progress bar as you complete each section.
- Click Submit to send the issue.
2. Rating Your Experience
- After submitting an issue, the Rate Your Experience form will appear.
- Choose a rating 
- Click Submit Feedback.
- You will receive a thank you message and be taken back to the Main Menu.

### 3. **Events Form**
- Displays upcoming **local events**.
- Category filtering and date range filtering.
- **Recommendations section** based on search history.
- Includes sample events and seeded searches for demo purposes.

### 4. **UI Styling**
- Consistent visual theme via `UIStyles` class.
- Reusable styling for:
  - Buttons (primary, secondary)
  - Labels, headers, list boxes, text boxes
  - Date pickers, combo boxes
- Hover effects and color theming applied to buttons.

//This application was developed as part of the PROG3B Portfolio of Evidence.


