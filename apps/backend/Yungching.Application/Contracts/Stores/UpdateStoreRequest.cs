using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Contracts.Stores
{
    public class UpdateStoreRequest
    {
        public Guid StoreId { get; set; }
        public string? Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public string? BusinessHours { get; set; }
    }
}
