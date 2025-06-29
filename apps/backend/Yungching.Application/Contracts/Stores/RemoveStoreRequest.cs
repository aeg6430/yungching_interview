using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Contracts.Stores
{
    public class RemoveStoreRequest
    {
        [Required]
        public Guid StoreId { get; set; }
    }
}
