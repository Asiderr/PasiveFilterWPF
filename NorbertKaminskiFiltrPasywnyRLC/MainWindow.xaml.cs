using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;
using Microsoft.Win32;
using System.Data;


namespace NorbertKaminskiFiltrPasywnyRLC
{
    public partial class MainWindow : Window
    {
        private double InductanceChartValue;
        private double CapacitanceChartValue;
        private double VoltageOneChartValue;
        private double ResistanceOneChartValue;
        private double ResistanceTwoChartValue;

        private string TemporaryInductance;
        private string TemporaryCapacitance;
        private string TemporaryVoltage;
        private string TemporaryResistanceOne;
        private string TemporaryResistanceTwo;

        private Chart PhaseAndMagnitude;

        public MainWindow()
        {
            InitializeComponent();
            // Initalize schematic view
            SchematicHeader.Text = "Click on an element to change its value";
            Schematic.ImageSource = new BitmapImage(new Uri(@"..\..\Images\schema.JPG", UriKind.Relative));
            this.WindowHidden();
            this.InputBoxesVisibility();
            this.InitDefaultValues();
            this.ShowValues();
            this.ValidateValues();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Schematic_Click(object sender, RoutedEventArgs e)
        {
            //Hide the window
            this.WindowHidden();
            this.InputBoxesVisibility();

            //Check if the schematic exists
            if (Schematic.ImageSource == null)
            {
                SchematicHeader.Text = "Click on an element to change its value";
                Schematic.ImageSource = new BitmapImage(new Uri(@"..\..\Images\schema.JPG", UriKind.Relative));
            }

            //Enable the elements buttons
            Inductance.IsEnabled = true;
            Capacitance.IsEnabled = true;
            ResistanceOne.IsEnabled = true;
            ResistanceTwo.IsEnabled = true;
            VoltageOne.IsEnabled = true;

            //Show Values of the elements
            this.ShowValues();

            //Validate Valuse
            this.ValidateValues();
        }

        private void DrawWaveforms_Click(object sender, RoutedEventArgs e)
        {
            //Hide the window
            this.WindowHidden();
            this.InputBoxesVisibility();
            this.HideValues();
            PhaseAndMagniutdeChart.Visibility = Visibility.Visible;
            SchematicHeader.Text = "Phase and magnitude spectrum";

            var Results = new List<List<double>>();

            PhaseAndMagnitude = new Chart();
            PhaseAndMagniutdeChart.Child = PhaseAndMagnitude;
            PhaseAndMagnitude.ChartAreas.Add(new ChartArea("Magnitude"));
            PhaseAndMagnitude.ChartAreas.Add(new ChartArea("Phase"));

            Results = this.CountChartValues();
            DataTable dTable;
            DataView dView;
            dTable = new DataTable();
            DataColumn column;
            DataRow row;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Frequency";
            dTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Magnitude";
            dTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Phase";
            dTable.Columns.Add(column);

            for (int i = 0; i < Results[0].Count ; i++)
            {
                row = dTable.NewRow();
                row["Frequency"] = Results[0][i];
                row["Magnitude"] = Results[1][i];
                row["Phase"] = Results[2][i];
                dTable.Rows.Add(row);
            }

            dView = new DataView(dTable);

            PhaseAndMagnitude.Series.Clear();
            PhaseAndMagnitude.Titles.Clear();

            PhaseAndMagnitude.DataBindTable(dView, "Frequency");
            PhaseAndMagnitude.Series["Magnitude"].ChartType =
                System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            PhaseAndMagnitude.Series["Phase"].ChartType =
                System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            PhaseAndMagnitude.Series["Magnitude"].ChartArea = "Magnitude";
            PhaseAndMagnitude.Series["Phase"].ChartArea = "Phase";

            PhaseAndMagnitude.ChartAreas[0].AxisY.Title = "Magnitude [db]";
            PhaseAndMagnitude.ChartAreas[0].AxisX.Minimum = 0.000000001;
            PhaseAndMagnitude.ChartAreas[0].AxisX.IsLogarithmic = true;
            PhaseAndMagnitude.ChartAreas[0].AxisX.Title = "Frequency [Hz]";
            PhaseAndMagnitude.ChartAreas[1].AxisY.Title = "Phase [deg]";
            PhaseAndMagnitude.ChartAreas[1].AxisX.Minimum = 0.000000001;
            PhaseAndMagnitude.ChartAreas[1].AxisX.IsLogarithmic = true;
            PhaseAndMagnitude.ChartAreas[1].AxisX.Title = "Frequency [Hz]";

            PhaseAndMagnitude.ChartAreas["Magnitude"].AxisY.LabelAutoFitStyle
                = LabelAutoFitStyles.None;
            PhaseAndMagnitude.ChartAreas["Magnitude"].AxisX.LabelStyle.Font
                = new System.Drawing.Font("Agency FB", 15, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Magnitude"].AxisX.TitleFont
                = new System.Drawing.Font("Agency FB", 25, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Magnitude"].AxisY.LabelStyle.Font
                = new System.Drawing.Font("Agency FB", 15, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Magnitude"].AxisY.TitleFont
                = new System.Drawing.Font("Agency FB", 25, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Magnitude"].AxisX.TitleForeColor
                = System.Drawing.ColorTranslator.FromHtml("#232645");
            PhaseAndMagnitude.ChartAreas["Phase"].AxisY.TitleForeColor
                = System.Drawing.ColorTranslator.FromHtml("#232645");

            PhaseAndMagnitude.ChartAreas["Phase"].AxisY.LabelAutoFitStyle
                = LabelAutoFitStyles.None;
            PhaseAndMagnitude.ChartAreas["Phase"].AxisX.LabelStyle.Font
                = new System.Drawing.Font("Agency FB", 15, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Phase"].AxisX.TitleFont
                = new System.Drawing.Font("Agency FB", 25, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Phase"].AxisY.LabelStyle.Font
                = new System.Drawing.Font("Agency FB", 15, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Phase"].AxisY.TitleFont
                = new System.Drawing.Font("Agency FB", 25, System.Drawing.FontStyle.Bold);
            PhaseAndMagnitude.ChartAreas["Phase"].AxisX.TitleForeColor
                = System.Drawing.ColorTranslator.FromHtml("#232645");
            PhaseAndMagnitude.ChartAreas["Phase"].AxisY.TitleForeColor
                = System.Drawing.ColorTranslator.FromHtml("#232645");

            //Disable the elements buttons
            this.DisableSchematic();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            //Show the window
            this.WindowVisible();
            this.InputBoxesVisibility();
            AboutText.Visibility = Visibility.Visible;

            //About text
            AboutText.Text = "Norbert Kaminski \nGdansk © 2020" +
                "\nProjektowanie Aplikacji Komputerowych";
        }

        private void Inductance_Click(object sender, RoutedEventArgs e)
        {
            //Show window
            this.WindowInit();
            IndutanceValue.Visibility = Visibility.Visible;

            //Indutance input text
            DataInputText.Text = "\nSet the inductance value [H]";
            ElementName.Text = "L = ";
        }
            private void Capacitance_Click(object sender, RoutedEventArgs e)
        {
            //Show window
            this.WindowInit();
            CapacitanceValue.Visibility = Visibility.Visible;

            //Capacitance input text
            DataInputText.Text = "\nSet the capacitance value [F]";
            ElementName.Text = "C = ";

            TemporaryCapacitance = "";
            foreach (char element in CapacitanceValue.Text)
            {
                if (element == ',')
                {
                    TemporaryCapacitance += '.';
                }
                else
                {
                    TemporaryCapacitance += element;
                }
            }
        }

        private void VoltageOne_Click(object sender, RoutedEventArgs e)
        {
            //Show window
            this.WindowInit();
            VoltageOneValue.Visibility = Visibility.Visible;

            //Voltage input text
            DataInputText.Text = "\nSet the voltage value [V]";
            ElementName.Text = "V = ";
        }

        private void ResistanceOne_Click(object sender, RoutedEventArgs e)
        {
            //Show window
            this.WindowInit();
            ResistanceOneValue.Visibility = Visibility.Visible;

            //Resistance one text
            DataInputText.Text = "\nSet the first resistance value [\u2126]";
            ElementName.Text = "R1 = ";
        }

        private void ResistanceTwo_Click(object sender, RoutedEventArgs e)
        {
            //Show window
            this.WindowInit();

            //Resistance two text
            ResistanceTwoValue.Visibility = Visibility.Visible;
            DataInputText.Text = "\nSet the second resistance value [\u2126]";
            ElementName.Text = "R2 = ";
        }

        //Method shows window
        private void WindowVisible()
        {
            PhaseAndMagniutdeChart.Visibility = Visibility.Hidden;
            DarkEffect.Visibility = Visibility.Visible;
            DataInputWindow.Visibility = Visibility.Visible;
        }

        //Method hides window
        private void WindowHidden()
        {
            PhaseAndMagniutdeChart.Visibility = Visibility.Hidden;
            DarkEffect.Visibility = Visibility.Hidden;
            DataInputWindow.Visibility = Visibility.Hidden;
        }

        //Method hides inputboxes and input description
        private void InputBoxesVisibility()
        {
            DataInputText.Visibility = Visibility.Hidden;
            ElementName.Visibility = Visibility.Hidden;
            AboutText.Visibility = Visibility.Hidden;
            SaveValueButton.Visibility = Visibility.Hidden;
            IndutanceValue.Visibility = Visibility.Hidden;
            CapacitanceValue.Visibility = Visibility.Hidden;
            VoltageOneValue.Visibility = Visibility.Hidden;
            ResistanceOneValue.Visibility = Visibility.Hidden;
            ResistanceTwoValue.Visibility = Visibility.Hidden;
        }

        //Method makes visible input description
        private void InputElementsVisibility()
        {
            DataInputText.Visibility = Visibility.Visible;
            ElementName.Visibility = Visibility.Visible;
            SaveValueButton.Visibility = Visibility.Visible;
        }

        //Method initializes input window
        private void WindowInit()
        {
            this.WindowVisible();
            this.InputBoxesVisibility();
            this.InputElementsVisibility();
        }

        //Method initializes a default values of
        //the schematic elements
        private void InitDefaultValues()
        {
            IndutanceValue.Text = "0,01";
            CapacitanceValue.Text = "0,22";
            VoltageOneValue.Text = "230";
            ResistanceOneValue.Text = "2000";
            ResistanceTwoValue.Text = "9000";

            TemporaryInductance = "0.01";
            TemporaryCapacitance = "0.22";
            TemporaryResistanceOne = ResistanceOneValue.Text;
            TemporaryResistanceTwo = ResistanceTwoValue.Text;
            TemporaryVoltage = VoltageOneValue.Text;
        }

        //Method shows values of the elements
        private void ShowValues()
        {
            ShowInductanceValue.Visibility = Visibility.Visible;
            ShowCapacitanceValue.Visibility = Visibility.Visible;
            ShowVoltageValue.Visibility = Visibility.Visible;
            ShowResistanceOneValue.Visibility = Visibility.Visible;
            ShowResistanceTwoValue.Visibility = Visibility.Visible;
            ShowVoltageTwo.Visibility = Visibility.Visible;

            ShowInductanceValue.Text = "L = " +
                IndutanceValue.Text + "H";
            ShowCapacitanceValue.Text = "C = " +
                CapacitanceValue.Text + "F";
            ShowVoltageValue.Text = "U1 = " +
                VoltageOneValue.Text + "V";
            ShowResistanceOneValue.Text = "R1 = " +
                ResistanceOneValue.Text + "\u2126";
            ShowResistanceTwoValue.Text = "R2 = " +
                ResistanceTwoValue.Text + "\u2126";
            ShowVoltageTwo.Text = "U2";
        }

        private void HideValues()
        {
            ShowInductanceValue.Visibility = Visibility.Hidden;
            ShowCapacitanceValue.Visibility = Visibility.Hidden;
            ShowVoltageValue.Visibility = Visibility.Hidden;
            ShowResistanceOneValue.Visibility = Visibility.Hidden;
            ShowResistanceTwoValue.Visibility = Visibility.Hidden;
            ShowVoltageTwo.Visibility = Visibility.Hidden;
        }

        private void DisableSchematic()
        {
            Schematic.ImageSource = null;
            Inductance.IsEnabled = false;
            Capacitance.IsEnabled = false;
            ResistanceOne.IsEnabled = false;
            ResistanceTwo.IsEnabled = false;
            VoltageOne.IsEnabled = false;
        }

        private void ValidateValues()
        {
            TemporaryInductance = "";
            TemporaryCapacitance = "";
            TemporaryResistanceTwo = "";
            TemporaryResistanceOne = "";
            TemporaryVoltage = "";

            //Comma to dot
            foreach (char element in IndutanceValue.Text)
            {
                if (element == ',')
                {
                    TemporaryInductance += '.';
                }
                else
                {
                    TemporaryInductance += element;
                }
            }

            foreach (char element in CapacitanceValue.Text)
            {
                if (element == ',')
                {
                    TemporaryCapacitance += '.';
                }
                else
                {
                    TemporaryCapacitance += element;
                }
            }

            foreach (char element in ResistanceTwoValue.Text)
            {
                if (element == ',')
                {
                    TemporaryResistanceTwo += '.';
                }
                else
                {
                    TemporaryResistanceTwo += element;
                }
            }

            foreach (char element in ResistanceOneValue.Text)
            {
                if (element == ',')
                {
                    TemporaryResistanceOne += '.';
                }
                else
                {
                    TemporaryResistanceOne += element;
                }
            }

            foreach (char element in VoltageOneValue.Text)
            {
                if (element == ',')
                {
                    TemporaryVoltage += '.';
                }
                else
                {
                    TemporaryVoltage += element;
                }
            }

            //Validate value
            if (!Double.TryParse(TemporaryInductance, out InductanceChartValue))
            {
                this.WindowInit();
                IndutanceValue.Visibility = Visibility.Visible;
                ElementName.Text = "L = ";
                DataInputText.Text = "\nWRONG INDUCTANCE VALUE!!";
            }
            if (!Double.TryParse(TemporaryCapacitance, out CapacitanceChartValue))
            {
                this.WindowInit();
                CapacitanceValue.Visibility = Visibility.Visible;
                ElementName.Text = "C = ";
                DataInputText.Text = "\nWRONG CAPACITANCE VALUE!!";
            }
            if (!Double.TryParse(TemporaryResistanceOne, out ResistanceOneChartValue))
            {
                this.WindowInit();
                ResistanceOneValue.Visibility = Visibility.Visible;
                ElementName.Text = "R1 = ";
                DataInputText.Text = "\nWRONG RESISTANCE VALUE!!";
            }
            if (!Double.TryParse(TemporaryResistanceTwo, out ResistanceTwoChartValue))
            {
                this.WindowInit();
                ResistanceTwoValue.Visibility = Visibility.Visible;
                ElementName.Text = "R2 = ";
                DataInputText.Text = "\nWRONG RESISTANCE VALUE!!";
            }
            if (!Double.TryParse(TemporaryVoltage, out VoltageOneChartValue))
            {
                this.WindowInit();
                VoltageOneValue.Visibility = Visibility.Visible;
                ElementName.Text = "V = ";
                DataInputText.Text = "\nWRONG VOLTAGE VALUE!!";
            }
        }

        private List<List<double>> CountChartValues()
        {
            // asix X list
            var asixX = new List<double>();
            // asix Y list - magnitude
            var asixMagnitudeY = new List<double>();
            // asix Y list - magnitude
            var asixPhaseY = new List<double>();

            var result = new List<List<double>>();

            double T1 = InductanceChartValue * (ResistanceOneChartValue + ResistanceTwoChartValue) / (ResistanceOneChartValue * ResistanceTwoChartValue);
            double T2 = CapacitanceChartValue * InductanceChartValue / (ResistanceOneChartValue * ResistanceTwoChartValue);
            double T3 = CapacitanceChartValue / ResistanceTwoChartValue;

            // asis X generation
            for (int k = -9; k < 5; k++)
            {
                for (int j = 1; j < 10; j++)
                {
                    asixX.Add(j * Math.Pow(10, k));
                };
            };

            // asix Y generation - magnitude
            foreach (double f in asixX)
            {
                double omega = 2 * Math.PI * f;
                double real = Math.Pow((T3 - T1 * omega), 2);
                double img = Math.Pow((1 + T2) * omega, 2);
                double denominator = Math.Sqrt(real + img);
                double module = omega / denominator;
                asixMagnitudeY.Add(20 * Math.Log10(module));
            }

            // asix Y generation - phase
            foreach (double f in asixX)
            {
                double omega = 2 * Math.PI * f;
                double nominator = (T3 - T1 * omega);
                double denominator = (T2 + 1) * omega;
                double argument = nominator / denominator;
                asixPhaseY.Add((180 / Math.PI) * Math.Atan(argument));
            }

            result.Add(asixX);
            result.Add(asixMagnitudeY);
            result.Add(asixPhaseY);
            return result;
        }
    }
}
