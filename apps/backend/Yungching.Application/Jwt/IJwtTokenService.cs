using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(JwtTokenPayload payload);
    }
}
