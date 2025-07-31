using KLVConverter.KLV.ST0601.Converter;

namespace KLVConverter.Tests.KLV.ST0601.Converter;

public class StringDatatypeConverterTests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void TestSimpleString()
    {
        StringDataTypeConverter dc = new();

        Assert.That(dc.Accept([0x54, 0x65, 0x73,0x74]), Is.EqualTo("Test"));
    }
}