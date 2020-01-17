using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Common.Utils
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string Perfil { get; set; }
        public string Sistema { get; set; }
        public string Modulo { get; set; }
        public string Interface { get; set; }
        public string TipoFuncao { get; set; }


        private CustomPrincipal GetUser(HttpContextBase httpContext)
        {
            HttpCookie authCookie = httpContext.Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);

                var principal = new CustomPrincipal(authTicket.Name)
                {
                    UserId = serializeModel.UserId,
                    FirstName = serializeModel.Name,
                    LastName = serializeModel.Password
                };

                return principal;
            }
            return null;
        }
    }
}
