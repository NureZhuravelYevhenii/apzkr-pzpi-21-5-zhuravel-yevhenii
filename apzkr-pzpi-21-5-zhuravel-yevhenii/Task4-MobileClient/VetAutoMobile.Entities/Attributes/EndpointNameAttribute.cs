namespace VetAutoMobile.Entities.Attributes
{
    public class EndpointNameAttribute : Attribute
    {
        public string EndpointName { get; set; }

        public EndpointNameAttribute(string endpointName) 
        { 
            EndpointName = endpointName;
        }
    }
}
