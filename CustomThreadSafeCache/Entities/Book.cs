using CustomThreadSafeCache.Interfaces;


namespace CustomThreadSafeCache.Entities
{
    public class Book : IEntity
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public int Id => BookId;

        public override string ToString()
        {
            return $"{BookId},{Title},{Author},{Year}";
        }
    }

}
