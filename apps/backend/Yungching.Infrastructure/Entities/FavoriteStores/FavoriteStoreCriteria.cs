using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Infrastructure.Entities.FavoriteStores
{
    public class FavoriteStoreCriteria
    {
        public Guid AppUserId { get; set; }
        public Guid StoreId { get; set; }
    }
}
