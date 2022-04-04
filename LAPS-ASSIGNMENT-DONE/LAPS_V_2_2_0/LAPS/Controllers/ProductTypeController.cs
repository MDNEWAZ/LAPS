using Azolution.Entities.HumanResource;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace LAPS.Controllers
{
    public class ProductTypeController: Controller
    {
        private readonly IProductRepository _productRepository = new ProductService();


        public ActionResult Product()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Products/ProductsSettings.cshtml");
            }
            return RedirectToAction("Logoff", "Home");

        }
        public ActionResult GetAllProductType()
        {
            try
            {
                var data = _productRepository.GetAllProductType();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AddProduct(Products product)
        {
            var data = _productRepository.AddProduct(product);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductSummary(GridOptions options)
        {
            var data = _productRepository.GetProductSummery(options);
            return Json(data);
        }
        public ActionResult GetProductModelSummary(GridOptions options, int id)
        {

            var studentList = _productRepository.GetProductModelSummary(options, id);
            return Json(studentList, JsonRequestBehavior.AllowGet);
        }
    }
}