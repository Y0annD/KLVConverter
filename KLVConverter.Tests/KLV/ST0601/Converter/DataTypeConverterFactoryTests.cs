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
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.STRING, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(StringDataTypeConverter)));
    }

    [Test]
    public void TestInteger()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT8, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(IntDataTypeConverter)));
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT16, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(IntDataTypeConverter)));
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT32, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(IntDataTypeConverter)));
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.UINT64, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(IntDataTypeConverter)));
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.INT16, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(IntDataTypeConverter)));
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.INT32, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(IntDataTypeConverter)));
    }

    [Test]
    public void TestFloat()
    {
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.FLOAT32, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(FloatDataTypeConverter)));
        Assert.That(DataTypeConverterFactory.GetConverterForDataType(new ST0601ConverterStructure("", ST0601Datatype.FLOAT64, ST0601Datatype.UINT8)), Is.InstanceOf(typeof(FloatDataTypeConverter)));
    }
}