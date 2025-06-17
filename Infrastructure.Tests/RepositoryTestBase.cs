using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly AssocTMCContext context;

    protected RepositoryTestBase()
    {
        // Configure AutoMapper
        //var config = new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile<DataModelMappingProfile>();
        //});
        //_mapper = config.CreateMapper();
        _mapper = new Mock<IMapper>();
        // Configure in-memory database
        var options = new DbContextOptionsBuilder<AssocTMCContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        context = new AssocTMCContext(options);
    }
}
