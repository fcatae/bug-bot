﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BugBot.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        MicrosoftAppCredentials _botCredentials;
        private IEventActivity _eventActivity;

        public EventsController(IOptions<BotFrameworkCredentials> options, IEventActivity eventActivity)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);
            this._eventActivity = eventActivity;
        }

        public string[] Get()
        {
            string[] eventList = null; // _eventActivity.GetAll();            

            return eventList;
        }
        
        [HttpPost("{event_name}", Order = -1)]
        public string Post(string event_name, [FromBody] object obj)
        {
            EventModel eventData = _eventActivity.Get(event_name);

            SendMessage(eventData, obj);

            return "OK";
        }

        [HttpPut("{event_name}", Order = -1)]
        public string Put(string event_name)
        {
            EventModel eventData = _eventActivity.Get(event_name);

            SendMessage(eventData, null);

            return "OK";
        }

        [HttpGet("{event_name}", Order = -1)]
        public string Get(string event_name)
        {
            EventModel eventData = _eventActivity.Get(event_name);

            return $"(Channel: {eventData.serviceUrl}) Template=[{eventData.messageTemplate}]";
        }

        string FormatString(string template, object obj)
        {
            Newtonsoft.Json.Linq.JObject jObject = (Newtonsoft.Json.Linq.JObject)obj;

            return jObject.ToString(template);
        }

        void SendMessage(EventModel eventData, object messageData)
        {
            var client = new ConnectorClient(new Uri(eventData.serviceUrl), _botCredentials);

            var botAccount = new ChannelAccount(name: "BotAccount", id: eventData.botAccount);

            string messageText = (messageData != null) ? FormatString(eventData.messageTemplate, messageData) : eventData.messageTemplate;

            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = botAccount;
            message.Conversation = new ConversationAccount(id: eventData.conversation);
            message.Text = messageText;

            client.Conversations.SendToConversation((Activity)message);
        }
        
    }
}
