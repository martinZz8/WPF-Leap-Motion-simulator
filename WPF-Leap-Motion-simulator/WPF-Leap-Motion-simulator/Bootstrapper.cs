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
        private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.RegisterPerRequest(typeof(ShellViewModel), null, typeof(ShellViewModel));
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        //protected override void OnExit(object sender, EventArgs e)
        //{

        //}
    }
}
