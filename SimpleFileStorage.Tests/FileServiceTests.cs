using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Moq;
using SimpleFileStorage.Web.AppServices;
using SimpleFileStorage.Web.AppServices.Contract;
using SimpleFileStorage.Web.DataAccess;
using SimpleFileStorage.Web.Infrastructure;
using Testcontainers.PostgreSql;

namespace SimpleFileStorage.Tests;

[TestFixture]
public class FileServiceTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IGuidProvider> _guidProviderMock = new();
    private readonly DateTime _defaultDateTime = new(2099, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private readonly Guid _defaultGuid = Guid.CreateVersion7();
    private PostgreSqlContainer _pgContainer;
    private string _storagePath;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(_defaultDateTime);
        _guidProviderMock.Setup(x => x.CreateVersion7()).Returns(_defaultGuid);
        _pgContainer = new PostgreSqlBuilder()
            .WithDatabase("files")
            .WithUsername("postgres")
            .WithPassword("secret")
            .Build();

        await _pgContainer.StartAsync();

        _storagePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_storagePath);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _pgContainer.StopAsync();
        await _pgContainer.DisposeAsync();
        Directory.Delete(_storagePath, true);
    }

    private IFileService CreateService()
    {
        var options = new DbContextOptionsBuilder<StoredFilesContext>()
            .UseNpgsql(_pgContainer.GetConnectionString())
            .Options;
        var dbContext = new StoredFilesContext(options);
        dbContext.Database.EnsureCreated();

        var fileRepo = new FileRepository(dbContext);
        var settings = Options.Create(new StorageSettings { FileStoragePath = _storagePath, MaxFileSizeKb = 2048 });
        var fsRepo = new FileSystemRepository(new FakeEnvironment(), settings);
        return new FileService(fileRepo, fsRepo, _guidProviderMock.Object, _dateTimeProviderMock.Object, settings);
    }

    [Test]
    public async Task UploadDownloadDelete_Success()
    { 
        const string content = "Hello";
        string fileName = _defaultGuid.ToString();
        string fullFileName = $"nested/path/{fileName}";
        var service = CreateService();

        var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var formFile = new FormFile(fileContent, 0, fileContent.Length, "file", fileName);

        var uploaded = await service.UploadAsync(formFile, fullFileName);
        Assert.That(uploaded.FileName, Is.EqualTo(fullFileName));

        var downloaded = await service.DownloadAsync(uploaded.Id);
        using var reader = new StreamReader(downloaded.File);
        var text = await reader.ReadToEndAsync();
        Assert.That(text, Is.EqualTo(content));
        await downloaded.File.DisposeAsync();
        
        await service.DeleteAsync(uploaded.Id);
        Assert.ThrowsAsync<FileNotFoundException>(() => service.DownloadAsync(uploaded.Id));
    }

    private sealed record FakeEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; } = "TestApp";
        public string WebRootPath { get; set; } = string.Empty;
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
