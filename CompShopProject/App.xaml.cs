using CompShopProject.Util;
using CompShopProject.View;
using Ninject;
using System.Windows;

namespace CompShopProject
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создание ядра Ninject
            IKernel kernel = new StandardKernel();

            // Загрузка модулей Ninject
            kernel.Load(new NinjectRegistrations(), new ReposModule("ModelsManager"));

            // Создание и показ начального окна
            AuthorizationView authorizationView = kernel.Get<AuthorizationView>();
            authorizationView.Show();
        }
    }
}
