using DeepCopy.Enums;


namespace DeepCopy.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CopyAttribute : Attribute
    {
        public CopyEnum copy { get; set; }
        public CopyAttribute(CopyEnum copy)
        {
            this.copy = copy;
        }
    }
}
