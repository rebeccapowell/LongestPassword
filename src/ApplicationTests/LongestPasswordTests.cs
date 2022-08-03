using Application;

namespace ApplicationTests
{
    [TestClass]
    public class LongestPasswordTests
    {
        [TestMethod]
        [DataRow(null, -1)]
        [DataRow("", -1)]
        [DataRow("pass007", -1)]
        [DataRow("test 5 a0A pass007 ?xy1", 7)]
        [DataRow("oFboVpwS7A Js6NYDEUZI Sq84eov91Q Dwt2dNJ4ZX bC4NNZZ62s G6Eoe0dbfF Um_pan4N7d", -1)]
        public void GivenListOfPasswordsWhenSolutionThenEvaluateAssertion(string s, int assert)
        {
            var sut = new LongestPassword();
            var result = sut.Solution(s);

            Assert.AreEqual(assert, result);
        }

        [TestMethod]
        [DataRow("test 5 a0A pass007 ?xy1", 7)]
        [DataRow("oFboVpwS7A Js6NYDEUZI Sq84eov91Q Dwt2dNJ4ZX bC4NNZZ62s G6Eoe0dbfF Um_pan4N7d", -1)]
        public void GivenListOfPasswordsWhenRegexThenEvaluateAssertion(string s, int assert)
        {
            var sut = new LongestPassword();
            var result = sut.SolutionRegExWithMax(s);

            Assert.AreEqual(assert, result);
        }

        [TestMethod]
        [DataRow("test 5 a0A pass007 ?xy1", 7)]
        [DataRow("oFboVpwS7A Js6NYDEUZI Sq84eov91Q Dwt2dNJ4ZX bC4NNZZ62s G6Eoe0dbfF Um_pan4N7d", -1)]
        public void GivenListOfPasswordsWhenIsLetterOrDigitThenEvaluateAssertion(string s, int assert)
        {
            var sut = new LongestPassword();
            var result = sut.SolutionIsLetterOrDigit(s);

            Assert.AreEqual(assert, result);
        }

        [TestMethod]
        [DataRow("test 5 a0A pass007 ?xy1", 7)]
        [DataRow("oFboVpwS7A Js6NYDEUZI Sq84eov91Q Dwt2dNJ4ZX bC4NNZZ62s G6Eoe0dbfF Um_pan4N7d", -1)]
        public void GivenListOfPasswordsWhenStrictIsLetterOrDigitThenEvaluateAssertion(string s, int assert)
        {
            var sut = new LongestPassword();
            var result = sut.SolutionStrictIsLetterOrDigit(s);

            Assert.AreEqual(assert, result);
        }

        [TestMethod]
        [DataRow("test 5 a0A pass007 ?xy1", 7)]
        [DataRow("oFboVpwS7A Js6NYDEUZI Sq84eov91Q Dwt2dNJ4ZX bC4NNZZ62s G6Eoe0dbfF Um_pan4N7d", -1)]
        public void GivenListOfPasswordsWhenRegexWithOrderedSelectThenEvaluateAssertion(string s, int assert)
        {
            var sut = new LongestPassword();
            var result = sut.SolutionRegexWithOrderedSelect(s);

            Assert.AreEqual(assert, result);
        }
    }
}