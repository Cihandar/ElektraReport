using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
