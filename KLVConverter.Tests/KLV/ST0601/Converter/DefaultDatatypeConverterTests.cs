using KLVConverter.KLV.ST0601.Converter;

namespace KLVConverter.Tests.KLV.ST0601.Converter;

public class DefaultDatatypeConverterTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestEmptyByteArray()
    {
        
        Assert.That(new DefaultDatatypeConverter().Accept([]), Is.EqualTo("[]"));
    }

    [Test]
    public void TestOneItemByteArray()
    {
        Assert.That(new DefaultDatatypeConverter().Accept([128]), Is.EqualTo("[128]"));
    }


    [Test]
    public void TestMultipleItemByteArray()
    {
        Assert.That(new DefaultDatatypeConverter().Accept([128, 1, 0, 3]), Is.EqualTo("[128,1,0,3]"));
    }
}
