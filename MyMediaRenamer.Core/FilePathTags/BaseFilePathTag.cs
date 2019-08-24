﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMediaRenamer.Core.FilePathTags
{
    public abstract class BaseFilePathTag
    {
        #region Constructors

        protected BaseFilePathTag(string tagOptionsString = null, MediaRenamer parent = null)
        {
            Parent = parent;

            foreach (var option in ParseTagOptionsString(tagOptionsString))
            {
                PropertyInfo property = this.GetType().GetProperty(option.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new ArgumentException($"Invalid option key: '{option.Key}'");

                if (property.PropertyType.IsEnum)
                {
                    if (GetType().BaseType.GetTypeInfo().GetDeclaredMethod("SetEnumProperty")
                        .MakeGenericMethod(property.PropertyType)
                        .Invoke(this, new object[] {property, option.Value}) is bool test)
                    {
                        if (!test)
                            throw new ArgumentException($"Invalid option value: '{option.Value}'");
                    }
                    else
                        throw new InvalidOperationException();
                }
                else if (property.PropertyType == typeof(int))
                {
                    property.SetValue(this, int.Parse(option.Value));
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

        protected MediaRenamer Parent { get; }

        public int MaxLength { get; set; }

        #endregion

        #region Methods

        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();
        public abstract override string ToString();
        protected abstract string GenerateString(MediaFile mediaFile);

        public string GetString(MediaFile mediaFile)
        {
            string generatedString = GenerateString(mediaFile);

            return generatedString;
        }

        private Dictionary<string, string> ParseTagOptionsString(string tagOptions)
        {
            if (string.IsNullOrEmpty(tagOptions))
                return new Dictionary<string, string>();

            return tagOptions.Split(';')
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
