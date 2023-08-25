
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using OneSignal.RestAPIv3.Client;
using OneSignal.RestAPIv3.Client.Resources.Notifications;

namespace tyenda_backend.App.Services.Notification_Service
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void SendNotificationAsync(string recipientId, string? title, string? message, string? route)
        {
            var appId = _configuration["OneSignal:AppId"];
            var apiId = _configuration["OneSignal:ApiId"];
            var baseUrl = _configuration["Urls:Frontend"];
            
            if (!String.IsNullOrEmpty(appId) && !String.IsNullOrEmpty(apiId))
            {
                var client = new OneSignalClient(apiId);

                var options = new NotificationCreateOptions
                {
                    AppId = new Guid(appId),
                    Headings = new Dictionary<string, string?> { { "en", title } },
                    Contents = new Dictionary<string, string?> { { "en", message } },
                    Url = baseUrl+route,
                    Filters = new List<INotificationFilter>()
                    {
                        new NotificationFilterField()
                        {
                            Field = NotificationFilterFieldTypeEnum.Tag,
                            Key = "account_id",
                            Relation = "=",
                            Value = recipientId
                        }
                    }
                };
                await client.Notifications.CreateAsync(options);
   
            }
            else
            {
                throw new Exception("App or Api Id should not be empty!");
            }
        }

        public async void SendNotificationToAllAsync(string? title, string? message, string? route)
        {
            var appId = _configuration["OneSignal:AppId"];
            var apiId = _configuration["OneSignal:ApiId"];
            var baseUrl = _configuration["Urls:Frontend"];
            
            if (!String.IsNullOrEmpty(appId) && !String.IsNullOrEmpty(apiId))
            {
                
                var client = new OneSignalClient(apiId);
                
                var options = new NotificationCreateOptions
                {
                    AppId = new Guid(appId),
                    Headings = new Dictionary<string, string?> { { "en", title } },
                    Contents = new Dictionary<string, string?> { { "en", message } },
                    Url = baseUrl+route,
                    IncludedSegments = new List<string>() {"Total Subscriptions"}
                };
                await client.Notifications.CreateAsync(options);
            }
            else
            {
                throw new Exception("App or Api Id should not be empty!");
            }

        }
    }
}