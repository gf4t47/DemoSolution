namespace Repository.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class DemoTests
{
    [DataTestMethod]
    [DataRow(1, 2, 3)]
    [DataRow(0, 0, 0)]
    [DataRow(100, -100, 0)]
    public void TestAdd(int l, int r, int expected)
    {
        var actual = l + r;
        Assert.AreEqual(expected, actual);
    }
}