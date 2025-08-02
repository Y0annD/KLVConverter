using KLVConverter.KLV.ST0601.Converter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace KLVConverter.Tests.KLV.ST0601.Converter;

public class IMAPBConverterTests
{
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void SimpleIMAPB3ByteTest()
    {
        IMAPBDataTypeConverter converter = new(0, 1500000);
        Assert.That(converter.Accept([0x0, 0xD9, 0x2A]), Is.EqualTo(Convert.ToString(13898.5)));
        IMAPBDataTypeConverter converter1 = new(-900, 40000);
        Assert.That(converter1.Accept([0x2F, 0x92, 0x1E]), Is.EqualTo(Convert.ToString(23456.234375)));
    }

    [Test]
    public void SimpleIMAPB2ByteTest()
    {
        IMAPBDataTypeConverter converter2 = new(-1000, 1000);
        Assert.That(converter2.Accept([0x3E, 0x90]), Is.EqualTo(Convert.ToString(1)));
    }

    [Test]
    public void SpecialValuesTest()
    {
        IMAPBDataTypeConverter converter2 = new(-900, 19000);
        Assert.That(converter2.Accept([0xC8, 0x00, 0x00]), Is.EqualTo("Infinity"));
        Assert.That(converter2.Accept([0xE8, 0x00, 0x00]), Is.EqualTo("-Infinity"));
        Assert.That(converter2.Accept([0xD0, 0x00, 0x00]), Is.EqualTo("NaN"));
        Assert.That(converter2.Accept([0xD4, 0x00, 0x00]), Is.EqualTo("NaN"));
        Assert.That(converter2.Accept([0xD8, 0x00, 0x00]), Is.EqualTo("NaN"));
        Assert.That(converter2.Accept([0xF8, 0x00, 0x00]), Is.EqualTo("NaN"));
        Assert.That(converter2.Accept([0xE0, 0x00, 0x00]), Is.EqualTo("MISB Special Value"));
        Assert.That(converter2.Accept([0xC0, 0x00, 0x00]), Is.EqualTo("User defined"));
    }
}