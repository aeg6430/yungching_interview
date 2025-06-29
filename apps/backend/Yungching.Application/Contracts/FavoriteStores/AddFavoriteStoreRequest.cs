using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Contracts.FavoriteStores
{
    public class AddFavoriteStoreRequest
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid StoreId { get; set; }
    }
}
