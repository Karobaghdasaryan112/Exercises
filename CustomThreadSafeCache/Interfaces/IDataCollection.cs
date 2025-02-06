using CustomThreadSafeCache.Entities;


namespace CustomThreadSafeCache.Interfaces
{
    public interface IDataCollection
    {
        IEnumerable<Book> GetBooks();

        IEnumerable<Order> GetOrders();

        IEnumerable<Product> GetProducts();

        IEnumerable<User> GetUsers();
    }
}
