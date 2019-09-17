using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMediaRenamer.Core.FilePathTags
{
    public abstract class BaseTag
    {
        #region Constructors

        protected BaseTag(string tagOptionsString = null)
        {
            foreach (var option in ParseTagOptionsString(tagOptionsString))
            {
                PropertyInfo property = this.GetType().GetProperty(option.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new ArgumentException($"Invalid option key: '{option.Key}'");

                if (property.PropertyType.IsEnum)
                {
                    if (GetType().BaseType.GetTypeInfo().GetDeclaredMethod("SetEnumProperty")
                        .MakeGenericMethod(property.PropertyType)
                        .Invoke(this, new object[] {property, option.Value}) is bool validEnum)
                    {
                        if (!validEnum)
                            throw new ArgumentException($"Invalid option value: '{option.Value}'");
                    }
                    else
                        throw new InvalidOperationException();
                }
                else if (property.PropertyType == typeof(int?))
                {
                    property.SetValue(this, int.Parse(option.Value));
                }
                else if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(this, bool.Parse(option.Value));
                }
                else if (property.PropertyType == typeof(string))
                {
                    property.SetValue(this, option.Value);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        #endregion

        #region Properties

        public int? MaxLength { get; set; } = null;

        #endregion

        #region Methods

        public abstract override string ToString();
        protected abstract string GenerateString(Renamer renamer,  MediaFile mediaFile);

        public override bool Equals(object obj)
        {
            if (!(obj is BaseTag other))
                return false;

            return MaxLength.Equals(other.MaxLength);
        }

        public string GetString(Renamer renamer, MediaFile mediaFile)
        {
            string generatedString = GenerateString(renamer, mediaFile);

            if (MaxLength.HasValue)
                generatedString = generatedString.Substring(0, MaxLength.GetValueOrDefault());

            return generatedString;
        }

        protected string GetBasePartialToString()
        {
            var str = new StringBuilder();

            if (MaxLength.HasValue)
                str.Append($",MaxLength='{MaxLength}'");

            return str.ToString();
        }

        private Dictionary<string, string> ParseTagOptionsString(string tagOptions)
        {
            if (string.IsNullOrEmpty(tagOptions))
                return new Dictionary<string, string>();

            return tagOptions.Split('/')
                .Select(x => x.Split('='))
                .ToDictionary(x => x[0], x => x[1]);

        }

        private bool SetEnumProperty<T>(PropertyInfo property, string stringValue) where T : struct
        {
            bool validEnum = Enum.TryParse(stringValue, true, out T enumVal);
            if (validEnum)
                property.SetValue(this, enumVal);
            return validEnum;
        }

        #endregion
    }
}
