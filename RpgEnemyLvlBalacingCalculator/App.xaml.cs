using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using RpgEnemyLvlBalacingCalculator.Services;
using RpgEnemyLvlBalacingCalculator.Views;

namespace RpgEnemyLvlBalacingCalculator
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        public IUnityContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            Container = new UnityContainer();

            //service registrations
            Container.RegisterType<ICalculationService, CalculationService>();
            
            //registraions utils
            //only one instance from messenger can exists! (recipient problems..)
            var messenger = new Messenger();
            Container.RegisterInstance(typeof (IMessenger), messenger);

            var mainWindow = Container.Resolve<MainWindow>();

            mainWindow.Show();
        }
    }
}