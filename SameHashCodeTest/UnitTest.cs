namespace SameHashCodeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void get_random_string_with_given_length_should_return_string_with_same_length()
        {
            var length = 3;
            Assert.IsTrue((Program.GetRandomString(length)).Result.Length == length);
        }

        [TestMethod]
        public void get_same_hash_string_should_return_different_string_with_same_hash()
        {
            var s = "IO";
            Assert.IsTrue(Program.GetRandomString(s.Length).Result.GetHashCode().Equals(Program.GetSameHashDifferentString(s).Result));
        }
    }
}