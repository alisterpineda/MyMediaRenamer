using System;
using System.Collections.Generic;
using System.Text;

namespace MyMediaRenamer.Core
{
    public class PatternInvalidException : Exception
    {
        #region Constructors

        public PatternInvalidException() { }

        public PatternInvalidException(string message) : base(message) { }

        public PatternInvalidException(string message, Exception inner) : base(message, inner) { }

        #endregion
    }
}
