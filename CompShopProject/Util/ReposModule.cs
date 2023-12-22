using DAL.Repositories;
using Interfaces.Repositories;
using Ninject.Modules;

namespace CompShopProject.Util
{
    public class ReposModule : NinjectModule
    {
        private string connectionString;
        public ReposModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IDbRepos>().To<DbRepos>().InSingletonScope().WithConstructorArgument(connectionString);
        }
    }
}
