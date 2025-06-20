using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Repositories;

namespace QuanLyCuaHangDoChoiOnline.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private string userName;
        public OrderController(IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userName = userId;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(OrderItem orderItem)
        {
            bool result1 = OrdersRes.checkOrders(userName);
            Orders orders = OrdersRes.GetOrdersUserOnStatus(userName, 1);
            OrderItem item = OrderItemRes.GetOrderItemWithOrderIDToyID(orders.OrderID, orderItem.ToyID);
            if(item != null)
            {
                item.Quantity += orderItem.Quantity;
                item.TotalPrice = item.Quantity * ToyRes.ToyWithID(orderItem.ToyID).Price;
                OrderItemRes.updateOrderItem(item);
            }
            else
            {
                Random rnd = new Random();
                int id = rnd.Next(100000, 999999);
                while (OrderItemRes.GetOrderItemWithID(id.ToString()) != null)
                {
                    id = rnd.Next(100000, 999999);
                }
                orderItem.ItemID = id.ToString();
                orderItem.OrderID = orders.OrderID;
                orderItem.TotalPrice = orderItem.Quantity * ToyRes.ToyWithID(orderItem.ToyID).Price;
                orderItem.Discount = 0;
                OrderItemRes.createOrderItem(orderItem);
            }
            return RedirectToAction("Cart", "Order");
        }
        public ActionResult Cart()
        {

            bool result1 = OrdersRes.checkOrders(userName);
            Orders orders =OrdersRes.GetOrdersUserOnStatus(userName, 1);
            List<OrderItem> lstOrderItem = OrderItemRes.GetOrderItemsWithOrderID(orders.OrderID);
            int totalPrice = 0; 
            List<Toy> lstToy = new List<Toy>();
            if(lstOrderItem.Count !=0)
            {
                foreach (var item in lstOrderItem)
                {
                    totalPrice += item.TotalPrice;
                    orders.OrderPrice = totalPrice;
                    OrdersRes.Orders_Update(orders);
                    lstToy.Add(ToyRes.ToyWithID(item.ToyID));
                }
            }
            else
            {
                orders.OrderPrice = totalPrice;
                OrdersRes.Orders_Update(orders);
            }
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            dy.orderItem = lstOrderItem;
            dy.lstToy = lstToy;
            dy.orders = orders;
            return View(dy);
        }
        public ActionResult Checkout()
        {
            Orders orders = OrdersRes.GetOrdersUserOnStatus(userName, 1);
            if(orders == null)
            {
                return Redirect("/");
            }
            if(orders.OrderPrice == 0)
            {
                return RedirectToAction(nameof(Cart));
            }
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            dy.account = AccountRes.GetAccountWithUser(userName);
            dy.orders = orders;
            dy.totalPrice = orders.OrderPrice + 30000;
            return View(dy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(InfoOrder infoOrder)
        {
            Orders orders = OrdersRes.GetOrdersUserOnStatus(userName, 1);
            List<OrderItem> lstOrderItem = OrderItemRes.GetOrderItemsWithOrderID(orders.OrderID);
            foreach(OrderItem item in lstOrderItem)
            {
                Toy toy = ToyRes.ToyWithID(item.ToyID);
                toy.OrderedQuantity = item.Quantity;
                ToyRes.Toy_Update(toy);
            }
            infoOrder.OrderID = orders.OrderID;
            infoOrder.TotalPrice = orders.OrderPrice + 30000;
            Random rnd = new Random();
            int id = rnd.Next(100000, 999999);
            while (InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id.ToString()) != null)
            {
                id = rnd.Next(100000, 999999);
            }
            infoOrder.InfoOrderID = id.ToString();
            InfoOrderRes.InfoOrder_create(infoOrder);
            orders.OrderStatus = 2;
            OrdersRes.Orders_Update(orders);
            return Redirect("OrdersList");
        }

        public ActionResult OrdersList()
        {
            List<InfoOrder> lstOrder = InfoOrderRes.InfoOrder_GetInfoOrderWithEmail(AccountRes.GetAccountWithUser(userName).Email);
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            dy.lstOrder = lstOrder;
            return View(dy);
        }
        public ActionResult InfoOrderDetail(string id)
        {
            var infoOrder = InfoOrderRes.InfoOrder_GetInfoOrdersWithID(id);
            List<OrderItem> lstOrderItem = OrderItemRes.GetOrderItemsWithOrderID(infoOrder.OrderID);
            List<Toy> lstToy = new List<Toy>();
            foreach (var item in lstOrderItem)
            {
                lstToy.Add(ToyRes.ToyWithID(item.ToyID));
            }
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            dy.orderItem = lstOrderItem;
            dy.lstToy = lstToy;
            dy.infoOrder = infoOrder;
            return View(dy);
        }
        public ActionResult DeleteItem(string id)
        {
            OrderItemRes.deleteOrderItem(id);
            return RedirectToAction("Cart");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(OrderItem orderItem,string add, string sub)
        {
            OrderItem orderItem1 = OrderItemRes.GetOrderItemWithID(orderItem.ItemID);
            int price = orderItem1.TotalPrice / orderItem1.Quantity;
            if (add != null)
            {
                orderItem1.Quantity ++;
                orderItem1.TotalPrice = price * orderItem1.Quantity;
                OrderItemRes.updateOrderItem(orderItem1);
            }
            else if(sub != null)
            {
                orderItem1.Quantity--;
                if(orderItem1.Quantity != 0)
                {
                    orderItem1.TotalPrice = price * orderItem1.Quantity;
                    OrderItemRes.updateOrderItem(orderItem1);
                }
                else
                {
                    OrderItemRes.deleteOrderItem(orderItem1.ItemID);
                }
            }
            return RedirectToAction("Cart");
        }
    }
}
