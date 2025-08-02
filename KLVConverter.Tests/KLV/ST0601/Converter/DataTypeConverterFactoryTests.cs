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
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.STRING, ST0601Datatype.UINT8)), Is.InstanceOf<StringDataTypeConverter>());
    }

    [Test]
    public void TestInteger()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT8, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT16, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT32, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT64, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.INT8, ST0601Datatype.INT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.INT16, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.INT32, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.INT64, ST0601Datatype.UINT8)), Is.InstanceOf<IntDataTypeConverter>());
    }

    [Test]
    public void TestFloat()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.FLOAT32, ST0601Datatype.UINT8)), Is.InstanceOf<FloatDataTypeConverter>());
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.FLOAT64, ST0601Datatype.UINT8)), Is.InstanceOf<FloatDataTypeConverter>());
    }

    [Test]
    public void TestUnknown()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.LOCALSET, ST0601Datatype.LOCALSET)), Is.InstanceOf<DefaultDatatypeConverter>());
    }
}