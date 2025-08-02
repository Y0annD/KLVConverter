using KLVConverter.KLV;
using Microsoft.Extensions.Logging;

namespace KLVConverter.Tests.KLV;

public class ST00601StandardTests
{

    private ST0601Standard Implementation;

    [SetUp]
    public void SetUp()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("ST00601StandardTests");
        Implementation = new(logger);
    }

    [Test]
    public void TestGetTagName()
    {
        Assert.That(Implementation.GetTagName(0), Is.EqualTo("0"));
        Assert.That(Implementation.GetTagName(1), Is.EqualTo("1:Checksum"));
    }

    [Test]
    public void TestGetValueToString()
    {
        KLVData data0 = new KLVData
        {
            Key = 0,
            Length = 4,
            Value = [0, 0, 0, 0]
        };
        KLVData data1 = new KLVData
        {
            Key = 1,
            Length = 2,
            Value = [0, 1]
        };
        Assert.That(Implementation.ValueToStringOutput(data0), Is.EqualTo("[0,0,0,0]"));
        Assert.That(Implementation.ValueToStringOutput(data1), Is.EqualTo("1"));
    }


}