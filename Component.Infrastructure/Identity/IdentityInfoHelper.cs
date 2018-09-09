using Component.Infrastructure.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Component.Infrastructure
{
    public class IdentityInfoHelper
    {
        public static object syncOjb = new object();
        public static IdentityInfo CurrentIdentityInfo
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items.Contains("IdentityInfo"))
                    {
                        return HttpContext.Current.Items["IdentityInfo"] as IdentityInfo;
                    }
                    else
                    {
                        IdentityInfo identityInfo = null;
                        lock (syncOjb)
                        {
                            if (!HttpContext.Current.Items.Contains("IdentityInfo"))
                            {
                                try
                                {
                                    var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                                    FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);
                                    identityInfo = JsonConvert.DeserializeObject<IdentityInfo>(Ticket.UserData.GZipDecompressString());
                                    HttpContext.Current.Items["IdentityInfo"] = identityInfo;
                                }
                                catch (Exception)
                                { }
                            }
                        }
                        return identityInfo;
                    }
                }
                else
                {
                    return new IdentityInfo()
                    {
                        Id = new Guid("E449C7B9-2205-419D-90BD-40D0442DCC3A")
                    };
                }
            }
        }
    }

    public class IdentityInfo
    {

        public IdentityInfo()
        {
            Roles = new List<string>();
            Pemission = new List<string>();
        }
        public string UserName { get; set; }

        public Guid Id { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Pemission { get; set; }
    }
}
