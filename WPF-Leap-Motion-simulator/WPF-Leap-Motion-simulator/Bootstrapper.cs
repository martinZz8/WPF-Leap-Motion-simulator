using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//Caliburn Micro
using Caliburn.Micro;

//ViewModels
using WPF_Leap_Motion_simulator.ViewModels;

namespace WPF_Leap_Motion_simulator
{
    public class Bootstrapper: BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        //protected override void OnExit(object sender, EventArgs e)
        //{
            
        //}
    }
}
