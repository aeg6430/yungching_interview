using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Jwt
{
    public class JwtTokenPayload
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
