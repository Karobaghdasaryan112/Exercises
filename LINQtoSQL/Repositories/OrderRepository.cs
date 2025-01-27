
using LINQtoSQL.Data;
using LINQtoSQL.Entities;
using LINQtoSQL.Interfaces;

namespace LINQtoSQL.Repositories
{
    public class OrderRepository : IRepository<Orders>
    {
        private DataSeeder _seeder;
        public OrderRepository(DataSeeder dataSeeder)
        {
            _seeder = dataSeeder;
        }
        public List<Orders> Entities { get => _seeder.GenerateOrderDatas(); set => throw new NotImplementedException(); }

    }       
}
