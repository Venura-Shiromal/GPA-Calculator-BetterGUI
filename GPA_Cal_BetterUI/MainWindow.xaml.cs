using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GPA_Cal_BetterUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateSemester2Labels("In Development...");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (Semester1Panel == null || Semester2Panel == null) return;

            bool isSemester1 = (sender as TabControl).SelectedIndex == 0;
            Semester1Panel.Visibility = isSemester1 ? Visibility.Visible : Visibility.Collapsed;
            Semester2Panel.Visibility = isSemester1 ? Visibility.Collapsed : Visibility.Visible;

        }

        private void Department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Department.SelectedItem is ComboBoxItem selectedDept)
            {
                string dept = selectedDept.Content.ToString();
                UpdateSemester2Labels(dept);
            }
        }

        private void UpdateSemester2Labels(string department)
        {
            switch (department)
            {
                case "Electrical":
                    Label1_S2.Content = "Methods of Mathematics";
                    Label2_S2.Content = "Computer Systems";
                    Label3_S2.Content = "Theory of Electricity";
                    Label4_S2.Content = "Basic Electronics";
                    Label5_S2.Content = "Manufacturing Processes";
                    Label6_S2.Content = "Communication Skills";
                    Label7_S2.Content = "Language Skills";
                    break;
                case "In Development...":
                    Label1_S2.Content = "Course 1";
                    Label2_S2.Content = "Course 2";
                    Label3_S2.Content = "Course 3";
                    Label4_S2.Content = "Course 4";
                    Label5_S2.Content = "Course 5";
                    Label6_S2.Content = "Course 6";
                    Label7_S2.Content = "Course 7";
                    break;
                default:
                    Label1_S2.Content = "Course 1";
                    Label2_S2.Content = "Course 2";
                    Label3_S2.Content = "Course 3";
                    Label4_S2.Content = "Course 4";
                    Label5_S2.Content = "Course 5";
                    Label6_S2.Content = "Course 6";
                    Label7_S2.Content = "Course 7";
                    break;
            }
        }

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

        private static double GpaCalculator(double[] grades, int[] credits)
        {
            double sum = 0;
            for (int i = 0; i < grades.Length; i++)
                sum += grades[i] * credits[i];
            int totalCredits = credits.Sum();
            return totalCredits > 0 ? sum / totalCredits : 0.0;
        }

        private int[] SemesterCredit()
        {
            int[] credits = null;

            if (Semester1Panel.Visibility == Visibility.Visible)
            {
                credits = new int[] { 3, 3, 2, 2, 2, 2 };
            }
            else
            {
                credits = new int[] { 3, 3, 3, 3, 3, 2, 2 };
            }

            return credits;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int[] credits = SemesterCredit();
            double[] grades = new double[credits.Length];

            if (Semester1Panel.Visibility == Visibility.Visible)
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
                grades[0] = GradeToGradePoint(Grade1_S2.SelectedIndex);
                grades[1] = GradeToGradePoint(Grade2_S2.SelectedIndex);
                grades[2] = GradeToGradePoint(Grade3_S2.SelectedIndex);
                grades[3] = GradeToGradePoint(Grade4_S2.SelectedIndex);
                grades[4] = GradeToGradePoint(Grade5_S2.SelectedIndex);
                grades[5] = GradeToGradePoint(Grade6_S2.SelectedIndex);
                grades[6] = GradeToGradePoint(Grade7_S2.SelectedIndex);
            }

            double gpa = GpaCalculator(grades, credits);
            Result.Content = "GPA = " + gpa.ToString("F2");
        }
    }
}