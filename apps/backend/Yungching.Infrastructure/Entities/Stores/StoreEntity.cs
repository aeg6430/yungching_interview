using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Infrastructure.Entities.Stores
{
    public class StoreEntity
    {
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string Address { get; set; }
        public string BusinessHours { get; set; }
    }
}
