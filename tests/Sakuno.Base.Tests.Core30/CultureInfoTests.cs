using System.Globalization;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class CultureInfoTests
    {
        [Fact]
        public static void InvariantCulture()
        {
            var culture = CultureInfo.InvariantCulture;

            Assert.False(culture.IsAncestorOf(culture));
            Assert.False(culture.IsDescendantOf(culture));
        }

        [Fact]
        public static void EnglishCulture()
        {
            var englishCulture = CultureInfo.GetCultureInfo("en");
            var englishInUS = CultureInfo.GetCultureInfo("en-US");
            var englishInUK = CultureInfo.GetCultureInfo("en-UK");

            Assert.True(englishCulture.IsAncestorOf(englishInUS));
            Assert.True(englishInUS.IsDescendantOf(englishCulture));
            Assert.True(englishCulture.IsAncestorOf(englishInUK));
            Assert.True(englishInUK.IsDescendantOf(englishCulture));
        }

        [Fact]
        public static void ChineseCulture()
        {
            var simplifiedChinese = CultureInfo.GetCultureInfo("zh-Hans");
            var chineseInChina = CultureInfo.GetCultureInfo("zh-CN");

            Assert.True(simplifiedChinese.IsAncestorOf(chineseInChina));
            Assert.True(chineseInChina.IsDescendantOf(simplifiedChinese));

            var traditionalChinese = CultureInfo.GetCultureInfo("zh-Hant");
            var chineseInHongkong = CultureInfo.GetCultureInfo("zh-HK");
            var chineseInTaiwan = CultureInfo.GetCultureInfo("zh-TW");

            Assert.True(traditionalChinese.IsAncestorOf(chineseInHongkong));
            Assert.True(chineseInHongkong.IsDescendantOf(traditionalChinese));
            Assert.True(traditionalChinese.IsAncestorOf(chineseInTaiwan));
            Assert.True(chineseInTaiwan.IsDescendantOf(traditionalChinese));
        }
    }
}
