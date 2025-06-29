using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yungching.Domain.ValueObjects
{
    public readonly struct Email
    {
        public string Value { get; }

        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public Email(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !EmailRegex.IsMatch(value))
                throw new ArgumentException("Invalid email format.");

            Value = value;
        }

        public static implicit operator string(Email email) => email.Value;
        public override string ToString() => Value;
    }
}
