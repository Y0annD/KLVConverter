using KLVConverter.KLV.ST0601.Converter;

namespace KLVConverter.Tests.KLV.ST0601.Converter;

public class IntDatatypeConverterTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestWithBadInputType()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.STRING, KLVConverter.KLV.ST0601.ST0601Datatype.INT16);

        Assert.That(dc.Accept([0, 0x14]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0xFF, 0xFF]), Is.EqualTo("0"));
    }

    [Test]
    public void TestSimpleUINT8ToUINT8()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT8, KLVConverter.KLV.ST0601.ST0601Datatype.UINT8);

        Assert.That(dc.Accept([0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF]), Is.EqualTo("255"));
    }

    [Test]
    public void TestSimpleINT8ToINT8()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT8, KLVConverter.KLV.ST0601.ST0601Datatype.INT8);

        Assert.That(dc.Accept([0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF]), Is.EqualTo("-1"));
    }

[Test]
    public void TestSimpleINT16ToINT16()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT16, KLVConverter.KLV.ST0601.ST0601Datatype.INT16);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF, 0xFF]), Is.EqualTo("-1"));
    }

    [Test]
    public void TestSimpleUINT16ToUINT16()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF, 0xFF]), Is.EqualTo("65535"));
    }

    [Test]
    public void TestSimpleUINT16ToUINT16WithOffset()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32, 1, 1);

        Assert.That(dc.Accept([0, 0]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0, 1]), Is.EqualTo("2"));
    }

    [Test]
    public void TestUINT16ToUINT16Error()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT16, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 2 but was 1"));
    }


    [Test]
    public void TestINT16ToINT16Error()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT16, KLVConverter.KLV.ST0601.ST0601Datatype.INT16);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 2 but was 1"));
    }

    [Test]
    public void TestSimpleUINT32ToUINT32()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT32, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32);

        Assert.That(dc.Accept([0, 0, 0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 0, 0, 1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF, 0xFF, 0xFF, 0xFF]), Is.EqualTo("4294967295"));
    }

    [Test]
    public void TestUINT32ToUINT32Error()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT32, KLVConverter.KLV.ST0601.ST0601Datatype.UINT32);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0, 0, 0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 4 but was 3"));
    }

    [Test]
    public void TestSimpleINT32ToINT32()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT32, KLVConverter.KLV.ST0601.ST0601Datatype.FLOAT32);

        Assert.That(dc.Accept([0, 0, 0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 0, 0, 1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF, 0xFF, 0xFF, 0xFF]), Is.EqualTo("-1"));
    }

    [Test]
    public void TestINT32ToFINT32Error()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT32, KLVConverter.KLV.ST0601.ST0601Datatype.INT32);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0, 0, 0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 4 but was 3"));
    }

    [Test]
    public void TestSimpleINT64ToINT64()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT64, KLVConverter.KLV.ST0601.ST0601Datatype.INT64);

        Assert.That(dc.Accept([0, 0, 0, 0, 0, 0, 0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 0, 0, 0, 0, 0, 0, 1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]), Is.EqualTo("-1"));
    }

    [Test]
    public void TestINT64ToINT64Error()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.INT64, KLVConverter.KLV.ST0601.ST0601Datatype.INT64);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0, 0, 0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 8 but was 3"));
    }

    [Test]
    public void TestSimpleUINT64ToUINT64()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT64, KLVConverter.KLV.ST0601.ST0601Datatype.UINT64);

        Assert.That(dc.Accept([0, 0, 0, 0, 0, 0, 0, 0]), Is.EqualTo("0"));
        Assert.That(dc.Accept([0, 0, 0, 0, 0, 0, 0, 1]), Is.EqualTo("1"));
        Assert.That(dc.Accept([0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]), Is.EqualTo("18446744073709551615"));
    }

    [Test]
    public void TestUINT64ToUINT64Error()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.UINT64, KLVConverter.KLV.ST0601.ST0601Datatype.UINT64);

        IOException exception = Assert.Throws<IOException>(() => { dc.Accept([0, 0, 0]); });
        Assert.That(exception.Message, Is.EqualTo("Insufficient array length. Expected 8 but was 3"));
    }

    [Test]
    public void TestUnknownInput()
    {
        IntDataTypeConverter dc = new(KLVConverter.KLV.ST0601.ST0601Datatype.IMAPB, KLVConverter.KLV.ST0601.ST0601Datatype.UINT64);

        Assert.That(dc.Accept([0,0,0,0, 0, 0, 0, 0, 0, 0, 1]), Is.EqualTo("0"));
    }
}
