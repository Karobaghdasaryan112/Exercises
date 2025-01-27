using LINQtoSQL.Data;
using LINQtoSQL.Entities;
using LINQtoSQL.Interfaces;
namespace LINQtoSQL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private List<User> _entities;
        private DataSeeder _seeder;

        public UserRepository(DataSeeder dataSeeder)
        {
            _seeder = dataSeeder;
            _entities = _seeder.GenerateUserDatas();
        }

        public List<User> Entities => _entities;
        
    }
}
