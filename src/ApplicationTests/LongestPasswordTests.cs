using Application;

namespace ApplicationTests
{
    [TestClass]
    public class LongestPasswordTests
    {
        [TestMethod]
        [DataRow("test 5 a0A pass007 ?xy1", 7)]
        [DataRow("oFboVpwS7A Js6NYDEUZI Sq84eov91Q Dwt2dNJ4ZX bC4NNZZ62s G6Eoe0dbfF Um_pan4N7d", -1)]
        public void GivenListOfPasswordsThenEvaluateAssertion(string s, int assert)
        {
            var sut = new LongestPassword();
            var result = sut.Solution(s);

            Assert.AreEqual(assert, result);
        }
    }
}