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

namespace NorbertKaminskiFiltrPasywnyRLC
{
    public partial class MainWindow : Window
    {
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
        }

        private void DrawWaveforms_Click(object sender, RoutedEventArgs e)
        {
            //Hide the window
            this.WindowHidden();
            this.InputBoxesVisibility();
            this.HideValues();
            SchematicHeader.Text = "To be done";

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
            DarkEffect.Visibility = Visibility.Visible;
            DataInputWindow.Visibility = Visibility.Visible;
        }

        //Method hides window
        private void WindowHidden()
        {
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
            IndutanceValue.Text = "0,001";
            CapacitanceValue.Text = "0,000001";
            VoltageOneValue.Text = "230";
            ResistanceOneValue.Text = "1000";
            ResistanceTwoValue.Text = "2000";
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
    }
}
