using System.Text;
using Xunit;

namespace HotChocolate.Language.SyntaxTree;

public class FloatValueNodeTests
{
    [InlineData("1.568")]
    [InlineData("2.0")]
    [Theory]
    public void CreateFloatValue(string value)
    {
        // arrange
        var buffer = Encoding.UTF8.GetBytes(value);

        // act
        var floatValueNode = new FloatValueNode(
            buffer, FloatFormat.FixedPoint);

        // assert
        Assert.Equal(value, floatValueNode.Value);
        Assert.Equal(SyntaxKind.FloatValue, floatValueNode.Kind);
        Assert.Null(floatValueNode.Location);
    }

    [InlineData("1.568")]
    [InlineData("2.0")]
    [Theory]
    public void CreateFloatValueWithLocation(string value)
    {
        // arrange
        var buffer = Encoding.UTF8.GetBytes(value);
        var location = new Location(0, 0, 0, 0);

        // act
        var floatValueNode = new FloatValueNode(
            location, buffer, FloatFormat.FixedPoint);

        // assert
        Assert.Equal(value, floatValueNode.Value);
        Assert.Equal(SyntaxKind.FloatValue, floatValueNode.Kind);
        Assert.Equal(location, floatValueNode.Location);
    }

    [InlineData("1.568", 1.568)]
    [InlineData("2.0", 2.0)]
    [Theory]
    public void ToSingle(string value, float expected)
    {
        // arrange
        var buffer = Encoding.UTF8.GetBytes(value);
        var location = new Location(0, 0, 0, 0);

        // act
        var floatValueNode = new FloatValueNode(
            location, buffer, FloatFormat.FixedPoint);

        // assert
        Assert.Equal(expected, floatValueNode.ToSingle());
    }

    [InlineData("1.568", 1.568)]
    [InlineData("2.0", 2.0)]
    [Theory]
    public void ToDouble(string value, double expected)
    {
        // arrange
        var buffer = Encoding.UTF8.GetBytes(value);
        var location = new Location(0, 0, 0, 0);

        // act
        var floatValueNode = new FloatValueNode(
            location, buffer, FloatFormat.FixedPoint);

        // assert
        Assert.Equal(expected, floatValueNode.ToDouble());
    }

    [InlineData("1.568", 1.568)]
    [InlineData("2.0", 2.0)]
    [Theory]
    public void ToDecimal(string value, decimal expected)
    {
        // arrange
        var buffer = Encoding.UTF8.GetBytes(value);
        var location = new Location(0, 0, 0, 0);

        // act
        var floatValueNode = new FloatValueNode(
            location, buffer, FloatFormat.FixedPoint);

        // assert
        Assert.Equal(expected, floatValueNode.ToDecimal());
    }

    [Fact]
    public void EqualsFloatValueNode()
    {
        // arrange
        var a = new FloatValueNode(1.0);
        var b = new FloatValueNode(1.0);
        var c = new FloatValueNode(3.0);

        // act
        var ab_result = a.Equals(b);
        var aa_result = a.Equals(a);
        var ac_result = a.Equals(c);
        var anull_result = a.Equals(default);

        // assert
        Assert.True(ab_result);
        Assert.True(aa_result);
        Assert.False(ac_result);
        Assert.False(anull_result);
    }

    [Fact]
    public void EqualsFloatValueNode_Float()
    {
        // arrange
        var a = new FloatValueNode((float)1.0);
        var b = new FloatValueNode((float)1.0);
        var c = new FloatValueNode((float)3.0);

        // act
        var ab_result = a.Equals(b);
        var aa_result = a.Equals(a);
        var ac_result = a.Equals(c);
        var anull_result = a.Equals(default);

        // assert
        Assert.True(ab_result);
        Assert.True(aa_result);
        Assert.False(ac_result);
        Assert.False(anull_result);
    }

    [Fact]
    public void EqualsFloatValueNode_Double()
    {
        // arrange
        var a = new FloatValueNode((double)1.0);
        var b = new FloatValueNode((double)1.0);
        var c = new FloatValueNode((double)3.0);

        // act
        var ab_result = a.Equals(b);
        var aa_result = a.Equals(a);
        var ac_result = a.Equals(c);
        var anull_result = a.Equals(default);

        // assert
        Assert.True(ab_result);
        Assert.True(aa_result);
        Assert.False(ac_result);
        Assert.False(anull_result);
    }

    [Fact]
    public void EqualsFloatValueNode_Decimal()
    {
        // arrange
        var a = new FloatValueNode((decimal)1.0);
        var b = new FloatValueNode((decimal)1.0);
        var c = new FloatValueNode((decimal)3.0);

        // act
        var ab_result = a.Equals(b);
        var aa_result = a.Equals(a);
        var ac_result = a.Equals(c);
        var anull_result = a.Equals(default);

        // assert
        Assert.True(ab_result);
        Assert.True(aa_result);
        Assert.False(ac_result);
        Assert.False(anull_result);
    }

