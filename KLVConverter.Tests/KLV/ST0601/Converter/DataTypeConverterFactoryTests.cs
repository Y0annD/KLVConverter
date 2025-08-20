using System.Collections.Generic;

namespace KLVConverter.KLV.ST0601.Converter;

public class DataTypeConverterFacoryTests()
{

    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void TestString()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(string), ST0601Datatype.STRING)), Is.InstanceOf<StringDataTypeConverter>());
    }

    [Test]
    public void TestInteger()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(byte), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(ushort), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(uint), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(ulong), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(sbyte), ST0601Datatype.INT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(short), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(int), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(long), ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
    }

    [Test]
    public void TestFloat()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(float), ST0601Datatype.UINT8)), Is.InstanceOf<FloatDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(double), ST0601Datatype.UINT8)), Is.InstanceOf<FloatDataTypeConverter>());
    }

    [Test]
    public void TestUnknown()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(Dictionary<string, string>), ST0601Datatype.LOCALSET)), Is.InstanceOf<DefaultDatatypeConverter>());
    }

    [Test]
    public void TestIMAPB()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(float), ST0601Datatype.IMAPB)), Is.InstanceOf<IMAPBDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", typeof(double), ST0601Datatype.IMAPB)), Is.InstanceOf<IMAPBDataTypeConverter>());
    }
}