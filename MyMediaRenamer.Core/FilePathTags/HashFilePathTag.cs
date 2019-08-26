using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace MyMediaRenamer.Core.FilePathTags
{
    public enum Algorithm
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public class HashFilePathTag : BaseFilePathTag
    {
        #region Members

        private static readonly Dictionary<Algorithm, HashAlgorithm> hashAlgDictionary = new Dictionary<Algorithm, HashAlgorithm>
        {
            {Algorithm.MD5, new MD5CryptoServiceProvider() },
            {Algorithm.SHA1, new SHA1CryptoServiceProvider() },
            {Algorithm.SHA256, new SHA256CryptoServiceProvider() },
            {Algorithm.SHA384, new SHA384CryptoServiceProvider() },
            {Algorithm.SHA512, new SHA512CryptoServiceProvider() }
        };

        #endregion

        #region Constructors
        public HashFilePathTag(string tagOptionsString = null, MediaRenamer parent = null) : base(tagOptionsString, parent)
        {
        }
        #endregion

        #region Properties

        public Algorithm Algorithm { get; set; } = Algorithm.MD5;
        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            HashFilePathTag other = obj as HashFilePathTag;
            if (other == null)
                return false;
            return Algorithm.Equals(other.Algorithm);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        protected override string GenerateString(MediaFile mediaFile)
        {
            using (var stream = mediaFile.GetStream())
            {
                var hash = hashAlgDictionary[Algorithm].ComputeHash(stream);
                string outString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                return outString;
            }
        }
        #endregion
    }
}
