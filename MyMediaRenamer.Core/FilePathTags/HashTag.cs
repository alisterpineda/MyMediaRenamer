using System;
using System.Collections.Generic;
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

    public class HashTag : BaseTag
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
        public HashTag(string tagOptionsString = null) : base(tagOptionsString)
        {
        }
        #endregion

        #region Properties

        public Algorithm Algorithm { get; set; } = Algorithm.MD5;
        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            HashTag other = obj as HashTag;
            if (other == null)
                return false;
            return Algorithm.Equals(other.Algorithm);
        }

        public override string ToString()
        {
            return $"HashTag :: Algorithm='{Algorithm}'" + GetBasePartialToString();
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
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
