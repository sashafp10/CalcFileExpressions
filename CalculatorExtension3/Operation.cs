using System;

namespace CalculatorExtension3
{
    public class Operation: IEquatable<Operation>
    {
        public Operation(string sign, int priority)
        {
            Sign = sign;
            Priority = priority;
        }

        public string Sign { get; }
        public int Priority { get; }

        public bool Equals(Operation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Sign == other.Sign;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Operation) obj);
        }

        public override int GetHashCode()
        {
            return (Sign != null ? Sign.GetHashCode() : 0);
        }
    }
}
