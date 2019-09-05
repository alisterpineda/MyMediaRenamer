using System;
using System.Collections.Generic;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core
{
    public class PatternParserTests : BaseTestFixture
    {
        public static object[] NTagsCases =
        {
            new object[] {"", new List<BaseTag>() },
            new object[] {"abc", new List<BaseTag>
            {
                new TextTag(){Text = "abc"}
            } },
            new object[] {"<hash>", new List<BaseTag>
            {
                new HashTag()
            } }
        };

        [TestCaseSource(nameof(NTagsCases))]
        public void NTagsTest(string pattern, List<BaseTag> expectedFilePathTags)
        {
            List<BaseTag> actualFilePathTags = PatternParser.Parse(pattern);

            Assert.AreEqual(expectedFilePathTags.Count, actualFilePathTags.Count);

            for (int i = 0; i < expectedFilePathTags.Count; i++)
            {
                Assert.AreEqual(expectedFilePathTags[i], actualFilePathTags[i]);
            }
        }


        public static object[] InvalidPatternCases =
        {
            "<", ">", "<>", "<hash>>", "<<hash>", "<<>>", "<invalidToken>"
        };

        [TestCaseSource(nameof(InvalidPatternCases))]
        public void InvalidPatternTest(string pattern)
        {
            Assert.That(() => PatternParser.Parse(pattern), Throws.Exception);
        }
    }
}
