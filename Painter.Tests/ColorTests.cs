using Painter.Drawing;

namespace Painter.Tests;

[TestClass]
public sealed class ColorTests
{
    [TestMethod]
    public void TestColorFromHexReturnsExpectedValue()
    {
        Assert.AreEqual(new Color(255, 255, 255), Color.FromHex("#FFFFFF"));
    }
    
    [TestMethod]
    public void TestColorFromHexWithLowercaseReturnsExpectedValue()
    {
        Assert.AreEqual(new Color(147, 26, 26), Color.FromHex("#931a1a"));
    }
    
    [TestMethod]
    public void TestColorFromHexMixLowercaseReturnsExpectedValue()
    {
        Assert.AreEqual(new Color(147, 26, 26), Color.FromHex("#931A1a"));
    }
    
    [TestMethod]
    public void TestColorFromHexNoPoundSignReturnsExpectedValue()
    {
        Assert.AreEqual(new Color(147, 26, 26), Color.FromHex("931A1a"));
    }
    
    [TestMethod]
    public void TestColorFromInvalidHexThrows()
    {
        Assert.Throws<FormatException>(() => Color.FromHex("931A1p"));
    }
    
    [TestMethod]
    public void TestColorFromShortHexThrows()
    {
        Assert.Throws<FormatException>(() => Color.FromHex("93"));
    }
    
    [TestMethod]
    public void TestColorFromHsvRedReturnExpectedValue()
    {
        Assert.AreEqual(Color.Red, Color.FromHsv(0, 1, 1));
    }
    
    [TestMethod]
    public void TestColorFromHsvGreenReturnExpectedValue()
    {
        Assert.AreEqual(Color.Green, Color.FromHsv(120, 1, 1));
    }
    
    [TestMethod]
    public void TestColorFromHsvBlueReturnExpectedValue()
    {
        Assert.AreEqual(Color.Blue, Color.FromHsv(240, 1, 1));
    }
    
    [TestMethod]
    public void TestColorFromHsvReturnExpectedValue()
    {
        Assert.AreEqual(Color.IndianRed, Color.FromHsv(0, 0.551, 0.804));
    }
    
    [TestMethod]
    public void TestColorFromHsvMaxHueDoesntThrow()
    {
        try
        {
            Color.FromHsv(359.999999999999, 1, 1);
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }
    
    [TestMethod]
    public void TestColorFromHsvThrowsWhenOutOfHueRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHsv(360, 1, 1));
    }
    
    [TestMethod]
    public void TestColorFromHsvThrowsWhenOutOfSaturationRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHsv(0, 1.1, 1));
    }
    
    [TestMethod]
    public void TestColorFromHsvThrowsWhenOutOfHueValueRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHsv(0, 1, 1.1));
    }
}