using KLVConverter.KLV.ST0601.Converter;

namespace KLVConverter.Tests.KLV.ST0601.Converter;

public class FloatDatatypeConverterTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSimpleUINT16ToFloat()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("0,5"));
        Assert.That(dc.Accept([0, 2]), Is.EqualTo("1"));
    }
}
