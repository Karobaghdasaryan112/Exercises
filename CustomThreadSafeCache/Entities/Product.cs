using CustomThreadSafeCache.Interfaces;


namespace CustomThreadSafeCache.Entities
{
    public class Product : IEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public int Id => ProductId;

        public override string ToString()
        {
            return $"{ProductId},{Name},{Price},{Category}";
        }
    }

}
