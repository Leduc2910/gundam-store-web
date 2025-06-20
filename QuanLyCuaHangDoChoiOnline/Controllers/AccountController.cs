using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using System.Threading.Tasks;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Repositories;

namespace QuanLyCuaHangDoChoiOnline.Controllers
{

    public class AccountController : Controller
    {

        private List<string> role = new List<string>();

        public ActionResult Index()
        {
            dynamic dy = new ExpandoObject();
            var account = AccountRes.GetAll();
            dy.account = account;
            return View(dy);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Account account, string returnUrl)
        {
            if (account.UserName == null)
            {
                ViewBag.ErrorMessage = "Vui lòng nhập tên tài khoản";
                return View(account);
            }
            else if (account.Password == null)
            {
                ViewBag.ErrorMessage = "Vui lòng nhập password";
                return View(account);
            }

            var result = AccountRes.CheckLogin(account.UserName, account.Password);
            if (result != null)
            {
                if (result.Authority == 0)
                {
                    role.Add("Customer");
                }
                else if (result.Authority == 1)
                {
                    role.Add("Customer");
                    role.Add("Admin");
                }
                else if (result.Authority == 2)
                {
                    role.Add("Customer");
                    role.Add("Admin");
                    role.Add("SuperAdmin");
                }
                if (!string.IsNullOrEmpty(result.UserName))
                {
                    ClaimsIdentity userIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                    userIdentity.AddClaim(new Claim(ClaimTypes.Name, result.FullName));
                    foreach (var r in role)
                    {
                        userIdentity.AddClaim(new Claim(ClaimTypes.Role, r));
                    }
                    userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.UserName));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    if (returnUrl == "/Order/AddToCart")
                    {
                        return Redirect("/Toy");
                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect(" / ");
                    }
                }
            }
            ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng";
            return View(account);
        }
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Account account, IFormCollection collection)
        {
            try
            {
                if (account.UserName == null)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập tên tài khoản";
                    return View();
                }
                else if (account.Password == null)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập mật khẩu";
                    return View();
                }
                else if (account.Password.Length < 6)
                {
                    ViewBag.ErrorMessage = "mật khẩu ít nhất 6 ký tự";
                    return View();
                }
                else if (account.FullName == null)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập họ tên";
                    return View();
                }
                else if (account.Age == 0)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập tuổi hoặc tuổi không được nhỏ hơn 0";
                    return View();
                }
                else if (account.Phone == null)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập số điện thoại";
                    return View();
                }
                else if (account.Address == null)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập địa chỉ";
                    return View();
                }
                else if (account.Email == null)
                {
                    ViewBag.ErrorMessage = "vui lòng nhập email";
                    return View();
                }
                Account acc = AccountRes.GetAccountWithUser(account.UserName);
                if (acc.UserName != null)
                {
                    ViewBag.ErrorMessage = "tài khoản này đã tồn tại";
                    return View();
                }
                acc = AccountRes.GetAccountWithEmail(account.Email);
                if (acc.Email != null)
                {
                    ViewBag.ErrorMessage = "email này đã tồn tại";
                    return View();
                }
                AccountRes.Account_Create(account);
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }
    }
}
