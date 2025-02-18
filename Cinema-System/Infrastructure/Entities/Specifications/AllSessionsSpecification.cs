using Ardalis.Specification;

namespace Infrastructure.Entities.Specifications
{
    public sealed class AllSessionsSpecification : Specification<Session>
    {
        public AllSessionsSpecification()
        {
            Query.Include(s => s.Movie);
            Query.Include(s => s.Hall);
        }
    }
}