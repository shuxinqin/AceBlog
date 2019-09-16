using Ace.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ace;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AceBlog.Web.Common
{
    public abstract class WebController<TService> : WebController
    {
        TService _service;
        protected TService Service
        {
            get
            {
                if (this._service == null)
                    this._service = this.CreateService<TService>();

                return this._service;
            }
        }
    }

    public abstract class WebController : BaseController
    {
        UserSession _session;
        public UserSession CurrentSession
        {
            get
            {
                if (this._session != null)
                    return this._session;

                if (this.HttpContext.User.Identity.IsAuthenticated == false)
                    return null;

                UserSession session = UserSession.Parse(this.HttpContext.User);
                this._session = session;
                return session;
            }
            set
            {
                UserSession session = value;

                if (session == null)
                {
                    //注销登录
                    this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }

                List<Claim> claims = session.ToClaims();

                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(30),
                    IsPersistent = false,
                    AllowRefresh = false
                });

                this._session = session;
            }
        }


        protected ActionResult InternalError()
        {
            this.Response.StatusCode = 500;
            return View("~/Views/Shared/Error.cshtml");
        }
        protected ActionResult NotFoundView()
        {
            this.Response.StatusCode = 404;
            return View("~/Views/Shared/NotFound.cshtml");
        }
    }
}