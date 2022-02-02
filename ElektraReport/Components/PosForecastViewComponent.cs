using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Interfaces.ElektraApis;
namespace ElektraReport.Components
{
    public class PosForecastViewComponent : ViewComponent
    {
        IApiRequest _apiRequest;
        public PosForecastViewComponent(IApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public    IViewComponentResult  Invoke(Guid CompanyId,string date)
        {

            var result = _apiRequest.GetPosForecast(CompanyId, DateTime.Parse(date), DateTime.Parse(date));
            return View("Default", result.Result);
        }
    }
}
