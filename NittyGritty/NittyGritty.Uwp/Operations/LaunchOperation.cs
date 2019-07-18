﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace NittyGritty.Uwp.Operations
{
    public class LaunchOperation
    {
        private readonly Func<LaunchActivatedEventArgs, Frame, Task> callback;

        public LaunchOperation(LaunchSource source, Func<LaunchActivatedEventArgs, Frame, Task> callback = null)
        {
            if(source == LaunchSource.Unknown)
            {
                throw new ArgumentException("Launch Operation Type cannot be Unknown");
            }

            Source = source;
            this.callback = callback;
        }

        public LaunchSource Source { get; }

        public virtual async Task Run(LaunchActivatedEventArgs args, Frame frame)
        {
            await callback?.Invoke(args, frame);
        }
    }

    public enum LaunchSource
    {
        Unknown = 0,
        Primary = 2,
        Secondary = 4,
        Jumplist = 8,
        Chaseable = 16,
    }
}
