using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Infrastructure.Contexts
{
    public class DapperContext : TransactionContext
    {
        public DapperContext(
            DatabaseSettings settings,
            ILogger<TransactionContext>? logger = null
         )
            : base(settings, logger)
        {
        }
    }
}
