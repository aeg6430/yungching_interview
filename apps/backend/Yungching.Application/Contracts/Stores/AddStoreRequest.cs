using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Application.Contracts.Stores
{
    public class AddStoreRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public string Address { get; set; }
        public string BusinessHours { get; set; }
    }
}
