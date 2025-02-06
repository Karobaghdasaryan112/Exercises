using CustomThreadSafeCache.Entities;
using CustomThreadSafeCache.Interfaces;
using System.Text.Json;


namespace CustomThreadSafeCache.Datas
{
    public class SetIntoFile : FileAccsess, IDataCollection
    {

        
        private static SetIntoFile? _instance;
        private SetIntoFile()
        {
            Set(); 
        }

        /// <summary>
        /// implemented Singleton Pattern
        /// </summary>
        /// <returns></returns>
        public static SetIntoFile GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SetIntoFile();
            }
            return _instance;
        }

        public IEnumerable<Book> GetBooks() => DataSeeder.GetBooks();

        public IEnumerable<Order> GetOrders() => DataSeeder.GetOrders();

        public IEnumerable<Product> GetProducts() => DataSeeder.GetProducts();

        public IEnumerable<User> GetUsers() => DataSeeder.GetUsers();

        /// <summary>
        /// Set Datas Into File 
        /// </summary>
        private void Set()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            string BooksAsJson = JsonSerializer.Serialize<IEnumerable<Book>>(GetBooks(), options);

            string OrdersAsJson = JsonSerializer.Serialize<IEnumerable<Order>>(GetOrders(), options);

            string ProductsAsJson = JsonSerializer.Serialize<IEnumerable<Product>>(GetProducts(), options);

            string UsersAsJson = JsonSerializer.Serialize<IEnumerable<User>>(GetUsers(), options);

            List<string> EntitiesAsJson = new List<string> { BooksAsJson, OrdersAsJson, ProductsAsJson, UsersAsJson };

            string EntitiesAsJsonWithLines = string.Join(Environment.NewLine, EntitiesAsJson);

            File.WriteAllLines(File_Path, EntitiesAsJson);
            
        }
    }
}
