using System;
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

        public MainWindow()
        {
            InitializeComponent();
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
            //if (Semester1Panel == null || Semester2Panel == null) return;

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

        // Department panel control
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

        // Credits array selector
        private int[] SemesterCredit()
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
                    credits = new int[] { 3, 3, 3, 3, 2, 2 }; // Just for now
                } 
            }

            return credits;
        }

        // Calculate button clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int[] credits = SemesterCredit();
            double[] grades = new double[credits.Length];

            // Reading the inputs
            if (semester == 1)
            {
                grades[0] = GradeToGradePoint(Grade1_S1.SelectedIndex);
                grades[1] = GradeToGradePoint(Grade2_S1.SelectedIndex);
                grades[2] = GradeToGradePoint(Grade3_S1.SelectedIndex);
                grades[3] = GradeToGradePoint(Grade4_S1.SelectedIndex);
                grades[4] = GradeToGradePoint(Grade5_S1.SelectedIndex);
                grades[5] = GradeToGradePoint(Grade6_S1.SelectedIndex);
            }
            else 
            {
                if (semester == 2 && department == "Electrical")
                {
                    grades[0] = GradeToGradePoint(Grade1_S2_EE.SelectedIndex);
                    grades[1] = GradeToGradePoint(Grade2_S2_EE.SelectedIndex);
                    grades[2] = GradeToGradePoint(Grade3_S2_EE.SelectedIndex);
                    grades[3] = GradeToGradePoint(Grade4_S2_EE.SelectedIndex);
                    grades[4] = GradeToGradePoint(Grade5_S2_EE.SelectedIndex);
                    grades[5] = GradeToGradePoint(Grade6_S2_EE.SelectedIndex);
                    grades[6] = GradeToGradePoint(Grade7_S2_EE.SelectedIndex);
                }
                else if (semester == 2 && department == "Computer Science")
                {
                    grades[0] = GradeToGradePoint(Grade1_S2_CS.SelectedIndex);
                    grades[1] = GradeToGradePoint(Grade2_S2_CS.SelectedIndex);
                    grades[2] = GradeToGradePoint(Grade3_S2_CS.SelectedIndex);
                    grades[3] = GradeToGradePoint(Grade4_S2_CS.SelectedIndex);
                    grades[4] = GradeToGradePoint(Grade5_S2_CS.SelectedIndex);
                    grades[5] = GradeToGradePoint(Grade6_S2_CS.SelectedIndex);
                }
            }

            double gpa = GpaCalculator(grades, credits); // Calculation
            Result.Content = "GPA = " + gpa.ToString("F2"); // Output
        }
    }
}