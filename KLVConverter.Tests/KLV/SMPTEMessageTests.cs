using System.Numerics;
using KLVConverter.KLV;

namespace KLVConverter.Tests;

public class SMPTEMessageTests
{
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void TestAddKlvDataWithoutWarnings()
    {
        SMPTEMessage message = new([0,1,2,3]);
        KLVData data = new()
        {
            Key = 1,
            Length = 1,
            Value = [0]
        };
        message.AddKLVData(data);
        Dictionary<int, KLVData> datas = message.GetDatas();
        Assert.That(datas, Has.Count.EqualTo(1));
        Assert.That(datas[1], Is.EqualTo(data));
        Assert.That(message.HasWarnings(), Is.EqualTo(false));
        Assert.That(message.GetWarnings(), Has.Count.EqualTo(0));
    }

    [Test]
    public void TestAddKlvDataWithWarnings()
    {
        SMPTEMessage message = new([0,1,2,3]);
        KLVData data = new()
        {
            Key = 1,
            Length = 1,
            Value = [0]
        };
        message.AddKLVData(data);
        message.AddKLVData(data);
        Dictionary<int, KLVData> datas = message.GetDatas();
        Assert.That(datas, Has.Count.EqualTo(1));
        Assert.That(datas[1], Is.EqualTo(data));
        Assert.That(message.HasWarnings(), Is.EqualTo(true));
        Assert.That(message.GetWarnings(), Has.Count.EqualTo(1));
        Assert.That(message.GetWarnings()[0], Is.EqualTo("Key 1 already exist for this message"));
    }
}