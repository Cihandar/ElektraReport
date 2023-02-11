using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ResultModels
{
    public class ResultJson2
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
        public Guid Id { get; set; }

    }
}
