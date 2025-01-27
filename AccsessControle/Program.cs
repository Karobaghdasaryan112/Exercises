using AccsessControl.Data;
using AccsessControl.Interfaces;
using AccsessControl.Services;
using System.Reflection;

namespace AccsessControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Create some Entities 
            DataSeeder seeder = new DataSeeder();
            seeder.SetDatas();

            //Cerate Service For Operation with Entity
            IEntityService entityService = new EntityServices(seeder);

            //Create Proxy Service
            var proxy = DispatchProxy.Create<IEntityService, ProxyService<IEntityService>>();
            ((ProxyService<IEntityService>)proxy).SetService(entityService);
            ((ProxyService<IEntityService>)proxy).SetCurrentRole(Enums.Roles.User);
            proxy.UpdateMeneger(1);
            Console.ReadLine();
        }
    }
}