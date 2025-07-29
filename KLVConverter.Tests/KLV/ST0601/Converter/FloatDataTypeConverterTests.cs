using KLVConverter.KLV.ST0601.Converter;

namespace KLVConverter.Tests.KLV.ST0601.Converter;

public class FloatDatatypeConverterTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestWithBadInputType()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.STRING, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        Assert.That(dc.Accept([0, 0x14]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0xFF, 0xFF]), Is.EqualTo("0"));
    }


    [Test]
    public void TestSimpleUINT16ToFloat()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("0,5"));
        Assert.That(dc.Accept([0xFF, 0xFF]), Is.EqualTo("32767,5"));
    }

    [Test]
    public void TestSimpleUINT16ToFloatWithOffset()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 1);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("1,5"));
    }

    [Test]
    public void TestUINT16ToFloatError()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 2 but was 1"));
    }

    [Test]
    public void TestSimpleINT16ToFloat()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("0,5"));
        Assert.That(dc.Accept([0xFF, 0xFF]), Is.EqualTo("-0,5"));
    }

    [Test]
    public void TestINT16ToFloatError()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 2 but was 1"));
    }

    [Test]
    public void TestSimpleUINT32ToFloat()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT32, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        Assert.That(dc.Accept([0, 0, 0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 0, 0, 1]), Is.EqualTo("0,5"));
        Assert.That(dc.Accept([0xFF, 0xFF, 0xFF, 0xFF]), Is.EqualTo("2147483647,5"));
    }

    [Test]
    public void TestUINT32ToFloatError()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT32, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0, 0, 0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 4 but was 3"));
    }

    [Test]
    public void TestSimpleINT32ToFloat()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT32, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        Assert.That(dc.Accept([0, 0, 0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 0, 0, 1]), Is.EqualTo("0,5"));
        Assert.That(dc.Accept([0xFF, 0xFF, 0xFF, 0xFF]), Is.EqualTo("-0,5"));
    }

    [Test]
    public void TestINT32ToFloatError()
    {
        FloatDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT32, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 0.5, 0);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0, 0, 0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 4 but was 3"));
    }
}
