﻿using NittyGritty.Models;
using NittyGritty.Uwp.Operations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;

namespace NittyGritty.Uwp.Services.Activation
{
    public class ToastNotificationActivationHandler : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        private readonly Dictionary<string, ToastOperation> operations;

        public ToastNotificationActivationHandler(params ToastOperation[] operations) : base(ActivationStrategy.Normal)
        {
            this.operations = new Dictionary<string, ToastOperation>();
            foreach (var operation in operations)
            {
                this.operations.Add(operation.Key, operation);
            }
            Operations = new ReadOnlyDictionary<string, ToastOperation>(this.operations);
        }

        public ReadOnlyDictionary<string, ToastOperation> Operations { get; }

        protected override async Task HandleInternal(ToastNotificationActivatedEventArgs args)
        {
            var query = QueryString.Parse(args.Argument);
            if(query.Count() == 0)
            {
                return;
            }

            if (Operations.TryGetValue(query.FirstOrDefault()?.Name ?? string.Empty, out var operation))
            {
                await operation.Run(args, NavigationContext);
            }
            else
            {
                // We should not reach this part. Please check if you have registered all of the Toast keys this app handles
            };
        }
    }
}
