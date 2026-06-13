using System;
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

                switch (dept)
                {
                    case "Electrical":
                        Label1_S2.Content = "Mathematics";
                        Label2_S2.Content = "Programming";
                        Label3_S2.Content = "Electrical";
                        Label4_S2.Content = "Material";
                        Label5_S2.Content = "Fluid";
                        Label6_S2.Content = "Mechanics";
                        break;
                    case "In Development...":
                        Label1_S2.Content = "In Development...";
                        Label2_S2.Content = "In Development...";
                        Label3_S2.Content = "In Development...";
                        Label4_S2.Content = "In Development...";
                        Label5_S2.Content = "In Development...";
                        Label6_S2.Content = "In Development...";
                        break;
                    default:
                        Label1_S2.Content = "Mathematics";
                        Label2_S2.Content = "Programming";
                        Label3_S2.Content = "Electrical";
                        Label4_S2.Content = "Material";
                        Label5_S2.Content = "Fluid";
                        Label6_S2.Content = "Mechanics";
                        break;
                }
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

        private static int CreditSum(double maths, double cs, double elec, double mech, double fluid, double mat)
        {
            int mathsCP = 3, csCP = 3, elecCP = 2, mechCP = 2, fluidCP = 2, matCP = 2;
            int total = 0;
            if (maths != 0) total += mathsCP;
            if (cs != 0) total += csCP;
            if (elec != 0) total += elecCP;
            if (mech != 0) total += mechCP;
            if (fluid != 0) total += fluidCP;
            if (mat != 0) total += matCP;
            return total;
        }

        private static double GpaCalculator(double maths, double cs, double elec, double mech, double fluid, double mat)
        {
            int mathsCP = 3, csCP = 3, elecCP = 2, mechCP = 2, fluidCP = 2, matCP = 2;
            double sum = maths * mathsCP + cs * csCP + elec * elecCP + mech * mechCP + fluid * fluidCP + mat * matCP;
            int totalCp = CreditSum(maths, cs, elec, mech, fluid, mat);
            return totalCp > 0 ? sum / totalCp : 0.0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double maths, cs, elec, mech, fluid, mat;

            if (Semester1Panel.Visibility == Visibility.Visible)
            {
                maths = GradeToGradePoint(Grade_Maths.SelectedIndex);
                cs = GradeToGradePoint(Grade_CS.SelectedIndex);
                elec = GradeToGradePoint(Grade_Elec.SelectedIndex);
                mech = GradeToGradePoint(Grade_Mech.SelectedIndex);
                fluid = GradeToGradePoint(Grade_Fluid.SelectedIndex);
                mat = GradeToGradePoint(Grade_Mat.SelectedIndex);
            }
            else
            {
                maths = GradeToGradePoint(Grade_Maths_S2.SelectedIndex);
                cs = GradeToGradePoint(Grade_CS_S2.SelectedIndex);
                elec = GradeToGradePoint(Grade_Elec_S2.SelectedIndex);
                mech = GradeToGradePoint(Grade_Mech_S2.SelectedIndex);
                fluid = GradeToGradePoint(Grade_Fluid_S2.SelectedIndex);
                mat = GradeToGradePoint(Grade_Mat_S2.SelectedIndex);
            }

            double gpa = GpaCalculator(maths, cs, elec, mech, fluid, mat);
            Result.Content = "GPA = " + gpa.ToString("F2");
        }
    }
}