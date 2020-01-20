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
        public ActionResult Logar([System.Web.Http.FromBody] User user)
        {
            string userData = JsonConvert.SerializeObject(user);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                (
                1, user.Name, DateTime.Now, DateTime.Now.AddYears(1), true, userData
                );

            string enTicket = FormsAuthentication.Encrypt(authTicket);

            var cookie = new HttpCookie("Cookie1", enTicket);
            Response.AppendCookie(cookie);

            return Json("Logado", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Logout()
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(authCookie);
                FormsAuthentication.SignOut();
                return Json("Logout ! ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Nenhum usuário logado!", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult JLoged()
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if(authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var usuario = Request.Cookies["Cookie1"] != null ? JsonConvert.DeserializeObject<User>(authTicket.UserData) : null;
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Nenhum usuário logado!", JsonRequestBehavior.AllowGet);
            }
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

        private bool HasUser(string email)
        {
            bool hasUser = false;
            var users = userContext.Listar();
            var user = users.Where(x => x.Email == email).FirstOrDefault();
            hasUser = (user == null ? false : true);
            return hasUser;
        }
    }
}