using VetAutoMobile.Entities.Guards;

namespace VetAutoMobile.Abstractions.Pages
{
    public interface IEntityWithGuards
    {
        public IEnumerable<Guard> Guards { get; }
    }
}
