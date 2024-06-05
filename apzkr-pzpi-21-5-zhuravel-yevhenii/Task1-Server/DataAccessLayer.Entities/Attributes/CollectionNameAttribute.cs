namespace DataAccessLayer.Entities.Attributes
{
    public class CollectionNameAttribute : Attribute
    {
        public string CollectionName { get; set; }
        public CollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
