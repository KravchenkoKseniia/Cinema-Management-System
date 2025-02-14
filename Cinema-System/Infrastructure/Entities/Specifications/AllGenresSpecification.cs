using Ardalis.Specification;

namespace Infrastructure.Entities.Specifications
{
    public sealed class AllGenresSpecification : Specification<Genre>
    {
        public AllGenresSpecification()
        {
            Query.OrderBy(g => g.GenreName);
        }
    }
}