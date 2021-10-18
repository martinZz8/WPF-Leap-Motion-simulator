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

//Caliburn Micro
using Caliburn.Micro;

// Leap Motion - models
using WPF_Leap_Motion_simulator.Models;

// Leap Motion - static classes
using WPF_Leap_Motion_simulator.StaticClasses;

namespace WPF_Leap_Motion_simulator.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl, IHandle<HandleRequestButtonData>
    {
        public MenuView()
        {
            InitializeComponent();
            //DialogEventAggregatorProvider.eventAggregator.Subscribe(this);
            this.Loaded += OnControlLoaded;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            // Test code
            Console.WriteLine("Loaded");
            Point relativePoint = LoadReceiveTheParcelView.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            Console.WriteLine("Point X:" + relativePoint.X + " Point Y: " + relativePoint.Y);

            // here can be code with firing event aggregator with delegate to get actual position of buttons
            
        }

        private List<ButtonData> GetButtonPositions()
        {
            List <ButtonData> buttonList = new List<ButtonData>();

            //LoadReceiveTheParcelView btn
            Point LoadReceiveTheParcelViewPoint = LoadReceiveTheParcelView.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            buttonList.Add(new ButtonData {
                RelativePositionX = LoadReceiveTheParcelViewPoint.X,
                RelativePositionY = LoadReceiveTheParcelViewPoint.Y,
                ButtonWidth = LoadReceiveTheParcelView.ActualWidth,
                ButtonHeight = LoadReceiveTheParcelView.ActualHeight
            });

            return buttonList;
        }

        // Handle RequestButtonData
        public void Handle(HandleRequestButtonData message)
        {
            if(message.ButtonType == ButtonTypes.RECEIVE_THE_PARCEL)
            {
                DialogEventAggregatorProvider.eventAggregator.Publish(
                    new HandleSendButtonData { ButtonList = GetButtonPositions() },
                    action => { }
                );
            }
        }
    }
}
