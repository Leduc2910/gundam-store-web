using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Repositories;

namespace QuanLyCuaHangDoChoiOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagerController : Controller
    {
        private readonly IHostingEnvironment he;
        private string userName;
        public ManagerController(IHostingEnvironment e, IHttpContextAccessor httpContextAccessor)
        {
            he = e;
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userName = userId;
        }
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManagerAccount()
        {
            var lstAcc = AccountRes.GetAll();
            return View(lstAcc);
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult DetailAccount(string username)
        {
            var acc = AccountRes.GetAccountWithUser(username);
            return View(acc);
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult EditAccount(string username)
        {
            var acc = AccountRes.GetAccountWithUser(username);
            return View(acc);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(Account account)
        {
            if (account.UserName == null)
            {
                ViewBag.ErrorMessage = "vui lòng nhập tên tài khoản";
                return View(account);
            }
            else if (account.FullName == null)
            {
                ViewBag.ErrorMessage = "vui lòng nhập họ tên";
                return View(account);
            }
            else if (account.Age == 0)
            {
                ViewBag.ErrorMessage = "vui lòng nhập tuổi hoặc tuổi không được nhỏ hơn 0";
                return View(account);
            }
            else if (account.Phone == null)
            {
                ViewBag.ErrorMessage = "vui lòng nhập số điện thoại";
                return View(account);
            }
            else if (account.Address == null)
            {
                ViewBag.ErrorMessage = "vui lòng nhập địa chỉ";
                return View(account);
            }
            else if (account.Email == null)
            {
                ViewBag.ErrorMessage = "vui lòng nhập email";
                return View(account);
            }
            var acc = AccountRes.GetAccountWithUser(account.UserName);
            var lstAccount = AccountRes.GetAll();
            var itemToRemove = lstAccount.Single(r => r.Email == acc.Email);
            lstAccount.Remove(itemToRemove);
            foreach(var i in lstAccount)
            {
                if(i.Email == account.Email)
                {
                    ViewBag.ErrorMessage = "email này đã tồn tại";
                    return View(account);
                }
            }
            AccountRes.Account_Update(account);
            return RedirectToAction("ManagerAccount");
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult DeleteAccount(string id)
        {
            var acc = AccountRes.GetAccountWithUser(id);
            return View(acc);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(string id, IFormCollection collection)
        {
            var lstOrders = OrdersRes.GetOrdersUser(id);
            foreach (var i in lstOrders)
            {
                InfoOrder infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithOrderID(i.OrderID);
                InfoOrderRes.InfoOrder_Delete(infoOrder.InfoOrderID);
                var listOrderItem = OrderItemRes.GetOrderItemsWithOrderID(i.OrderID);
                foreach (var item in listOrderItem)
                {
                    OrderItemRes.deleteOrderItem(item.ItemID);
                }
                OrdersRes.Orders_Delete(i.OrderID);
            }
            AccountRes.Account_Delete(id);
            return RedirectToAction("ManagerAccount");
        }

        public ActionResult ManagerToy()
        {
            var lstToy = ToyRes.GetAll();
            return View(lstToy);
        }
        public ActionResult DetailsToy(string id)
        {
            var toy = ToyRes.ToyWithID(id);
            dynamic dy = new ExpandoObject();
            dy.toy = toy;
            dy.toyType = ToyTypeRes.ToyTypeWithID(toy.ToyTypeID);
            return View(dy);
        }
        public ActionResult EditToy(string id)
        {
            var toy = ToyRes.ToyWithID(id);
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            dy.toy = toy;
            return View(dy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditToy(string id,Toy toy, IFormCollection collection, IFormFile Image)
        {
            try
            {
                if (toy.ToyName == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng không để trống tên";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    return View(dy);
                }
                else if (toy.ToyTypeID == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng chọn loại đồ chơi";
                    dynamic dy = new ExpandoObject();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    return View(dy);
                }
                else if (toy.Brand == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng không để trống nhà sản xuất";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    return View(dy);
                }
                else if (toy.Description == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng không để trống mô tả đồ chơi";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    return View(dy);
                }
                else if (toy.Price == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng không để trống giá bán";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    return View(dy);
                }
                else if (toy.Quantity == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng không để trống số lượng";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    return View(dy);
                }
                else if (toy.OrderedQuantity == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng không để trống lượng đặt hàng";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    dy.toy = ToyRes.ToyWithID(toy.ToyID);
                    return View(dy);
                }
                toy.ToyID = id;
                if(Image == null)
                {
                    toy.Image = ToyRes.ToyWithID(id).Image;
                    ToyRes.Toy_Update(toy);
                }
                else
                {
                    var fileName = Path.Combine(he.WebRootPath + "/images", Path.GetFileName(Image.FileName));
                    Image.CopyTo(new FileStream(fileName, FileMode.Create));
                    toy.Image = Image.FileName;
                    ToyRes.Toy_Update(toy);
                }
                return RedirectToAction("ManagerToy");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult CreateToy()
        {
            dynamic dy = new ExpandoObject();
            dy.toy = new Toy();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            return View(dy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateToy(Toy toy,IFormFile Image)
        {
            try
            {
                if (toy.ToyName == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập tên";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    return View(dy);
                }
                else if(toy.ToyTypeID == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng chọn loại đồ chơi";
                    dynamic dy = new ExpandoObject();
                    dy.toy = new Toy();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    return View(dy);
                }
                else if(toy.Brand == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập tên nhà sản xuất";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    return View(dy);
                }
                else if (toy.Description == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập mô tả";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    return View(dy);
                }
                else if (toy.Price == null)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập giá bán";
                    dynamic dy = new ExpandoObject();
                    dy.toytypeNAV = ToyTypeRes.GetAllType();
                    return View(dy);
                }
                Random rnd = new Random();
                int id = rnd.Next(10000, 99999);
                while (ToyRes.ToyWithID(id.ToString()) != null)
                {
                    id = rnd.Next(10000, 99999);
                }
                var fileName = Path.Combine(he.WebRootPath+"/images", Path.GetFileName(Image.FileName));
                Image.CopyTo(new FileStream(fileName, FileMode.Create));
                toy.ToyID = id.ToString();
                toy.Image = Image.FileName;
                ToyRes.Toy_CreateToy(toy);
                return RedirectToAction("ManagerToy");
            }
            catch
            {
                string name = toy.ToyName;
                return View();
            }
        }
        public ActionResult DeleteToy(string id)
        {
            var toy = ToyRes.ToyWithID(id);
            return View(toy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteToy(string id, IFormCollection collection, string cancelButton)
        {
            try
            {
                if (cancelButton != null)
                {
                    return RedirectToAction("ManagerToy");
                }
                ToyRes.Toy_Delete(id);
                return RedirectToAction("ManagerToy");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ManagerOrder()
        {
            var lstInfoOrder = InfoOrderRes.InfoOrder_GetAll();
            return View(lstInfoOrder);
        }
        public ActionResult DeleteOrder(string id)
        {
            dynamic dy = new ExpandoObject();
            var infoOrder =  InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            dy.infoOrder = infoOrder;
            var orderItem = OrderItemRes.GetOrderItemsWithOrderID(infoOrder.OrderID);
            dy.orderItem = orderItem;
            List<Toy> lstToy = new List<Toy>();
            foreach(var i in orderItem)
            {
                lstToy.Add(ToyRes.ToyWithID(i.ToyID));
            }
            dy.lstToy = lstToy;
            return View(dy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder(string id, IFormCollection collection)
        {
            var infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            var orderItem = OrderItemRes.GetOrderItemsWithOrderID(infoOrder.OrderID);
            foreach(var i in orderItem)
            {
                OrderItemRes.deleteOrderItem(i.ItemID);
            }
            InfoOrderRes.InfoOrder_Delete(id);
            OrdersRes.Orders_Delete(infoOrder.OrderID);

            return RedirectToAction("ManagerOrder");
        }

        public ActionResult DetailOrder(string id)
        {
            dynamic dy = new ExpandoObject();
            var infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            dy.infoOrder = infoOrder;
            var orderItem = OrderItemRes.GetOrderItemsWithOrderID(infoOrder.OrderID);
            dy.orderItem = orderItem;
            List<Toy> lstToy = new List<Toy>();
            foreach (var i in orderItem)
            {
                lstToy.Add(ToyRes.ToyWithID(i.ToyID));
            }
            dy.lstToy = lstToy;
            return View(dy);
        }
        public ActionResult EditOrder(string id)
        {
            dynamic dy = new ExpandoObject();
            var infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            dy.infoOrder = infoOrder;
            var orderItem = OrderItemRes.GetOrderItemsWithOrderID(infoOrder.OrderID);
            dy.orderItem = orderItem;
            List<Toy> lstToy = new List<Toy>();
            foreach (var i in orderItem)
            {
                lstToy.Add(ToyRes.ToyWithID(i.ToyID));
            }
            dy.lstToy = lstToy;
            return View(dy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(InfoOrder infoOrder)
        {
            InfoOrderRes.InfoOrder_Update(infoOrder);
            return RedirectToAction("ManagerOrder");
        }
        public ActionResult InfoOrderComplete(string id)
        {
            var infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            infoOrder.Status = 1;
            InfoOrderRes.InfoOrder_Update(infoOrder);
            return RedirectToAction("ManagerOrder");
        }
        public ActionResult InfoOrderIncomplete(string id)
        {
            var infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            infoOrder.Status = 0;
            InfoOrderRes.InfoOrder_Update(infoOrder);
            return RedirectToAction("ManagerOrder");
        }
    }
}
