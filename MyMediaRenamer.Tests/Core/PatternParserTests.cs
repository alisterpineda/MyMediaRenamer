using System.Collections.Generic;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core
{
    public class PatternParserTests : BaseTestFixture
    {
        public static object[] SingleTagCases =
        {
            new object[] { "<datetime>", new DateTimeTag(),  },
            new object[] { "<datetime format=yyyyMMdd_HHmmss_fff>", new DateTimeTag{Format = "yyyyMMdd_HHmmss_fff" } },
            new object[] { "<hash>", new HashTag() },
            new object[] { "<hash algorithm=md5>", new HashTag() },
            new object[] { "<hash algorithm=sha1>", new HashTag{Algorithm = Algorithm.SHA1} },
            new object[] { "<hash algorithm=sha256>", new HashTag{Algorithm = Algorithm.SHA256} },
            new object[] { "<hash algorithm=sha384>", new HashTag{Algorithm = Algorithm.SHA384} },
            new object[] { "<hash algorithm=sha512>", new HashTag{Algorithm = Algorithm.SHA512} },
            new object[] { "<inc>", new IncrementTag() },
            new object[] { "<inc reference=directory start=101 step=5 format=X>", new IncrementTag{Reference = IncrementReference.Directory, Start = 101, Step = 5, Format = "X"}},
            new object[] { "<meta>", new MetadataTag() },
            new object[] { "<meta Name=ExifOrientation>", new MetadataTag{Name = MetadataName.ExifOrientation} },
            new object[] { "<meta Name=ExifOrientation UseRaw=true>", new MetadataTag{Name = MetadataName.ExifOrientation, UseRaw = true} },
            new object[] { "<name>", new NameTag() },
            new object[] { "<name type=name>", new NameTag() },
            new object[] { "<name type=extension>", new NameTag{Type = NameTagType.Extension} },
            new object[] { "<name type=full>", new NameTag{Type = NameTagType.Full} },
            // Shortcuts
            new object[] {"<ext>", new NameTag{Type = NameTagType.Extension} },
        };

        [TestCaseSource(nameof(SingleTagCases))]
        public void SingleTagTest(string pattern, BaseTag expectedTag)
        {
            List<BaseTag> actualTags = PatternParser.Parse(pattern);
            Assert.AreEqual(1, actualTags.Count);
            Assert.AreEqual(expectedTag, actualTags[0]);
        }

        public static object[] MultiTagOptionsCases =
        {
            new object[] { "<datetime format=yyyyMMdd_HHmmss_fff maxlength=4>", new DateTimeTag{Format = "yyyyMMdd_HHmmss_fff", MaxLength = 4}  }
        };

        [TestCaseSource(nameof(MultiTagOptionsCases))]
        public void MultiTagOptionsTest(string pattern, BaseTag expectedTag)
        {
            List<BaseTag> actualTags = PatternParser.Parse(pattern);
            Assert.AreEqual(1, actualTags.Count);
            Assert.AreEqual(expectedTag, actualTags[0]);
        }

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
