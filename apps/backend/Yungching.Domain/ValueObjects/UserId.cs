using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Domain.ValueObjects
{
    public readonly struct UserId
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");

            Value = value;
        }

        public static implicit operator Guid(UserId id) => id.Value;
        public override string ToString() => Value.ToString();
    }
}
