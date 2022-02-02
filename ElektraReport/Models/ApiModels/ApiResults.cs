using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ApiModels
{
    public class ApiResults<T>
    {
        public bool Status { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }

        public List<T> Data { get; set; }
    }
}
