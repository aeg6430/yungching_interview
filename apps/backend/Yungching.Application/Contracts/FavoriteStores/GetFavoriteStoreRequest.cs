using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Contracts.FavoriteStores
{
    public class GetFavoriteStoreRequest
    {
        [Required]
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
    }
}
