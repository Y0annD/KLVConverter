
using KLVConverter.KLV;
using Microsoft.Extensions.Logging;

namespace KLVConverter.Tests.KLV;

public class KLVManagerTests
{

    KLVManager Manager;

    [SetUp]
    public void Setup()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("KLVManagerTests");
        Manager = new(logger);
        Manager.RegisterImplementation(new ST0601Standard(logger));
    }

    [Test]
    public void TestSeekToNextMessageFirstMatch()
    {
        Stream stream = new MemoryStream([0x6, 0xE, 0x2B, 0x34, 0x2, 0xB, 0x1, 0x1, 0xE, 0x1, 0x3, 0x1, 0x1, 0x0, 0x0, 0x0, 0x1, 0x0]);
        BinaryReader reader = new(stream);
        Assert.That(Manager.SeekToNextSMPTEKnownMessage(reader), Is.True);
        Assert.That(stream.Position, Is.EqualTo(16));
    }

    [Test]
    public void TestSeekToNextMessageWithOffset()
    {
        Stream stream = new MemoryStream([0x0, 0x6, 0xE, 0x2B, 0x34, 0x2, 0xB, 0x1, 0x1, 0xE, 0x1, 0x3, 0x1, 0x1, 0x0, 0x0, 0x0, 0x1, 0x0]);
        BinaryReader reader = new(stream);
        Assert.That(Manager.SeekToNextSMPTEKnownMessage(reader), Is.True);
        Assert.That(stream.Position, Is.EqualTo(17));
    }

    [Test]
    public void TestSeekToNextMessageNotFound()
    {
        Stream stream = new MemoryStream([0x0, 0x0, 0xE, 0x2B, 0x34, 0x2, 0xB, 0x1, 0x1, 0xE, 0x1, 0x3, 0x1, 0x1, 0x0, 0x0, 0x0, 0x1, 0x0]);
        BinaryReader reader = new(stream);
        Assert.That(Manager.SeekToNextSMPTEKnownMessage(reader), Is.False);
    }

    [Test]
    public void TestReadShortBerLength()
    {
        BinaryReader reader = new(new MemoryStream([1]));


        Assert.That(Manager.ReadOidLength(reader), Is.EqualTo(1));

        reader = new(new MemoryStream([0x7F]));


        Assert.That(Manager.ReadOidLength(reader), Is.EqualTo(0x7F));
    }

    [Test]
    public void TestReadLongBerLength()
    {
        BinaryReader reader = new(new MemoryStream([0x81, 0x1]));


        Assert.That(Manager.ReadOidLength(reader), Is.EqualTo(1));

        reader = new(new MemoryStream([0x81, 0xFF]));


        Assert.That(Manager.ReadOidLength(reader), Is.EqualTo(0xFF));

        reader = new(new MemoryStream([0x82, 0xFF, 0xFF]));

        Assert.That(Manager.ReadOidLength(reader), Is.EqualTo(65535));
    }

    [Test]
    public void TestReadNextKLVMessage()
    {
        BinaryReader reader = new(new MemoryStream([0x6, 0xE, 0x2B, 0x34, 0x2, 0xB, 0x1, 0x1, 0xE, 0x1, 0x3, 0x1, 0x1, 0x0, 0x0, 0x0/* Lenth*/, 0x6/* Data */, 0x3, 0x4, 0x54, 0x65, 0x73, 0x74]));

        Dictionary<int, KLVData> result = Manager.ReadNextKLVMessage(reader);

        Assert.That(result.Count, Is.EqualTo(1));
        KLVData testData = new();
        testData.Key = 3;
        testData.Length = 4;
        testData.Value = [0x54, 0x65, 0x73, 0x74];
        KLVData foundResult = result.GetValueOrDefault(3);
        Assert.That(foundResult, Is.Not.Null);
        Assert.That(foundResult.Key, Is.EqualTo(3));
        Assert.That(foundResult.Length, Is.EqualTo(4));
        Assert.That(foundResult.Value, Is.EqualTo(testData.Value));
    }
}
