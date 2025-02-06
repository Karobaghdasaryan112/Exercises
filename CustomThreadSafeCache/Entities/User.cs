using CustomThreadSafeCache.Interfaces;

namespace CustomThreadSafeCache.Entities
{
    public class User : IEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public int Id => UserId;

        public override string ToString()
        {
            return $"{UserId},{Name},{Email},{Role}";
        }
    }
}
