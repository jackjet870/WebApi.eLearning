﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Learning.Web.Services;
using Newtonsoft.Json.Serialization;

namespace Learning.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Courses",
                routeTemplate: "api/courses/{id}",
                defaults: new {controller="courses", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
             name: "Students",
             routeTemplate: "api/students/{userName}",
             defaults: new { controller = "students", userName = RouteParameter.Optional }
         );
            config.Routes.MapHttpRoute(
            name: "Enrollments",
            routeTemplate: "api/courses/{courseId}/students/{userName}",
            defaults: new { controller = "Enrollments", userName = RouteParameter.Optional }
        );
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //Replace the controller configuration selector
            config.Services.Replace(typeof(IHttpControllerSelector), new LearningControllerSelector((config)));
            //Configure HTTP Caching using Entity Tags (ETags)
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["eLearningConnection"].ConnectionString;
            var eTagStore = new CacheCow.Server.EntityTagStore.SqlServer.SqlServerEntityTagStore(connString);
            var cacheCowCacheHandler = new CacheCow.Server.CachingHandler(eTagStore);
            cacheCowCacheHandler.AddLastModifiedHeader = false;
            config.MessageHandlers.Add(cacheCowCacheHandler);
        }
    }
}
