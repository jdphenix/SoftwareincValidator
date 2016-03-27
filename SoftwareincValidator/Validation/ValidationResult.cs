using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Validation
{
    public class ValidationResult : EventArgs, IEquatable<ValidationResult>
    {
        public static bool operator ==(ValidationResult left, ValidationResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ValidationResult left, ValidationResult right)
        {
            return !left.Equals(right);
        }

        public string Message { get; }
        public ValidationLevel Level { get; }
        public ValidationSource Source { get; }

        public ValidationResult(
            string message, 
            ValidationLevel level = ValidationLevel.Error, 
            ValidationSource source = ValidationSource.Undefined)
        {
            Message = message;
            Level = level; 
            Source = source;
        }

        public bool Equals(ValidationResult other)
        {
            return string.Equals(Message, other.Message) && Level == other.Level && Source == other.Source;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ValidationResult && Equals((ValidationResult)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Message?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (int)Level;
                hashCode = (hashCode * 397) ^ (int)Source;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Level}: {Message}";
        }
    }
}