using AutoMapper;
using Cinema_System.Mapping;

namespace CinemaSystem.Test;

public class MapperFixture
{
    public IMapper Mapper { get; }

    public MapperFixture()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        Mapper = config.CreateMapper();
    }
}