    [Fact]
    public void EqualsIValueNode()
    {
        // arrange
        var a = new FloatValueNode(1.0);
        var b = new FloatValueNode(1.0);
        var c = new FloatValueNode(2.0);
        var d = new StringValueNode("foo");

        // act
        var ab_result = a.Equals((IValueNode)b);
        var aa_result = a.Equals((IValueNode)a);
        var ac_result = a.Equals((IValueNode)c);
        var ad_result = a.Equals((IValueNode)d);
        var anull_result = a.Equals(default(IValueNode));

        // assert
        Assert.True(ab_result);
        Assert.True(aa_result);
        Assert.False(ac_result);
        Assert.False(ad_result);
        Assert.False(anull_result);
    }

    [Fact]
    public void EqualsObject()
    {
        // arrange
        var a = new FloatValueNode(1.0);
        var b = new FloatValueNode(1.0);
        var c = new FloatValueNode(2.0);
        var d = "foo";
        var e = 1;

        // act
        var ab_result = a.Equals((object)b);
        var aa_result = a.Equals((object)a);
        var ac_result = a.Equals((object)c);
        var ad_result = a.Equals((object)d);
        var ae_result = a.Equals((object)e);
        var anull_result = a.Equals(default(object));

        // assert
        Assert.True(ab_result);
        Assert.True(aa_result);
        Assert.False(ac_result);
        Assert.False(ad_result);
        Assert.False(ae_result);
        Assert.False(anull_result);
    }

    [Fact]
    public void CompareGetHashCode()
    {
        // arrange
        var a = new FloatValueNode(1.0);
        var b = new FloatValueNode(1.0);
        var c = new FloatValueNode(2.0);

        // act
        var ahash = a.GetHashCode();
        var bhash = b.GetHashCode();
        var chash = c.GetHashCode();

        // assert
        Assert.Equal(ahash, bhash);
        Assert.NotEqual(ahash, chash);
    }

    [Fact]
    public void StringRepresentation()
    {
        // arrange
        var a = new FloatValueNode(1.0);
        var b = new FloatValueNode(2.0);

        // act
        var astring = a.ToString();
        var bstring = b.ToString();

        // assert
        Assert.Equal("1", astring);
        Assert.Equal("2", bstring);
    }

    [Fact]
    public void ClassIsSealed()
    {
        Assert.True(typeof(FloatValueNode).IsSealed);
    }

    [Fact]
    public void Convert_Value_Float_To_Span_To_String()
    {
        // act
        var a = new FloatValueNode(2.5);
        FloatValueNode b = a.WithValue(a.AsSpan(), FloatFormat.FixedPoint);
        var c = b.Value;

        // assert
        Assert.Equal("2.5", c);
    }

    [Fact]
    public void Equals_With_Same_Location()
    {
        var a = new FloatValueNode(
            TestLocations.Location1,
            1.1);
        var b = new FloatValueNode(
            TestLocations.Location1,
            1.1);
        var c = new FloatValueNode(
            TestLocations.Location1,
            1.2);

        // act
        var abResult = a.Equals(b);
        var aaResult = a.Equals(a);
        var acResult = a.Equals(c);
        var aNullResult = a.Equals(default);

        // assert
        Assert.True(abResult);
        Assert.True(aaResult);
        Assert.False(acResult);
        Assert.False(aNullResult);
    }

    [Fact]
    public void Equals_With_Different_Location()
    {
        // arrange
        var a = new FloatValueNode(
            TestLocations.Location1,
            1.1);
        var b = new FloatValueNode(
            TestLocations.Location2,
            1.1);
        var c = new FloatValueNode(
            TestLocations.Location1,
            1.2);

        // act
        var abResult = a.Equals(b);
        var aaResult = a.Equals(a);
        var acResult = a.Equals(c);
        var aNullResult = a.Equals(default);

        // assert
        Assert.True(abResult);
        Assert.True(aaResult);
        Assert.False(acResult);
        Assert.False(aNullResult);
    }

    [Fact]
    public void GetHashCode_With_Location()
    {
        // arrange
        var a = new FloatValueNode(
            TestLocations.Location1,
            1.1);
        var b = new FloatValueNode(
            TestLocations.Location2,
            1.1);
        var c = new FloatValueNode(
            TestLocations.Location1,
            1.2);
        var d = new FloatValueNode(
            TestLocations.Location2,
            1.2);

        // act
        var aHash = a.GetHashCode();
        var bHash = b.GetHashCode();
        var cHash = c.GetHashCode();
        var dHash = d.GetHashCode();

        // assert
        Assert.Equal(aHash, bHash);
        Assert.NotEqual(aHash, cHash);
        Assert.Equal(cHash, dHash);
        Assert.NotEqual(aHash, dHash);
    }
}
