using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Utilities;

namespace LAPS.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult MenuSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }



        //These two line is called for work with audit trail
        IAuditHendler hendler = new IAuditHendler();
        AuditTrailDataService aService = new AuditTrailDataService();
        IMenuRepository _menuRepository = new MenuService();
        
        public JsonResult GetMenuSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var menuList = _menuRepository.GetMenuSummary(skip, take, page, pageSize, sort, filter);
            

            return Json(menuList); 
        }

        [HttpGet]
        public JsonResult SelectAllMenu()
        {
            
            IQueryable<Menu> menuList = _menuRepository.SelectAllMenu();
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult SelectAllMenuForSorting()
        {
           
            IQueryable<Menu> menuList = _menuRepository.SelectAllMenu();

            var results = new
            {
                Items = menuList,
                TotalCount = 0
            };

            return Json(results); 
        }

        [HttpPost]
        public string SaveMenu(string strobjMenuInfo)
        {
            var res = "";
            Users user = ((Users)(Session["CurrentUser"]));
            strobjMenuInfo = strobjMenuInfo.Replace("^", "&");
            var menuId = 0;
            try
            {
                var menu = (Menu)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjMenuInfo, typeof(Menu));
               
                menu.MenuPath =RemoveRightSlash(menu.MenuPath);
                res = _menuRepository.SaveMenu(menu);
                menuId = menu.MenuId;
            }
            catch (Exception exception)
            {
                res = exception.Message;
            }

            //Audittail
            var audit = hendler.GetAuditInfo(user.UserId, "Save/Update Menu", menuId, res);
            aService.SendAudit(audit);
            return res;

        }

        private string RemoveRightSlash (string menuPath)
        {
            var resMenuPath = menuPath;
            try
            {
                if (!string.IsNullOrEmpty(menuPath))
                {
                    var index = menuPath.Length - 1;
                    var path = (menuPath.LastIndexOf('/') == index) ? menuPath.Remove(index) : menuPath;
                    resMenuPath = (menuPath == path)? path : RemoveRightSlash(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resMenuPath;
        }
        
        public ActionResult GetMenuByModuleId(int moduleId)
        {
           
            IQueryable<Menu> menuList = _menuRepository.SelectAllMenuByModuleId(moduleId);
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectMenuByUserPermission()
        {
            try
            {
                int userId = 0;
                if (Session["CurrentUser"] != null)
                {
                    Users user = ((Users)(Session["CurrentUser"]));
                    userId = user.UserId;
                   
                    IQueryable<Menu> menuList = _menuRepository.SelectMenuByUserPermission(userId);
                    return Json(menuList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GetToDoList()
        {
            
           
            
            var menuList = _menuRepository.GetToDoList();

            var menu = new Menu();
            menu.MenuId = -1;
            menu.MenuName = "Change Password";
            menu.MenuPath = "../Home/ChangePassword";
            menuList.Add(menu);


            menu = new Menu();
            menu.MenuId = -2;
            menu.MenuName = "Logoff";
            menu.MenuPath = "../Home/Logoff";
            menuList.Add(menu);

            var results = new
            {
                Items = menuList,
                TotalCount = 0
            };
            return Json(results);
        }

        

        public string UpdateMenuSorting(string strobjMenuInfo)
        {
            var res = "";

            Users user = ((Users)(Session["CurrentUser"]));
            try
            {
                

                var menuList = (List<Menu>)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjMenuInfo, typeof(List<Menu>));

                res = _menuRepository.UpdateMenuSorting(menuList);

            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            //Audittail
            var audit = hendler.GetAuditInfo(user.UserId, "Update Menu Sorting", "Update", res);
            aService.SendAudit(audit);
            return res;
        }
        public JsonResult GetParentMenuByMenu(int parentMenuId)
        {
            return Json(_menuRepository.GetParentMenuByMenu(parentMenuId), JsonRequestBehavior.AllowGet);
        }

    }
}
