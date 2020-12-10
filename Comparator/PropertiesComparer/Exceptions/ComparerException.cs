using System;

namespace PropertiesComparer.Exceptions
{
    public abstract class ComparerException : Exception
    {
        public virtual string Code { get; }

        protected ComparerException(string message) : base(message) {}
    }
}
