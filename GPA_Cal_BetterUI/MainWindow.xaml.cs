using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPA_Cal_BetterUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static double Grade_to_GradePoint(int Grade)
        {
            if ((Grade == 0) | (Grade == 1))
            {
                return 4.00;
            }
            else if (Grade == 2)
            {
                return 3.70;
            }
            else if (Grade == 3)
            {
                return 3.30;
            }
            else if (Grade == 4)
            {
                return 3.00;
            }
            else if (Grade == 5)
            {
                return 2.70;
            }
            else if (Grade == 6)
            {
                return 2.30;
            }
            else if (Grade == 7)
            {
                return 2.00;
            }
            else if (Grade == 8)
            {
                return 1.70;
            }
            else if (Grade == 9)
            {
                return 1.00;
            }
            else
            {
                return 0.00;
            }
        }

        private static int Credit_Sum(double Maths, double CS, double Elec, double Mech, double Fluid, double Mat)
        {
            int Sum = 0;
            int Maths_CP = 3, CS_CP = 3, Elec_CP = 2, Mech_CP = 2, Fluid_CP = 2, Mat_CP = 2;

            if (Maths != 0)
            {
                Sum += Maths_CP;
            }

            if (CS != 0)
            {
                Sum += CS_CP;
            }

            if (Elec != 0)
            {
                Sum += Elec_CP;
            }

            if (Mech != 0)
            {
                Sum += Mech_CP;
            }

            if (Fluid != 0)
            {
                Sum += Fluid_CP;
            }

            if (Mat != 0)
            {
                Sum += Mat_CP;
            }

            return Sum;
        }

        private static double GPA_Calculator(double Maths, double CS, double Elec, double Mech, double Fluid, double Mat)
        {
            // Credit Points for each module
            int Maths_CP = 3, CS_CP = 3, Elec_CP = 2, Mech_CP = 2, Fluid_CP = 2, Mat_CP = 2;

            double Sum = Maths * Maths_CP + CS * CS_CP + Elec * Elec_CP + Mech * Mech_CP + Fluid * Fluid_CP + Mat * Mat_CP;

            int Total_CP = Credit_Sum(Maths, CS, Elec, Mech, Fluid, Mat);

            double GPA = Sum / Total_CP;

            return GPA;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Extracting all the Grade Points from Grades

            double Maths, CS, Elec, Mech, Fluid, Mat;

            if (Grade_Maths.SelectedIndex != -1)
            {
                Maths = Grade_to_GradePoint(Grade_Maths.SelectedIndex);
            }
            else
            {
                Maths = 0;
            }

            if (Grade_CS.SelectedIndex != -1)
            {
                CS = Grade_to_GradePoint(Grade_CS.SelectedIndex);
            }
            else
            {
                CS = 0;
            }

            if (Grade_Elec.SelectedIndex != -1)
            {
                Elec = Grade_to_GradePoint(Grade_Elec.SelectedIndex);
            }
            else
            {
                Elec = 0;
            }

            if (Grade_Mech.SelectedIndex != -1)
            {
                Mech = Grade_to_GradePoint(Grade_Mech.SelectedIndex);
            }
            else
            {
                Mech = 0;
            }

            if (Grade_Fluid.SelectedIndex != -1)
            {
                Fluid = Grade_to_GradePoint(Grade_Fluid.SelectedIndex);
            }
            else
            {
                Fluid = 0;
            }

            if (Grade_Mat.SelectedIndex != -1)
            {
                Mat = Grade_to_GradePoint(Grade_Mat.SelectedIndex);
            }
            else
            {
                Mat = 0;
            }

            //Calculating GPA
            string GPA = GPA_Calculator(Maths, CS, Elec, Mech, Fluid, Mat).ToString("F2");

            Result.Content = "GPA = " + GPA;

            return;
        }
    }
}
