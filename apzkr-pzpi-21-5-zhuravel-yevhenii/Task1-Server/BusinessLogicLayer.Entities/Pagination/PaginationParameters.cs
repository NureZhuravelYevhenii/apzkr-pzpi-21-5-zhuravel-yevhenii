namespace BusinessLogicLayer.Entities.Pagination
{
    public class PaginationParameters
    {
        public static PaginationParameters All() => new PaginationParameters(0, int.MaxValue);

        public int Page { get; }
        public int CapacityOfPage { get; }

        public PaginationParameters(int page, int capacityOfPage)
        {
            Page = page;
            CapacityOfPage = capacityOfPage;
        }

        public int GetSkip()
        {
            return CapacityOfPage * Page;
        }

        public int GetTake()
        {
            return CapacityOfPage;
        }
    }
}
