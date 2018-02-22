﻿using ChatBot.Server.Extensions;
using ChatBot.Server.Models;
using ChatBot.Server.Services.SearchService;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Server.LUIS
{
    [Serializable]
    [LuisModel("c382651a-e936-49de-bafc-e131bd030e05", "81d08cadc86c493bba26ff425e8693de")]
    public class ChatDialog : LuisDialog<object>
    {
        [Serializable]
        public class PartialMessage
        {
            public string Text { set; get; }
        }

        private PartialMessage message;

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var msg = await item;
            this.message = new PartialMessage { Text = msg.Text };
            await base.MessageReceived(context, item);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var response = ChatResponse.Default;

            await context.PostAsync(await response.ToUserLocale(context));

            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            var response = ChatResponse.Greeting;

            await context.PostAsync(await response.ToUserLocale(context));

            context.Wait(MessageReceived);
        }

        [LuisIntent("Question")]
        public async Task Question(IDialogContext context, LuisResult result)
        {
            var response = ChatResponse.Question;

            var resultJson = SearchQueryService.SearchQueryId(message.Text);
            var resultQuery = JsonConvert.DeserializeObject<SearchApiResult>(resultJson);
            await context.PostAsync(await response.ToUserLocale(context));
            
            context.Wait(MessageReceived);
        }

        [LuisIntent("Error")]
        public async Task Error(IDialogContext context, LuisResult result)
        {
            var response = ChatResponse.Error;

            await context.PostAsync(await response.ToUserLocale(context));

            context.Wait(MessageReceived);
        }

        [LuisIntent("Thanks")]
        public async Task Farewell(IDialogContext context, LuisResult result)
        {
            var response = ChatResponse.Farewell;

            await context.PostAsync(await response.ToUserLocale(context));

            context.Wait(MessageReceived);
        }
    }
}