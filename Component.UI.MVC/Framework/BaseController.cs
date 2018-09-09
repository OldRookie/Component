using Component.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Framework
{
    [LocalizationAttribute]
    [UserAuthorizeAttribute]
    public class BaseController : Controller
    {
        public void SetErrorMsgs(ResponseResultBase response)
        {
            var msgs = new List<string>();
            ModelState.Values.Where(x => x.Errors.Count > 0).Select(x => x.Errors).ToList()
                    .ForEach(x =>
                    {
                        msgs.AddRange(x.Select(y => y.ErrorMessage).ToList());
                    });
            response.ErrorMessages = msgs;
        }
    }
}