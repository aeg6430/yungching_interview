using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Domain.ValueObjects
{
    public readonly struct UserName
    {
        public string Value { get; }

        public UserName(string value)
        {
            Value = value?.Trim() ?? string.Empty;
        }

        public static implicit operator string(UserName name) => name.Value;
        public override string ToString() => Value;
    }
}
