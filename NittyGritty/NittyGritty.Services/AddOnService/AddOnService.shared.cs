﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty.Platform.Store;

namespace NittyGritty.Services
{
    public partial class AddOnService : IAddOnService, IConfigurable<AddOn>
    {
        #region IConfigurable
        private readonly Dictionary<string, AddOn> _addOnsByKey = new Dictionary<string, AddOn>();

        public void Configure(string key, AddOn value)
        {
            lock (_addOnsByKey)
            {
                if (_addOnsByKey.ContainsKey(key))
                {
                    throw new ArgumentException($"This key is already used: {key}");
                }

                if (_addOnsByKey.Any(p => p.Value == value))
                {
                    throw new ArgumentException(
                        "This add-on is already configured with key " + _addOnsByKey.First(p => p.Value == value).Key);
                }

                _addOnsByKey.Add(
                    key,
                    value);
            }
        }

        public string GetKeyForValue(AddOn value)
        {
            lock (_addOnsByKey)
            {
                if (_addOnsByKey.ContainsValue(value))
                {
                    return _addOnsByKey.FirstOrDefault(p => p.Value == value).Key;
                }
                else
                {
                    throw new ArgumentException($"The add-on '{value.Id}' is unknown by the AddOnService.");
                }
            }
        }
        #endregion

        public async Task<ConsumableAddOn> GetConsumableAddOn(string key)
        {
            return await PlatformGetConsumableAddOn(key);
        }

        public async Task<ReadOnlyCollection<ConsumableAddOn>> GetConsumableAddOns(params string[] keys)
        {
            return await PlatformGetConsumableAddOns(keys);
        }

        public async Task<DurableAddOn> GetDurableAddOn(string key)
        {
            return await PlatformGetDurableAddOn(key);
        }

        public async Task<ReadOnlyCollection<DurableAddOn>> GetDurableAddOns(params string[] keys)
        {
            return await PlatformGetDurableAddOns(keys);
        }

        public async Task<SubscriptionAddOn> GetSubscriptionAddOn(string key)
        {
            return await PlatformGetSubscriptionAddOn(key);
        }

        public async Task<ReadOnlyCollection<SubscriptionAddOn>> GetSubscriptionAddOns(params string[] keys)
        {
            return await PlatformGetSubscriptionAddOns(keys);
        }

        public async Task<UnmanagedConsumableAddOn> GetUnmanagedConsumableAddOn(string key)
        {
            return await PlatformGetUnmanagedConsumableAddOn(key);
        }

        public async Task<ReadOnlyCollection<UnmanagedConsumableAddOn>> GetUnmanagedConsumableAddOns(params string[] keys)
        {
            return await PlatformGetUnmanagedConsumableAddOns(keys);
        }

        public async Task<bool> IsDurableActive(string key)
        {
            return await PlatformIsDurableActive(key);
        }

        public async Task<bool> IsSubscriptionActive(string key)
        {
            return await PlatformIsSubscriptionActive(key);
        }

        public async Task Purchase(string key)
        {
            await PlatformPurchase(key);
        }

        public async Task Purchase(AddOn addOn)
        {
            await PlatformPurchase(addOn);
        }

        public async Task<string> ReportUnmanagedConsumableFulfillment(string key)
        {
            return await PlatformReportUnmanagedConsumableFulfillment(key);
        }

        public async Task<string> UpdateConsumableBalance(string key, uint quantity)
        {
            return await PlatformUpdateConsumableBalance(key, quantity);
        }
    }
}