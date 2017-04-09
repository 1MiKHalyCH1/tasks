using System.IO;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonConversion
{
    public class Tests
    {
        [Test]
        [TestCase("1.v2.json", "1.v3.json")]
        [TestCase("2.v2.json", "2.v3.json")]
        public void JsonTestV1(string v2, string v3)
        {
            var jsonV2 = "";
            using (StreamReader sr = new StreamReader(v2))
            {
                jsonV2 = sr.ReadToEnd();
            }
            JObject jsonV3 = new JObject();
            using (StreamReader sr = new StreamReader(v3))
            {
                jsonV3 = JObject.Parse(sr.ReadToEnd());
            }

            JObject resultJson = JsonProgram.ConverJson(jsonV2);
            Assert.AreEqual(jsonV3, resultJson);
        }
    }
}