namespace TypeHelpers
{
    public static class TypeHelper<T>
    {
        static TypeHelper()
        {
            PropertyNames = typeof(T)
                .GetProperties()
                .Select(p => p.Name)
                .ToList();
        }

        public static IEnumerable<string> PropertyNames { get; }
    }
}
