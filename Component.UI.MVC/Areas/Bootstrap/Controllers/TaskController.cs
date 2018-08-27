using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap.Controllers
{
    public class TaskController : Controller
    {
        // GET: Bootstrap/Task
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Context()
        {
            Response.Write(" Context action invoked.<hr/><ul>");

            Response.Write($"<li> Current HttpContext in <strong> controller context </strong> is { System.Web.HttpContext.Current.ToString() }</li>  ");

            //first we save the current SC


            //instead of awaiting on Task.Run(WriteHttpContext()), we use a wrapper
            //the SC is therefore persisted both within the awaited and awaiter
            await Task.Factory.StartNew(() =>
            {
                var context = System.Web.HttpContext.Current;
                Response.Write($"<li>AttachedToParent Current HttpContext in <strong> awaited Task context</strong> is " +
                $"{ (context == null ? "null" : context.ToString()) }" +
                $"</li>");
            }, TaskCreationOptions.AttachedToParent);
            await Task.Factory.StartNew(() =>
            {
                var context = System.Web.HttpContext.Current;
                Response.Write($"<li> FromCurrentSynchronizationContext Current HttpContext in <strong> awaited Task context</strong> is " +
                $"{ (context == null ? "null" : context.ToString()) }" +
                $"</li>");
            }, System.Threading.CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());

            var sc = System.Threading.SynchronizationContext.Current;
            await Task.Factory.StartNew(() =>
            {
                sc.Post((state) =>
                {
                    var context = System.Web.HttpContext.Current;
                    Response.Write($"<li>Post Context Current HttpContext in <strong> awaited Task context</strong> is " +
                    $"{ (context == null ? "null" : context.ToString()) }" +
                    $"</li>");
                }, null);
            });
            await Task.Factory.StartNew(() =>
            {
                var context = System.Web.HttpContext.Current;
                Response.Write($"<li>Current HttpContext in <strong> awaited Task context</strong> is " +
                $"{ (context == null ? "null" : context.ToString()) }" +
                $"</li>");
            });
            //await Task.Run(() =>
            //{

            //    var context = System.Web.HttpContext.Current;
            //    Response.Write($"<li>Current HttpContext in <strong> awaited Task context</strong> is " +
            //    $"{ (context == null ? "null" : context.ToString()) }" +
            //    $"</li>");
            //    //WriteHttpContext();
            //    //sc.Post((state) =>
            //    //{
            //    //    WriteHttpContext().Invoke();
            //    //}, null);
            //}).ConfigureAwait(true);

            Response.Write($"<li> Current HttpContext in <strong> controller context </strong> is { System.Web.HttpContext.Current.ToString() }</li>  ");


            Response.Write($"</ul><hr/> Response complete.");
            return new HttpStatusCodeResult(200);
        }

        private Action WriteHttpContext()
        {
            return delegate ()
            {
                var context = System.Web.HttpContext.Current;
                Response.Write($"<li>Current HttpContext in <strong> awaited Task context</strong> is " +
                $"{ (context == null ? "null" : context.ToString()) }" +
                $"</li>");
            };
        }

        private async Task AyncWriteHttpContext()
        {
            await Task.Factory.StartNew(() =>
            {
                var context = System.Web.HttpContext.Current;
                Response.Write($"<li>Current HttpContext in <strong> awaited Task context</strong> is " +
                $"{ (context == null ? "null" : context.ToString()) }" +
                $"</li>");
            }, System.Threading.CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}