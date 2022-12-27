using Api.Configs;
using Api.Models.Push;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PushSharp.Common;
using PushSharp.Google;
using System.Text;

namespace Api.Services
{
    public class GooglePushService
    {
        private const int MaxPayloadLength = 2048;
        private const int MaxAndroidPayloadLength = 4096;
        private readonly List<string> _messages;
        private readonly PushConfig.GoogleConfig _config;

        public GooglePushService(IOptions<PushConfig> config)
        {
            _messages = new List<string>();
            if (config.Value.Google == null)
            {
                throw new ArgumentNullException("Google configuration not found");
            }

            _config = config.Value.Google;
        }


        public List<string> SendNotification(string pushToken, PushModel model)
        {
            _messages.Clear();

            var config = new GcmConfiguration(_config.ServerKey);
            config.GcmUrl = _config.GcmUrl;

            var gcmBroker = new GcmServiceBroker(config);
            gcmBroker.OnNotificationFailed += GcmBroker_OnNotificationFailed;
            gcmBroker.OnNotificationSucceeded += GcmBroker_OnNotificationSucceeded;

            gcmBroker.Start();
            var jdata = CreateDataMessage(model.CustomData);
            var notify = new GcmNotification
            {
                RegistrationIds = new List<string> { pushToken },
                Data = jdata,
                Notification = CreateMessage(model.Alert),
                ContentAvailable = jdata["data"] != null,
            };
            gcmBroker.QueueNotification(notify);
            gcmBroker.Stop();


            return _messages;
        }

        private JObject CreateDataMessage(Dictionary<string, object>? customData)
        {
            var jData = new JObject();
            var jcustomData = new JObject();
            if (customData != null)
            {
                jcustomData = JObject.FromObject(customData);
            }
            jData["data"] = jcustomData;
            return jData;

        }

        private JObject CreateMessage(PushModel.AlertModel alert)
        {
            var jNotify = new JObject();
            if (!string.IsNullOrWhiteSpace(alert.Title))
                jNotify["title"] = alert.Title;
            if (!string.IsNullOrWhiteSpace(alert.Body))
            {
                jNotify["body"] = alert.Body;

                var curPayloadLength = Encoding.UTF8.GetBytes(jNotify.ToString(Newtonsoft.Json.Formatting.None)).Length;
                if (curPayloadLength > MaxAndroidPayloadLength)
                {
                    var dif = curPayloadLength - MaxPayloadLength + 3;
                    jNotify["body"] = alert.Body.Length - dif <= 0 ? null : alert.Body[..^dif] + "...";
                }
            }

            return jNotify;
        }

        private void GcmBroker_OnNotificationSucceeded(GcmNotification notification)
        {
            _messages.Add("An alert push has been successfully sent!");
        }

        private void GcmBroker_OnNotificationFailed(GcmNotification notification, AggregateException exception)
        {
            exception.Handle(ex =>
            {
                if (ex is GcmNotificationException gcmNotificationException)
                {
                    var gcmNotification = gcmNotificationException.Notification;
                    _messages.Add($"Unable to send notification to {gcmNotification.Tag} : ID={gcmNotification.MessageId}. Exception:{ex}");
                }
                else if (ex is GcmMulticastResultException multicastException)
                {
                    foreach (var succeededNotification in multicastException.Succeeded)
                        _messages.Add($"Notification for '{succeededNotification.Tag}' has been sent");
                    foreach (var failedNotification in multicastException.Failed)
                        _messages.Add($"Unable to send notification to {failedNotification.Key.Tag} : ID={failedNotification.Key.MessageId}. Exception:{ex}");
                }
                else if (ex is DeviceSubscriptionExpiredException expiredException)
                {
                    _messages.Add($"Unable to send notification. Token is expired. Exception:{ex}");
                }
                else if (ex is RetryAfterException retryException)
                {
                    _messages.Add($"Rate Limited, try to send after {retryException.RetryAfterUtc} UTC. Exception: {ex}");
                }
                else
                {
                    _messages.Add($"Unexpected error:{ex}");
                }
                return true;
            });

        }
    }
}
