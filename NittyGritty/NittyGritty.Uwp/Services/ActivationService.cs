﻿using NittyGritty.Uwp.Services.Activation;
using NittyGritty.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NittyGritty.Uwp.Services
{
    public class ActivationService
    {
        private readonly Func<Task> initialization;
        private readonly Lazy<UIElement> shell;
        private readonly Lazy<Frame> navigationContext;
        private readonly IEnumerable<IActivationHandler> handlers;
        private readonly Lazy<DefaultActivationHandler> defaultHandler;
        private readonly Func<Task> startup;

        public ActivationService(Func<Task> initialization,
                                 Lazy<UIElement> shell,
                                 Lazy<Frame> navigationContext,
                                 IEnumerable<IActivationHandler> handlers,
                                 Lazy<DefaultActivationHandler> defaultHandler,
                                 Func<Task> startup)
        {
            this.initialization = initialization;
            this.handlers = handlers;
            this.defaultHandler = defaultHandler ?? throw new ArgumentNullException(nameof(defaultHandler), "Default activation handler can not be null");
            this.shell = shell;
            this.startup = startup;
        }

        public ActivationService(INittyGrittyApp app) : this(
            app.Initialization,
            new Lazy<UIElement>(app.CreateShell),
            new Lazy<Frame>(app.GetNavigationContext),
            app.GetActivationHandlers(),
            new Lazy<DefaultActivationHandler>(app.GetDefaultHandler),
            app.Startup)
        {
        }

        public async Task ActivateAsync(object args)
        {
            var activationHandler = handlers?.FirstOrDefault(h => h.CanHandle(args));

            // Default activation and ALL Normal activations (exclude Background, Picker and Custom activations)
            if (activationHandler == null || activationHandler.Strategy == ActivationStrategy.Normal)
            {
                // Initialize things like registering background task before the app is loaded
                await initialization?.Invoke();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = shell?.Value ?? new Frame();
                }
            }
            //  Share Target, File Open Picker, File Save Picker, Contact Panel activation
            else if(activationHandler.Strategy == ActivationStrategy.Picker)
            {
                Window.Current.Content = new Frame();
            }

            // Background activations do not need UI so we are not going to initialize the current Window's content
            // NewView and Other activations must handle their own UI logic in their implementations (HandleAsync)

            if (activationHandler != null)
            {
                if(activationHandler.NeedsNavigationContext)
                {
                    activationHandler.SetNavigationContext(navigationContext.Value);
                }
                await activationHandler.HandleAsync(args);
            }
            else
            {
                await defaultHandler?.Value.HandleAsync(args);
            }

            if(args is IActivatedEventArgs)
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Default activation and All normal activations (exclude Background, Picker and Custom activations)
                if (activationHandler == null || activationHandler.Strategy == ActivationStrategy.Normal)
                {
                    // Tasks after activation
                    await startup?.Invoke();
                }
            }
        }
    }
}