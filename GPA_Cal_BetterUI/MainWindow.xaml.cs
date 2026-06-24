using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GPA_Cal_BetterUI
{
    public partial class MainWindow : Window
    {
        // Default global variables
        string department = "Electrical";
        int semester = 1;

        private List<(ComboBox electiveCombo, ComboBox gradeCombo)> electivePairs = new List<(ComboBox, ComboBox)>();
        private const int MIN_ELECTIVES = 0;
        private const int MAX_ELECTIVES = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Add elective row
        private void AddElectiveRow()
        {
            if (electivePairs.Count >= MAX_ELECTIVES)
            {
                return;
            }

            // Create a horizontal container for the pair
            StackPanel rowPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Height = 36,
                Margin = new Thickness(0, 0, 0, 0)
            };

            // Create elective ComboBox
            ComboBox electiveCombo = new ComboBox
            {
                Style = (Style)FindResource("RoundedComboBox"),
                Width = 270,
                Height = 26,
                SelectedIndex = -1,
                FontSize = 14,
                BorderBrush = null,
                Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2C2835"),
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("White"),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            // Add new electives here (only module name)
            string[] electives =
            {
                "Visual Programming",
                "Computer Systems",
                "Introduction to Telecommunications",
                "Basic Electronics",
                "Manufacturing Processes",
                "Entrepreneurship Theory",
                "Humanities I"
            };

            foreach (string elective in electives)
                electiveCombo.Items.Add(elective);

            // Create grade ComboBox
            ComboBox gradeCombo = new ComboBox
            {
                Style = (Style)FindResource("RoundedComboBox"),
                Width = 58,
                Height = 26,
                SelectedIndex = -1,
                FontSize = 14,
                BorderBrush = null,
                Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2C2835"),
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("White"),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(54, 0, 0, 0)
            };

            string[] grades = { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D", "F" };
            foreach (string grade in grades)
                gradeCombo.Items.Add(grade);

            // Add both to the row panel
            rowPanel.Children.Add(electiveCombo);
            rowPanel.Children.Add(gradeCombo);

            // Add row to container
            ElectiveRowsContainer.Children.Add(rowPanel);
            electivePairs.Add((electiveCombo, gradeCombo));

            // Scroll to bottom after adding new row
            if (ElectiveScrollViewer != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ElectiveScrollViewer.ScrollToEnd();
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }

        // Remove last elective row
        private void RemoveElectiveRow()
        {
            if (electivePairs.Count <= MIN_ELECTIVES)
            {
                return;
            }

            var lastPair = electivePairs.Last();
            var rowToRemove = ElectiveRowsContainer.Children[ElectiveRowsContainer.Children.Count - 1] as StackPanel;
            ElectiveRowsContainer.Children.Remove(rowToRemove);
            electivePairs.RemoveAt(electivePairs.Count - 1);
        }

        // Add elective button click
        private void AddElective_Click(object sender, RoutedEventArgs e)
        {
            AddElectiveRow();
        }

        // Remove elective button click
        private void RemoveElective_Click(object sender, RoutedEventArgs e)
        {
            RemoveElectiveRow();
        }

        // Dragging mechanism
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        // Closing mechanism
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Switching tabs
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tabControl = (sender as TabControl).SelectedIndex;

            if (tabControl == 0)
            {
                semester = 1;
                Semester1Panel.Visibility = Visibility.Visible;
                Semester2Panel.Visibility = Visibility.Collapsed;
            } 
            else
            {
                semester = 2;
                Semester1Panel.Visibility = Visibility.Collapsed;
                Semester2Panel.Visibility = Visibility.Visible;
            }
        }

        // Switching departments
        private void Department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Department.SelectedItem is ComboBoxItem selectedDept)
            {
                string dept = selectedDept.Content.ToString();
                department = dept;
                DepartmentSwitcher(dept);
            }
        }

        // Department panel control (for adding new departments)
        private void DepartmentSwitcher(string department)
        {
            switch (department) 
            {
                case "Electrical":
                    Semester1Panel.Visibility = Visibility.Collapsed;
                    Semester2Panel.Visibility = Visibility.Visible;
                    Electrical_Panel.Visibility = Visibility.Visible;
                    CS_Panel.Visibility = Visibility.Collapsed;
                    break;

                case "Computer Science":
                    Semester1Panel.Visibility = Visibility.Collapsed;
                    Semester2Panel.Visibility = Visibility.Visible;
                    Electrical_Panel.Visibility = Visibility.Collapsed;
                    CS_Panel.Visibility = Visibility.Visible;
                    break;

                default:
                    Semester1Panel.Visibility = Visibility.Collapsed;
                    Semester2Panel.Visibility = Visibility.Visible;
                    Electrical_Panel.Visibility = Visibility.Visible;
                    CS_Panel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        // Grade to grade point converter
        private static double GradeToGradePoint(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: return 4.00;  // A+
                case 1: return 4.00;  // A
                case 2: return 3.70;  // A-
                case 3: return 3.30;  // B+
                case 4: return 3.00;  // B
                case 5: return 2.70;  // B-
                case 6: return 2.30;  // C+
                case 7: return 2.00;  // C
                case 8: return 1.70;  // C-
                case 9: return 1.00;  // D
                case 10: return 0.00; // F
                default: return 0.00;
            }
        }

        // GPA calculator logic
        private static double GpaCalculator(double[] grades, int[] credits)
        {
            double sum = 0;
            for (int i = 0; i < grades.Length; i++)
                sum += grades[i] * credits[i];
            int totalCredits = credits.Sum();
            return totalCredits > 0 ? sum / totalCredits : 0.0;
        }

        // Credits array selector (for adding new semester or department course credits)
        private int[] CompulsaryCredit()
        {
            int[] credits = null;

            if (semester == 1)
            {
                credits = new int[] { 3, 3, 2, 2, 2, 2 };
            }
            else
            {
                if (department == "Electrical") 
                {
                    credits = new int[] { 3, 3, 3, 3, 3, 2, 2 };
                } 
                else if (department == "Computer Science")
                {
                    credits = new int[] { 3, 3, 3, 3, 2, 2 };
                } 
            }

            return credits;
        }

        // Add new electives here (only module credits)
        private int GetElectiveCredit(int electiveIndex)
        {
            switch (electiveIndex)
            {
                case 0: case 2: case 6:
                    return 2;  // Visual Programming, Introduction to Telecommunications, Humanities I 
                case 1: case 3: case 4: case 5:
                    return 3;  // Computer Systems, Basic Electronics, Manufacturing Processes, Entrepreneurship Theory
                default: 
                    return 0;
            }
        }

        // Get selected electives with grades and credits
        private (List<double> grades, List<int> credits) GetSelectedElectives()
        {
            var electiveGrades = new List<double>();
            var electiveCredits = new List<int>();

            foreach (var (electiveCombo, gradeCombo) in electivePairs)
            {
                if (electiveCombo.SelectedIndex != -1 && gradeCombo.SelectedIndex != -1)
                {
                    electiveGrades.Add(GradeToGradePoint(gradeCombo.SelectedIndex));
                    electiveCredits.Add(GetElectiveCredit(electiveCombo.SelectedIndex));
                }
            }

            return (electiveGrades, electiveCredits);
        }

        // Calculate button clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var credits = new List<int>();
            var grades = new List<double>();

            // Reading the inputs (for adding new departments and semesters)
            if (semester == 1)
            {
                credits.AddRange(CompulsaryCredit());
                grades.Add(GradeToGradePoint(Grade1_S1.SelectedIndex));
                grades.Add(GradeToGradePoint(Grade2_S1.SelectedIndex));
                grades.Add(GradeToGradePoint(Grade3_S1.SelectedIndex));
                grades.Add(GradeToGradePoint(Grade4_S1.SelectedIndex));
                grades.Add(GradeToGradePoint(Grade5_S1.SelectedIndex));
                grades.Add(GradeToGradePoint(Grade6_S1.SelectedIndex));
            }
            else if (semester == 2)
            {
                if (department == "Electrical")
                {
                    credits.AddRange(CompulsaryCredit());
                    grades.Add(GradeToGradePoint(Grade1_S2_EE.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade2_S2_EE.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade3_S2_EE.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade4_S2_EE.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade5_S2_EE.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade6_S2_EE.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade7_S2_EE.SelectedIndex));
                }
                else if (department == "Computer Science")
                {
                    credits.AddRange(CompulsaryCredit());
                    grades.Add(GradeToGradePoint(Grade1_S2_CS.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade2_S2_CS.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade3_S2_CS.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade4_S2_CS.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade5_S2_CS.SelectedIndex));
                    grades.Add(GradeToGradePoint(Grade6_S2_CS.SelectedIndex));
                }
                var electives = GetSelectedElectives();
                grades.AddRange(electives.grades);
                credits.AddRange(electives.credits);
            }

            double gpa = GpaCalculator(grades.ToArray(), credits.ToArray());
            Result.Content = "GPA = " + gpa.ToString("F2");
        }
    }
}