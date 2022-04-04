using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Dashboard.Service.Interface;
using Azolution.Dashboard.Service.Service;
using Azolution.Entities.Core;


namespace LAPS.Controllers
{
    public class GraphAndChartController : Controller
    {

        public JsonResult GetMonthWiseStockData()
        {
            IGraphAndChartRepository _graphAndChartRepository = new GraphAndChartService();
            Users objUser = ((Users)(Session["CurrentUser"]));
            return Json(_graphAndChartRepository.GetMonthWiseStockData(objUser), JsonRequestBehavior.AllowGet);
        }

    }
}
