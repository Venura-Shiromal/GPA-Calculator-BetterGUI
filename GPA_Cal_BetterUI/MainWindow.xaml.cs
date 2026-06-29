using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GPA_Cal_BetterUI
{
    public partial class MainWindow : Window
    {
        // Default global variables
        string[] departments = {
                "Electrical",
                "Electronic",
                "Biomedical",
                "Computer Science",
                "Mechanical",
                "Chemical",
                "Material",
                "Civil",
                "Textile",
                "Transport",
                "Earth Resource"
        };

        string department_S1 = "General";
        string department = "Electrical";
        string stream = "General";
        int semester = 1;

        private bool _isRemoving = false;
        private List<ComboBox> gradeComboList = new List<ComboBox>();
        private List<string> moduleList = new List<string>();
        private List<(ComboBox electiveCombo, ComboBox gradeCombo)> electivePairs = new List<(ComboBox, ComboBox)>();
        private List<(TextBox creditBox, ComboBox gradeCombo)> modulePairs = new List<(TextBox, ComboBox)>();
        private const int MAX_ELECTIVES = 10;
        private const int MAX_MODULES = 20;

        // Initialization
        public MainWindow()
        {
            InitializeComponent();
            SemSwitcher(semester);
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

        // Animation - Remove Element
        private void AnimateRemoveElement(FrameworkElement element, Panel parent, Action onComplete = null)
        {
            if (element == null || !parent.Children.Contains(element)) return;

            if (!(element.RenderTransform is TranslateTransform))
                element.RenderTransform = new TranslateTransform();

            var storyboard = new Storyboard();

            var fade = new DoubleAnimation { To = 0, Duration = TimeSpan.FromSeconds(0.25) };
            Storyboard.SetTarget(fade, element);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

            var slide = new DoubleAnimation { To = 10, Duration = TimeSpan.FromSeconds(0.25) };
            Storyboard.SetTarget(slide, element);
            Storyboard.SetTargetProperty(slide, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            storyboard.Children.Add(fade);
            storyboard.Children.Add(slide);

            storyboard.Completed += (s, e) =>
            {
                parent.Children.Remove(element);
                onComplete?.Invoke();
            };
            storyboard.Begin();
        }

        // Animation - Show Element
        private void AnimateShowElement(FrameworkElement element)
        {
            if (element == null) return;

            // Ensure it's visible and has a transform
            element.Visibility = Visibility.Visible;
            if (!(element.RenderTransform is TranslateTransform))
                element.RenderTransform = new TranslateTransform();
            element.Opacity = 0;
            ((TranslateTransform)element.RenderTransform).Y = -10; // slide from above

            var storyboard = new Storyboard();

            var fade = new DoubleAnimation { To = 1, Duration = TimeSpan.FromSeconds(0.25) };
            fade.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(fade, element);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

            var slide = new DoubleAnimation { To = 0, Duration = TimeSpan.FromSeconds(0.25) };
            slide.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(slide, element);
            Storyboard.SetTargetProperty(slide, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            storyboard.Children.Add(fade);
            storyboard.Children.Add(slide);

            storyboard.Begin();
        }

        // Animation - Hide Element
        private void AnimateHideElement(FrameworkElement element, Action onComplete = null)
        {
            if (element == null) return;
            if (!(element.RenderTransform is TranslateTransform))
                element.RenderTransform = new TranslateTransform();

            var storyboard = new Storyboard();

            var fade = new DoubleAnimation { To = 0, Duration = TimeSpan.FromSeconds(0.25) };
            fade.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(fade, element);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

            var slide = new DoubleAnimation { To = -10, Duration = TimeSpan.FromSeconds(0.25) }; // slide up
            slide.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(slide, element);
            Storyboard.SetTargetProperty(slide, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            storyboard.Children.Add(fade);
            storyboard.Children.Add(slide);

            storyboard.Completed += (s, e) =>
            {
                element.Visibility = Visibility.Collapsed;
                onComplete?.Invoke();
            };
            storyboard.Begin();
        }

        // Add elective row
        private void AddElectiveRow()
        {
            if (electivePairs.Count < MAX_ELECTIVES)
            {
                // Create a horizontal container for the pair
                StackPanel rowPanel = new StackPanel
                {
                    Style = (Style)this.Resources["AnimatedRowStyle"],
                    Orientation = Orientation.Horizontal,
                    Height = 36,
                    Margin = new Thickness(0, 0, 0, 0)
                };

                // Create elective ComboBox
                ComboBox electiveCombo = new ComboBox
                {
                    Style = (Style)FindResource("RoundedComboBox"),
                    Width = 340,
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

                List<string> electives = new List<string>();

                if (semester == 2)
                {
                    electives = new List<string>
                        {
                            "Visual Programming",
                            "Computer Systems",
                            "Introduction to Telecommunications Engineering",
                            "Basic Electronics for Engineering Applications",
                            "Introduction to Manufacturing Processes",
                            "Entrepreneurship Theory",
                            "Humanities I"
                        };
                }

                foreach (string elective in electives)
                    electiveCombo.Items.Add(elective);

                // Create grade ComboBox
                ComboBox gradeCombo = CreateGradeCombo(54);

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
        }

        // Remove last elective row 
        private void RemoveElectiveRow()
        {
            // Prevent overlapping removals
            if (_isRemoving || electivePairs.Count == 0) return;
            _isRemoving = true;

            // Get the last visual row
            var lastRow = ElectiveRowsContainer.Children[ElectiveRowsContainer.Children.Count - 1] as FrameworkElement;
            if (lastRow == null)
            {
                _isRemoving = false;
                return;
            }

            // Remove the last pair from the list immediately
            electivePairs.RemoveAt(electivePairs.Count - 1);

            // Animate the visual removal – after animation, clear the flag
            AnimateRemoveElement(lastRow, ElectiveRowsContainer, () => { _isRemoving = false; });
        }

        /* Remove last elective row (old - no animations)
        private void RemoveElectiveRow()
        {
            if (electivePairs.Count > 0)
            {
                var lastPair = electivePairs.Last();
                var rowToRemove = ElectiveRowsContainer.Children[ElectiveRowsContainer.Children.Count - 1] as StackPanel;
                ElectiveRowsContainer.Children.Remove(rowToRemove);
                electivePairs.RemoveAt(electivePairs.Count - 1);
            }
        }*/

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

        // Add module row
        private void AddModuleRow()
        {
            HeaderRow.Visibility = Visibility.Visible;

            if (modulePairs.Count < MAX_MODULES)
            {
                // Create a horizontal container for the pair
                StackPanel rowPanel = new StackPanel
                {
                    Style = (Style)this.Resources["AnimatedRowStyle"],
                    Orientation = Orientation.Horizontal,
                    Height = 36,
                    Margin = new Thickness(0, 0, 0, 0)
                };

                // Create module TextBox
                TextBox moduleBox = new TextBox
                {
                    Style = (Style)FindResource("RoundedTextBox"),
                    Width = 267,
                    Height = 26,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 14
                };

                TextBox creditBox = new TextBox
                {
                    Style = (Style)FindResource("RoundedTextBox"),
                    Width = 56,
                    Height = 26,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 14,
                    Margin = new Thickness( 36, 0, 0, 0 )
                };

                // Create grade ComboBox
                ComboBox gradeCombo = CreateGradeCombo(36);

                // Add them to the row panel
                rowPanel.Children.Add(moduleBox);
                rowPanel.Children.Add(creditBox);
                rowPanel.Children.Add(gradeCombo);

                // Add row to container
                ModuleRowsContainer.Children.Add(rowPanel);
                modulePairs.Add((creditBox, gradeCombo));

                if (modulePairs.Count == 1) AnimateShowElement(HeaderRow);
            }
        }

        private void RemoveModuleRow()
        {
            if (_isRemoving || modulePairs.Count == 0) return;
            _isRemoving = true;

            var lastRow = ModuleRowsContainer.Children[ModuleRowsContainer.Children.Count - 1] as FrameworkElement;
            if (lastRow == null)
            {
                _isRemoving = false;
                return;
            }

            modulePairs.RemoveAt(modulePairs.Count - 1);

            if (modulePairs.Count == 0)
            {
                // No modules left: animate header out and hide it
                AnimateHideElement(HeaderRow, () => { _isRemoving = false; });
                // Also remove the row visually (no callback needed – flag already handled by header)
                AnimateRemoveElement(lastRow, ModuleRowsContainer);
            }
            else
            {
                // Still rows left: clear flag after row removal
                AnimateRemoveElement(lastRow, ModuleRowsContainer, () => { _isRemoving = false; });
            }
        }

        /* Remove last elective row
        private void RemoveModuleRow()
        {
            if (modulePairs.Count > 0)
            {
                var lastPair = modulePairs.Last();
                var rowToRemove = ModuleRowsContainer.Children[ModuleRowsContainer.Children.Count - 1] as StackPanel;
                ModuleRowsContainer.Children.Remove(rowToRemove);
                modulePairs.RemoveAt(modulePairs.Count - 1);
            }

            if (modulePairs.Count == 0)
            {
                HeaderRow.Visibility = Visibility.Collapsed;
            }
        }*/

        // Add module button click
        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
            AddModuleRow();
        }

        // Remove module button click
        private void RemoveModule_Click(object sender, RoutedEventArgs e)
        {
            RemoveModuleRow();
        }

        // Create grade ComboBox
        private ComboBox CreateGradeCombo(int gap)
        {
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
                Margin = new Thickness(gap, 0, 0, 0)
            };

            string[] grades = { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D", "F" };
            foreach (string grade in grades)
                gradeCombo.Items.Add(grade);

            return gradeCombo;
        }

        // Add grade ComboBox
        private void AddGradeCombo(int h, int b)
        {
            Border GradeBorder = new Border
            {
                Height = h,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, b, 0, 0)
            };

            ComboBox gradeCombo = CreateGradeCombo(0);

            gradeComboList.Add(gradeCombo);

            GradeBorder.Child = gradeCombo;
            Grade_Panel.Children.Add(GradeBorder);
        }

        // Dynamically updates ComboBoxes
        private void AddComboBox_Dep(int h, int b)
        {
            RemoveGradeCombo();
            AddGradeCombo(h, b);
            for (int i = 0; i < moduleList.Count; i++) AddGradeCombo(h, 0);
        }

        // Remove grading ComboBox
        private void RemoveGradeCombo()
        {
            int N = Grade_Panel.Children.Count;

            for (int i = 0; i < N; i++)
            {
                int n = Grade_Panel.Children.Count;
                var boxToRemove = Grade_Panel.Children[n - 1];
                Grade_Panel.Children.Remove(boxToRemove);
            }

            gradeComboList.Clear();
        }

        // Create module Labels
        private void CreateModuleLabel(int h, int b, string name)
        {
            Border LabelBorder = new Border
            {
                Height = h,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, b, 0, 0)
            };

            Label module = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("White"),
                FontSize = 14,
                Width = 350,
                Height = 29,
                Margin = new Thickness(10, 0, 0, 0),
                Content = name
            };

            LabelBorder.Child = module;
            Labels_Panel.Children.Add(LabelBorder);
        }

        // Add module Labels
        private void AddModuleLabels()
        {
            var moduleLabels = new List<string>();
            string moduleFirst = "";
            int h = 46; int b = 29;

            if (semester == 1)
            {
                h = 46; b = 29;
                moduleFirst = "Mathematics";
                moduleLabels = new List<string> {
                        "Programming Fundamentals",
                        "Electrical Fundamentals",
                        "Properties of Materials",
                        "Mechanics",
                        "Fluid Mechanics"
                    };

                if (department_S1 == departments[8])
                {
                    moduleLabels.Add("Fibre Science");
                }
                else if (department_S1 == departments[9])
                {
                    moduleLabels.Remove("Mechanics");
                    moduleLabels.Remove("Properties of Materials");
                    moduleLabels.Remove("Fluid Mechanics");
                    moduleLabels.Add("Mechanics in Transport");
                    moduleLabels.Add("Fundamentals of Transport and Logistics Systems");
                    moduleLabels.Add("Fluid Mechanics");
                }
            }
            else if (semester == 2)
            {
                h = 34; b = 18;
                moduleFirst = "Methods of Mathematics";

                if (department == departments[0])
                {
                    moduleLabels = new List<string> {
                    "Theory of Electricity",
                    "Computer Systems",
                    "Basic Electronics for Engineering Applications",
                    "Introduction to Manufacturing Processes",
                    "Communication Skills",
                    "Language Skills"
                    };
                }
                else if (department == departments[1])
                {
                    moduleLabels = new List<string> {
                    "Electronic Engineering",
                    "Introduction to Telecommunications Engineering",
                    "Circuits, Signals, and Systems",
                    "Laboratory Practice",
                    "Communication Skills",
                    "Language Skills",
                    "Engineering Design Project"
                    };
                }
                else if (department == departments[2])
                {
                    moduleLabels = new List<string> {
                    "Electronic Engineering",
                    "Introduction to Telecommunications Engineering",
                    "Circuits, Signals, and Systems",
                    "Laboratory Practice",
                    "Communication Skills",
                    "Language Skills",
                    "Engineering Design Project"
                    };
                }
                else if (department == departments[3])
                {
                    moduleLabels = new List<string> {
                    "Theory of Electricity",
                    "Data Structures and Algorithms",
                    "Program Construction",
                    "Computer Organization and Digital Design",
                    "Language Skills"
                    };
                }
                else if (department == departments[4])
                {
                    moduleLabels = new List<string> {
                    "Mechanics of Materials I",
                    "Manufacturing Technology",
                    "Engineering Graphics and Machine Drawing",
                    "Fundamentals of Engineering Thermodynamics",
                    "Fundamentals of Mechatronics",
                    "Engineering Materials",
                    "Language Skills"
                    };

                    if (stream == "Mechatronic")
                    {
                        moduleLabels[moduleLabels.IndexOf("Fundamentals of Mechatronics")] = "Mechatronic Systems Engineering";
                    }
                }
                else if (department == departments[5])
                {
                    moduleLabels = new List<string> {
                    "Engineering Thermodynamics",
                    "Fluid Dynamics",
                    "Chemistry and Green Chemistry for Process Engineers",
                    "Chemical and Bioprocess Engineering Principles",
                    "Language Skills"
                    };
                }
                else if (department == departments[6])
                {
                    moduleLabels = new List<string> {
                    "Visual Programming",
                    "Basic Electronics for Engineering Applications",
                    "Engineering Drawing and Computer Aided Modelling",
                    "Thermodynamics and Phase Equilibria",
                    "Fundamentals of Materials Science and Engineering",
                    "Language Skills"
                    };
                }
                else if (department == departments[7])
                {
                    moduleLabels = new List<string> {
                    "Structural Mechanics I",
                    "Fluid Dynamics",
                    "Building Construction and Materials",
                    "Language Skills"
                    };
                }
                else if (department == departments[8])
                {
                    moduleLabels = new List<string> {
                    "Visual Programming",
                    "Basic Electronics Engineering Applications",
                    "Introduction to Textile and Apparel Industry",
                    "Textile Chemistry",
                    "Pattern Technology and Construction I",
                    "Engineering Materials",
                    "Principles of Textile Machinery & Instrumentation",
                    "Language Skills"
                    };
                }
                else if (department == departments[9])
                {
                    moduleLabels = new List<string> {
                    "Visual Programming",
                    "Basic Electronics for Engineering Applications",
                    "Operations Research for Transport and Logistics Systems",
                    "Geo-Spatial Analysis",
                    "Systems Engineering",
                    "Language Skills"
                    };
                }
                else if (department == departments[10])
                {
                    moduleLabels = new List<string> {
                    "Geology",
                    "Introduction to Mining & Mineral Engineering",
                    "Basic Mine Thermodynamics",
                    "Engineering Drawing & Computer Aided Modelling",
                    "Humanities I",
                    "Language Skills"
                    };
                }
            }

            RemoveModuleLabels();
            CreateModuleLabel(h, b, moduleFirst);
            foreach (string module in moduleLabels)
            {
                CreateModuleLabel(h, 0, module);
            }

            moduleList = moduleLabels;
        }

        // Remove module Labels
        private void RemoveModuleLabels()
        {
            int N = Labels_Panel.Children.Count;

            for (int i = 0; i < N; i++)
            {
                int n = Labels_Panel.Children.Count;
                var boxToRemove = Labels_Panel.Children[n - 1];
                Labels_Panel.Children.Remove(boxToRemove);
            }
        }

        // Reset Departments
        private void ResetDepartments(int sem)
        {
            if (sem == 1)
            {
                Department.SelectedIndex = 0;
                department = "Electrical";
            }
            else if (sem == 2)
            {
                Department_Sem1.SelectedIndex = 0;
                department_S1 = "General";
            }
        }

        // Switching semesters
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tabControl = (sender as TabControl).SelectedIndex;
            if (tabControl == 0) semester = 1;
            if (tabControl == 1) semester = 2;
            if (tabControl == 2) semester = 0;
            SemSwitcher(semester);
            ResetDepartments(semester);
        }

        // Add new semesters here
        private void SemSwitcher(int sem)
        {
            Result.Content = "";

            if (sem == 0)
            {
                CompulsaryPanel.Visibility = Visibility.Collapsed;
                Semester1Panel.Visibility = Visibility.Collapsed;
                Semester2Panel.Visibility = Visibility.Collapsed;
                GeneralPanel.Visibility = Visibility.Visible;
            }
            else if (sem == 1)
            {
                CompulsaryPanel.Visibility = Visibility.Visible;
                Semester1Panel.Visibility = Visibility.Visible;
                Semester2Panel.Visibility = Visibility.Collapsed;
                GeneralPanel.Visibility = Visibility.Collapsed;
                AddModuleLabels();
                AddComboBox_Dep(46, 29);
            }
            else if (sem == 2)
            {
                CompulsaryPanel.Visibility = Visibility.Visible;
                Semester1Panel.Visibility = Visibility.Collapsed;
                Semester2Panel.Visibility = Visibility.Visible;
                GeneralPanel.Visibility = Visibility.Collapsed;
                AddModuleLabels();
                AddComboBox_Dep(34, 18);
            }
        }

        // Switching departments (Sem 1)
        private void Sem1Dep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Department_Sem1.SelectedItem is ComboBoxItem selectedDepSem1)
            {
                string deptS1 = selectedDepSem1.Content.ToString();
                department_S1 = deptS1;
                DepartmentSwitcher(deptS1);
            }
        }

        // Switching departments
        private void Department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Department.SelectedItem is ComboBoxItem selectedDept)
            {
                string deptS2 = selectedDept.Content.ToString();
                department = deptS2;
                DepartmentSwitcher(deptS2);
            }
        }

        // Add new departments here
        private void DepartmentSwitcher(string department)
        {
            AddModuleLabels();

            if (semester == 1)
            {
                AddComboBox_Dep(46, 29);
            }
            else if (semester == 2)
            {
                AddComboBox_Dep(34, 18);

                if (department == departments[0])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[1])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[2])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[3])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[4])
                {
                    Mech_Panel.Visibility = Visibility.Visible;
                    Department.Margin = new Thickness(58, 21, 240, 0);
                }
                else if (department == departments[5])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[6])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[7])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[8])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[9])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
                else if (department == departments[10])
                {
                    Mech_Panel.Visibility = Visibility.Collapsed;
                    Department.Margin = new Thickness(0, 21, 0, 0);
                }
            }
        }

        // Switching streams
        private void Mech_Stream_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Mech_Stream.SelectedItem is ComboBoxItem selected_MechStream)
            {
                string mechStream = selected_MechStream.Content.ToString();
                stream = mechStream;
                AddModuleLabels();
                AddComboBox_Dep(34, 18);
            }
        }

        // Add new streams here
        /*
        private void StreamSwitcher(string stream)
        {
            if (department == "Mechanical")
            {
                switch (stream)
                {
                    case "General":
                        AddComboBox_Dep(34, 18, 6);
                        break;

                    case "Aeronautical":
                        AddComboBox_Dep(34, 18, 6);
                        break;

                    case "Biomechanical":
                        AddComboBox_Dep(34, 18, 6);
                        break;

                    case "Mechatronic":
                        AddComboBox_Dep(34, 18, 6);
                        break;

                    default:
                        AddComboBox_Dep(34, 18, 6);
                        break;
                }
            }
        }*/

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
        private List<int> CompulsoryCredit()
        {
            List<int> credits = null;

            if (semester == 1)
            {
                if (department == departments[8])
                {
                    credits = new List<int> { 3, 3, 2, 2, 2, 2, 3 };
                }
                else if (department == departments[9])
                {
                    credits = new List<int> { 3, 3, 2, 3, 3, 2 };
                }
                else if (department == departments[10])
                {
                    credits = new List<int> { 3, 3, 2, 2, 2, 2 };
                }
                else
                {
                    credits = new List<int> { 3, 3, 2, 2, 2, 2 };
                }
            }
            else
            {
                if (department == departments[0]) 
                {
                    credits = new List<int> { 3, 3, 3, 3, 3, 2, 2 };
                }
                else if (department == departments[1])
                {
                    credits = new List<int> { 3, 4, 4, 3, 2, 2, 2, 3 };
                }
                else if (department == departments[2])
                {
                    credits = new List<int> { 3, 4, 4, 3, 2, 2, 2, 4 };
                }
                else if (department == departments[3])
                {
                    credits = new List<int> { 3, 3, 3, 3, 3, 2 };
                }
                else if (department == departments[4])
                {
                    credits = new List<int> { 3, 3, 3, 3, 3, 3, 2, 2 };
                }
                else if (department == departments[5])
                {
                    credits = new List<int> { 3, 3, 4, 3, 4, 2 };
                }
                else if (department == departments[6])
                {
                    credits = new List<int> { 3, 2, 3, 3, 3, 4, 2 };
                }
                else if (department == departments[7])
                {
                    credits = new List<int> { 3, 3, 3, 3, 2 };
                }
                else if (department == departments[8])
                {
                    credits = new List<int> { 3, 2, 3, 3, 3, 2, 2, 2, 2 };
                }
                else if (department == departments[9])
                {
                    credits = new List<int> { 3, 2, 3, 3, 3, 3, 2 };
                }
                else if (department == departments[10])
                {
                    credits = new List<int> { 3, 3, 2, 2, 3, 2, 2 };
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

        // Get grades of compulsory modules
        private List<double> GetGrades()
        {
            var gradesList = new List<double>();

            foreach (var gradeCombo in gradeComboList)
            {
                if (gradeCombo.SelectedIndex != -1)
                {
                    gradesList.Add(GradeToGradePoint(gradeCombo.SelectedIndex));
                }
            }

            return gradesList;
        }

        // Get General Tab Module Credit and Grade
        private (List<int> creditList, List<double> gradeList) getGeneralModules()
        {
            List<int> creditList = new List<int>();
            List<double> gradeList = new List<double>();

            foreach ((TextBox credit, ComboBox grade) in modulePairs)
            {

                if (!int.TryParse(credit.Text, out int creditValue))
                {
                    Result.Content = "Enter a valid credit value";
                }

                if (grade.SelectedIndex == -1)
                {
                    Result.Content = "Please input all grades";
                }

                creditList.Add(creditValue);
                gradeList.Add(GradeToGradePoint(grade.SelectedIndex));
            }

            return (creditList, gradeList);
        }

        // Calculate button clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var creditList = new List<int>();
            var gradeList = new List<double>();

            if (semester == 0)
            {
                (List<int> credit_List, List<double> grade_List) = getGeneralModules();

                creditList.AddRange(credit_List);
                gradeList.AddRange(grade_List);
            }
            else if (semester == 1)
            {
                var grades = GetGrades();
                var credits = CompulsoryCredit();

                gradeList.AddRange(grades);
                creditList.AddRange(credits);
            }
            else if (semester == 2)
            {
                var grades = GetGrades();
                var credits = CompulsoryCredit();

                (List<double> grades_El, List<int> credits_El) = GetSelectedElectives();

                grades.AddRange(grades_El);
                credits.AddRange(credits_El);

                gradeList.AddRange(grades);
                creditList.AddRange(credits);
            }

            double gpa = GpaCalculator(gradeList.ToArray(), creditList.ToArray());
            Result.Content = "GPA = " + gpa.ToString("F2");
        }
    }
}