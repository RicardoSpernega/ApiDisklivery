using Common.Models;
using DrinkElivery.Context;
using DrinkElivery.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DrinkElivery.Controllers
{
    public class UserController : Controller
    {
        // GET: Teste
        public ActionResult Logar(User user)
        {
            var userModel = new CustomSerializeModel()
            {
                UserId = user.UserId,
                Name = user.Name,
                Password = user.Password,
            };

            string userData = JsonConvert.SerializeObject(userModel);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                (
                1, user.Name, DateTime.Now, DateTime.Now.AddMinutes(20), true, userData
                );

            string enTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie("Cookie1", enTicket)
            {
                Domain = ConfigurationManager.AppSettings["Default"],
                Expires = DateTime.Now.AddMinutes(20)
            };
            Response.Cookies.Add(faCookie);
            return Json("ola", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register(User user)
        {
            try
            {
                UserContext userContext = new UserContext();
                userContext.Inserir(user);
                return Json("Inserido com sucesso!" + user.Name , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        

        [HttpGet]
        public ActionResult Listar()
        {
            UserContext userContext = new UserContext();
            var users = userContext.Listar();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

    }
}