using NUnit.Framework;

namespace Komodo.Core.Tests
{
    [TestFixture]
    public class CalculatorTest
    {
        [Test]
        public void ShouldAddTwoNumbers()
        {
            ICalculator sut = new Calculator();
            int expectedResult = sut.Add(7, 8);
            Assert.That(expectedResult, Is.EqualTo(15));
        }

        [Test]
        public void ShouldMulTwoNumbers()
        {
            ICalculator sut = new Calculator();
            int expectedResult = sut.Mul(7, 8);
            Assert.That(expectedResult, Is.EqualTo(56));
        }

        [Test]
        public void JsonVerify()
        {
            var jsonExpected = "{\"pop\":\"popvalue\"}";
            var jsonActual = "{\"pop\":\"popvalue\"}";
            Assert.IsTrue(JsonHelper.CompareJson(jsonExpected, jsonActual));
        }

        [Test]
        public void JsonVerifyFail()
        {
            var jsonExpected = "{\"pop\":\"popvaluee\"}";
            var jsonActual = "{\"pop\":\"popvalue\"}";
            Assert.IsFalse(JsonHelper.CompareJson(jsonExpected, jsonActual));
        }

        [Test]
        [TestCase("pop2")]
        [TestCase("$..pop2")]
        public void JsonVerifyIgnoreFieldsFail(string ignoreFields)
        {
            var jsonExpected = "{\"pop\":\"popvalue\", \"pop2\":\"popvalue\"}";
            var jsonActual = "{\"pop\":\"popvalue\", \"pop2\":\"popvaluee\"}";
            Assert.IsTrue(JsonHelper.CompareJson(jsonExpected, jsonActual, ignoreFields));
        }
    }
}