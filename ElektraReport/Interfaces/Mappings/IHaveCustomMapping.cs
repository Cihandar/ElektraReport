using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ElektraReport.Interfaces.Mappings
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configration);
    }
}
