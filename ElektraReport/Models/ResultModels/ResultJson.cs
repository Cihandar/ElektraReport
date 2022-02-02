using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ResultModels
{
    public class ResultJson<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
        public Guid Id { get; set; }
        public T Data { get; set; }
    }
}
