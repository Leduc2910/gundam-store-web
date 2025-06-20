using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Repositories;

namespace QuanLyCuaHangDoChoiOnline.Controllers
{

    public class ToyController : Controller
    {
        public ActionResult Index(int page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }
            var lstToy = ToyRes.GetAll();
            var lstToyType = ToyTypeRes.GetAllType();
            List<Toy> toys = new List<Toy>();
            int i;
            int toyPerPage = 6;
            for (i = (page - 1) * toyPerPage; i < page * toyPerPage; i++)
            {
                if (lstToy.Count == i)
                {
                    break;
                }
                toys.Add(lstToy[i]);
            }
            int MaxPage = lstToy.Count / toyPerPage;
            int tmp = lstToy.Count % toyPerPage;
            if (tmp >= 1) MaxPage += 1;
            dynamic dy = new ExpandoObject();
            dy.toy = toys;
            dy.toytypeNAV = lstToyType;
            dy.maxpage = MaxPage;
            dy.currentpage = page;
            return View(dy);
        }
        
        public ActionResult ToyType(string ID,int page)
        {
            if(page == null || page<1)
            {
                page = 1;
            }
            var lstToyTypeList = ToyTypeRes.GetAllType();
            var lstToy = ToyRes.GetTypeList(ID);
            List<Toy> toys = new List<Toy>();
            int i;
            int toyPerPage = 6;
            for (i =(page-1)*toyPerPage ; i < page*toyPerPage; i++)
            {
                if(lstToy.Count == i)
                {
                    break;
                }
                toys.Add(lstToy[i]);
            }
            var ToyType = ToyTypeRes.ToyTypeWithID(ID);
            int MaxPage = lstToy.Count / toyPerPage;
            int tmp = lstToy.Count % toyPerPage;
            if (tmp >= 1) MaxPage += 1;
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = lstToyTypeList;
            dy.toytypelist = toys;
            dy.toytype = ToyType;
            dy.maxpage = MaxPage;
            dy.currentpage = page;
            return View(dy);
        }

        public ActionResult Details(string id)
        {
            var toy = ToyRes.ToyWithID(id);
            var lstToy = ToyRes.GetAll();
            List<Toy> toys = new List<Toy>();
            int tr = 0;
            var it = lstToy.Single(r => r.ToyID == toy.ToyID);
            if (lstToy.Remove(it)) { 
                tr = 1;
            }
            Random rnd = new Random();
            for (int i = 1; i <= 3; i++) 
            {
                int random = rnd.Next(lstToy.Count);
                int j = 0;
                foreach (var item in lstToy)
                {
                    if(j == random)
                    {
                        toys.Add(item);
                        lstToy.Remove(item);
                        break;
                    }
                    j++;
                }
            }
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            dy.toydetail = toy;
            dy.lstToy = toys;
            return View(dy);
        }

        public ActionResult ToyName(string name, int page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }
            var lstToyWithName = ToyRes.ToyWithName(name);
            var lstToyType = ToyTypeRes.GetAllType();
            var lstAllToys = ToyRes.GetAll();
            List<Toy> toys = new List<Toy>();
            int i;
            int toyPerPage = 6;
            int MaxPage;
            if (string.IsNullOrWhiteSpace(name))
            {
                for (i = (page - 1) * toyPerPage; i < page * toyPerPage; i++)
                {
                    if (lstAllToys.Count == i)
                    {
                        break;
                    }
                    toys.Add(lstAllToys[i]);
                }
                MaxPage = lstAllToys.Count / toyPerPage;
                int tmp = lstAllToys.Count % toyPerPage;
                if (tmp >= 1) MaxPage += 1;
            } 
            else
            {

                for (i = (page - 1) * toyPerPage; i < page * toyPerPage; i++)
                {
                    if (lstToyWithName.Count == i)
                    {
                        break;
                    }
                    toys.Add(lstToyWithName[i]);
                }
                MaxPage = lstToyWithName.Count / toyPerPage;
                int tmp = lstToyWithName.Count % toyPerPage;
                if (tmp >= 1) MaxPage += 1;
            }

            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = lstToyType; 
            dy.toynamelist = toys;
            dy.maxpage = MaxPage;
            dy.currentpage = page;
            dy.searchName = name;
            if (lstToyWithName.Count == 0 && !string.IsNullOrWhiteSpace(name))
            {
                dy.Message = $"Không tìm thấy sản phẩm có tên <strong>\"{name}\"</strong>. Vui lòng kiểm tra lại.";
            } else
            {
                dy.Message = null;
            }
            return View(dy);
        }

    }
}
