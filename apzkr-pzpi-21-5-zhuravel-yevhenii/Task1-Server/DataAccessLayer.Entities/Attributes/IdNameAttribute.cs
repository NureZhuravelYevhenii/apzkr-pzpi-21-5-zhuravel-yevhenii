namespace DataAccessLayer.Entities.Attributes
{
    public class IdNameAttribute : Attribute
    {
        public string Name { get; set; }

        public IdNameAttribute(string name)
        {
            Name = name;
        }
    }
}
