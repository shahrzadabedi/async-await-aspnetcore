using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CompanyEmployees.Filters
{
    public interface ICompanyLog
    {
        void InitiateService(HttpRequestMessage message);
        Task Log(HttpActionExecutedContext context);
        Task Log(HttpActionContext context);
        void EndService(HttpRequestMessage requestMessage);
    }
    public class CompanyLog : ICompanyLog
    {
        private ILogger Logger;
        public CompanyLog(ILogger logger)
        {
            this.Logger = logger;
        }
        public void InitiateService(HttpRequestMessage message)
        {
            Logger.Debug(message.RequestUri.AbsoluteUri);
            Logger.Debug("Service Started ...");
        }
        public async Task Log(HttpActionContext context)
        {
            var request = context.Request;
            var objectContent = context.Request.Content as ObjectContent;
            string requestString = "";
            if (request.Method == HttpMethod.Post)
            {
                if (objectContent != null)
                {
                    requestString = await objectContent.ReadAsStringAsync();
                }
                if (requestString==null && context.Request.Content != null)
                {
                    using (var stream = new StreamReader(await context.Request.Content.ReadAsStreamAsync()))
                    {
                        stream.BaseStream.Position = 0;
                        requestString = stream.ReadToEnd();
                    }
                }
                Logger.Info("Request is " + requestString);
            }
        }
        public async Task Log(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            var response = context.Response;

            if (response == null && exception != null)
            {
                Logger.Error(context.Exception.Message);
            }
            else
            {
                await Log(response);
            }
        }
        public async Task Log(HttpResponseMessage response)
        {
            var objectContent = response.Content as ObjectContent;
            if (response != null && response.Content == null)
            {
                if (response.IsSuccessStatusCode)
                    Logger.Info("OK");
                else
                    Logger.Error(response.StatusCode);

            }
            else
            {
                string responseString = "";
                if (objectContent != null)
                {
                    responseString = await objectContent.ReadAsStringAsync();
                }
                if (responseString==null && objectContent != null)
                {
                    using (var stream = new StreamReader(await response.Content.ReadAsStreamAsync()))
                    {
                        stream.BaseStream.Position = 0;
                        responseString = stream.ReadToEnd();
                    }
                }
                if (objectContent.ObjectType == typeof(HttpError))
                {
                    Logger.Warn(responseString);
                }
                else
                {
                    Logger.Info("Response is " + responseString);
                }
            }
        }

        public void EndService(HttpRequestMessage requestMessage)
        {
            Logger.Debug(requestMessage.RequestUri.AbsoluteUri);
            Logger.Debug("Service Ended");
        }
    }
    public class LoggingFilterAttribute : IAsyncActionFilter
    {
        private readonly ICompanyLog Log;
        public LoggingFilterAttribute()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            this.Log = new CompanyLog(logger);
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
        //public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        //{
        //    Log.InitiateService(actionContext.Request);
        //    await Log.Log(actionContext);
        //}
        //public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //    //string userName = ((System.Security.Principal.GenericPrincipal)Thread.CurrentPrincipal).Identity.Name;
        //    //GlobalDiagnosticsContext.Set("user", userName);

        //    await Log.Log(actionExecutedContext);
        //    Log.EndService(actionExecutedContext.Request);
        //}
    }
}
