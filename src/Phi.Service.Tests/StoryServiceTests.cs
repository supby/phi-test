namespace Phi.Service.Tests;
using Moq;
using Phi.Client;
using Phi.Model.Api;
using Microsoft.Extensions.Logging;

public class StoryServiceTests
{
    private const int requestedTestsCount = 2;
    private readonly int[] _storyIds = [33, 44, 55];
    private readonly Mock<IDataClient> _dataClientMock;

    public StoryServiceTests()
    {
        _dataClientMock = new Mock<IDataClient>();
        _dataClientMock
            .Setup(x => x.GetBestStoryIds())
            .Returns(Task.FromResult(_storyIds.AsEnumerable()));

        _dataClientMock
            .Setup(x => x.GetStoryById(It.IsIn<int>(_storyIds.Take(requestedTestsCount))))
            .Returns(Task.FromResult<Model.Client.Story?>(new Model.Client.Story
            {
                Id = 123,
                By = "test by",
                Score = 11,
                Time = 454545454,
                Title = "test title 1",
                Url = "test url 1",
                Descendants = 33
            }));
    }

    [Fact]
    public async void StoryService_Success_Exists_in_Cache()
    {
        var cacheServiceMock = new Mock<ICacheService<int,Story>>();
        cacheServiceMock
            .Setup(x => x.GetByKey(It.IsIn<int>(_storyIds.Take(requestedTestsCount))))
            .Returns(new Story {
                Title = "test 1",
                Uri = "test uri 1",
                PostedBy = "by test 1",
                Time = DateTime.Now,
                Score = 45,
                CommentCount = 4
            });

        var loggerMock = new Mock<ILogger<StoryService>>();

        var sut = new StoryService(_dataClientMock.Object, cacheServiceMock.Object, loggerMock.Object);
        var res = await sut.GetNBestStories(requestedTestsCount);

        Assert.Equal(requestedTestsCount, res.Count());

        _dataClientMock.Verify(x => x.GetBestStoryIds(), Times.Once);
        _dataClientMock.Verify(x => x.GetStoryById(It.IsIn<int>(_storyIds.Take(requestedTestsCount))), Times.Never);
        cacheServiceMock.Verify(x => x.GetByKey(It.IsIn<int>(_storyIds.Take(requestedTestsCount))), Times.Exactly(requestedTestsCount));
        cacheServiceMock.Verify(x => x.Add(It.IsIn<int>(_storyIds.Take(requestedTestsCount)), It.IsAny<Phi.Model.Api.Story>()), Times.Never);
    }

    [Fact]
    public async void StoryService_Success_Not_Exists_in_Cache()
    {
        var cacheServiceMock = new Mock<ICacheService<int,Story>>();
        cacheServiceMock
            .Setup(x => x.GetByKey(It.IsIn<int>(_storyIds.Take(requestedTestsCount))))
            .Returns(default(Story));

        var loggerMock = new Mock<ILogger<StoryService>>();

        var sut = new StoryService(_dataClientMock.Object, cacheServiceMock.Object, loggerMock.Object);
        var res = await sut.GetNBestStories(requestedTestsCount);

        Assert.Equal(requestedTestsCount, res.Count());

        _dataClientMock.Verify(x => x.GetBestStoryIds(), Times.Once);
        _dataClientMock.Verify(x => x.GetStoryById(It.IsIn<int>(_storyIds.Take(requestedTestsCount))), Times.Exactly(requestedTestsCount));
        cacheServiceMock.Verify(x => x.GetByKey(It.IsIn<int>(_storyIds.Take(requestedTestsCount))), Times.Exactly(requestedTestsCount));
        cacheServiceMock.Verify(x => x.Add(It.IsIn<int>(_storyIds.Take(requestedTestsCount)), It.IsAny<Phi.Model.Api.Story>()), Times.Exactly(requestedTestsCount));
    }

    [Fact]
    public async void StoryService_Success_Not_Exists_in_Cache_and_API_returns_null()
    {
        var cacheServiceMock = new Mock<ICacheService<int,Story>>();
        cacheServiceMock
            .Setup(x => x.GetByKey(It.IsIn<int>(_storyIds.Take(requestedTestsCount))))
            .Returns(default(Story));

        var dataClientMock = new Mock<IDataClient>();
        dataClientMock
            .Setup(x => x.GetBestStoryIds())
            .Returns(Task.FromResult(_storyIds.AsEnumerable()));
        dataClientMock
            .Setup(x => x.GetStoryById(It.IsIn<int>(_storyIds.Take(requestedTestsCount))))
            .Returns(Task.FromResult<Model.Client.Story?>(null));

        var loggerMock = new Mock<ILogger<StoryService>>();

        var sut = new StoryService(dataClientMock.Object, cacheServiceMock.Object, loggerMock.Object);
        var res = await sut.GetNBestStories(requestedTestsCount);

        Assert.Empty(res);

        dataClientMock.Verify(x => x.GetBestStoryIds(), Times.Once);
        dataClientMock.Verify(x => x.GetStoryById(It.IsIn<int>(_storyIds.Take(requestedTestsCount))), Times.Exactly(requestedTestsCount));
        cacheServiceMock.Verify(x => x.GetByKey(It.IsIn<int>(_storyIds.Take(requestedTestsCount))), Times.Exactly(requestedTestsCount));
    }
}