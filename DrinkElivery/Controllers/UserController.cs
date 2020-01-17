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
        private UserContext userContext = new UserContext();
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

        public ActionResult JRegister(User user)
        {
            if(HasUser(user.Name)== true) { return Json("Usuário ja existe", JsonRequestBehavior.AllowGet); }
            try
            {
                userContext.Inserir(user);
                return Json("Inserido com sucesso!" + user.Name , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JHasUser(string name)
        {
            try
            {
                var user = (HasUser(name) == false ? "Válido" : "Inválido");
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult JListar()
        {
            UserContext userContext = new UserContext();
            var users = userContext.Listar();
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        private bool HasUser(string name)
        {
            bool hasUser = false;
            var users = userContext.Listar();
            var user = users.Where(x => x.Name == name).FirstOrDefault();
            hasUser = (user == null ? false : true);
            return hasUser;
        }
    }
}