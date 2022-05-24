namespace SameHashCodeTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task get_random_string_with_given_length_should_return_string_with_same_length()
        {
            var length = 100000;
            var result = await RandomStringGenerator.GetRandomString(length);
            Assert.AreEqual(length, result.Length);
        }

        [TestMethod]
        public void get_same_hash_string_should_return_different_string_with_same_hash()
        {
            var s = "IO";
           // Assert.IsTrue(Program.GetRandomString(s.Length).Result.GetHashCode().Equals(Program.GetSameHashDifferentString(s).Result));
        }
    }
}