using CustomThreadSafeCache.Entities;
public static class DataSeeder
{
    /// <summary>
    /// User Datas
    /// </summary>
    /// <returns></returns>
    public static List<User> GetUsers()
    {
        return new List<User>
        {
            new User { UserId = 1, Name = "Alice", Email = "alice@example.com", Role = "User" },
            new User { UserId = 2, Name = "Bob", Email = "bob@example.com", Role = "Admin" },
            new User { UserId = 3, Name = "Charlie", Email = "charlie@example.com", Role = "User" },
            new User { UserId = 4, Name = "David", Email = "david@example.com", Role = "User" },
            new User { UserId = 5, Name = "Eve", Email = "eve@example.com", Role = "Admin" },
            new User { UserId = 6, Name = "Frank", Email = "frank@example.com", Role = "User" },
            new User { UserId = 7, Name = "Grace", Email = "grace@example.com", Role = "User" },
            new User { UserId = 8, Name = "Hank", Email = "hank@example.com", Role = "Admin" },
            new User { UserId = 9, Name = "Ivy", Email = "ivy@example.com", Role = "User" },
            new User { UserId = 10, Name = "Jack", Email = "jack@example.com", Role = "User" }
        };
    }

    /// <summary>
    /// Product Datas
    /// </summary>
    /// <returns></returns>
    /// 
    public static List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { ProductId = 1, Name = "Laptop", Price = 1200, Category = "Electronics" },
            new Product { ProductId = 2, Name = "Smartphone", Price = 800, Category = "Electronics" },
            new Product { ProductId = 3, Name = "Keyboard", Price = 100, Category = "Accessories" },
            new Product { ProductId = 4, Name = "Mouse", Price = 50, Category = "Accessories" },
            new Product { ProductId = 5, Name = "Monitor", Price = 300, Category = "Electronics" },
            new Product { ProductId = 6, Name = "Headphones", Price = 150, Category = "Audio" },
            new Product { ProductId = 7, Name = "Webcam", Price = 120, Category = "Accessories" },
            new Product { ProductId = 8, Name = "Printer", Price = 400, Category = "Office" },
            new Product { ProductId = 9, Name = "Desk Chair", Price = 250, Category = "Furniture" },
            new Product { ProductId = 10, Name = "USB Drive", Price = 20, Category = "Accessories" }
        };
    }
    /// <summary>
    /// Book Datas
    /// </summary>
    /// <returns></returns>
    /// 
    public static List<Book> GetBooks()
    {
        return new List<Book>
        {
            new Book { BookId = 1, Title = "1984", Author = "George Orwell", Year = 1949 },
            new Book { BookId = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960 },
            new Book { BookId = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925 },
            new Book { BookId = 4, Title = "Moby Dick", Author = "Herman Melville", Year = 1851 },
            new Book { BookId = 5, Title = "War and Peace", Author = "Leo Tolstoy", Year = 1869 },
            new Book { BookId = 6, Title = "Pride and Prejudice", Author = "Jane Austen", Year = 1813 },
            new Book { BookId = 7, Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937 },
            new Book { BookId = 8, Title = "Harry Potter", Author = "J.K. Rowling", Year = 1997 },
            new Book { BookId = 9, Title = "The Catcher in the Rye", Author = "J.D. Salinger", Year = 1951 },
            new Book { BookId = 10, Title = "Ulysses", Author = "James Joyce", Year = 1922 }
        };
    }

    /// <summary>
    /// Order Datas
    /// </summary>
    /// <returns></returns>
    public static List<Order> GetOrders()
    {
        return new List<Order>
        {
            new Order { OrderId = 1, UserId = 1, ProductId = 2, Quantity = 1, Status = "Completed" },
            new Order { OrderId = 2, UserId = 2, ProductId = 1, Quantity = 1, Status = "Pending" },
            new Order { OrderId = 3, UserId = 3, ProductId = 5, Quantity = 2, Status = "Shipped" },
            new Order { OrderId = 4, UserId = 4, ProductId = 4, Quantity = 3, Status = "Processing" },
            new Order { OrderId = 5, UserId = 5, ProductId = 3, Quantity = 1, Status = "Completed" },
            new Order { OrderId = 6, UserId = 6, ProductId = 7, Quantity = 1, Status = "Pending" },
            new Order { OrderId = 7, UserId = 7, ProductId = 6, Quantity = 4, Status = "Shipped" },
            new Order { OrderId = 8, UserId = 8, ProductId = 8, Quantity = 1, Status = "Processing" },
            new Order { OrderId = 9, UserId = 9, ProductId = 10, Quantity = 2, Status = "Completed" },
            new Order { OrderId = 10, UserId = 10, ProductId = 9, Quantity = 1, Status = "Shipped" }
        };
    }
}
