using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Leap_Motion_simulator.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            this.Loaded += OnControlLoaded;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            // Test code
            Console.WriteLine("Loaded");
            //Point relativePoint = LoadReceiveTheParcelView.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            //Console.WriteLine("Point X:" + relativePoint.X + " Point Y: " + relativePoint.Y);

            // here i don;t know what to do to pass data to viewModel - do something else
        }
    }
}