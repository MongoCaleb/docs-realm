﻿using System;
using Realms;
using Realms.Sync;
using MongoDB.Bson;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Realms.Exceptions.Sync;
using Realms.Sync.Exceptions;
using Microsoft.VisualBasic;

namespace Examples
{
    public class FlexibleSyncExamples
    {

        public async Task TestUseFlexibleSync()
        {
            var app = App.Create(Config.FSAppId);
            var user = await app.LogInAsync(Credentials.Anonymous());

            var config = new FlexibleSyncConfiguration(app.CurrentUser!);
            var realm = Realm.GetInstance(config);
            // :snippet-start:get-subscriptions
            var subscriptions = realm.Subscriptions;
            // :snippet-end:
            // :snippet-start:update-subscriptions
            realm.Subscriptions.Update(() =>
            {
                // Subscribe to all long running items, and give the subscription
                // the name 'longRunningItems'
                var longRunningItemsQuery = realm.All<MyTask>()
                    .Where(i => i.Status == "completed" && i.ProgressMinutes > 120);
                realm.Subscriptions
                    .Add(longRunningItemsQuery,
                        new SubscriptionOptions() { Name = "longRunningItems" });

                // Subscribe to all of Ben's Item objects
                realm.Subscriptions.Add(realm.All<MyTask>()
                    .Where(i => i.Owner == "Ben"));

                // Subscribe to all Teams, and give the subscription the
                // name 'teamsSubscription' and throw an error if a new
                // query is added to the team subscription
                realm.Subscriptions.Add(realm.All<Team>(),
                    new SubscriptionOptions()
                    {
                        Name = "teams",
                        UpdateExisting = false
                    });
            });
            // :snippet-end:
            // :snippet-start: wait-for-synchronization
            try
            {
                await realm.Subscriptions.WaitForSynchronizationAsync();
            }
            catch (SubscriptionException ex)
            {
                // Do something in response to the exception or log it
                Console.WriteLine($@"The subscription set's state is Error
                and synchronization is paused:  {ex.Message}");
            }
            // :snippet-end:
            // :snippet-start:update-a-subscription
            realm.Subscriptions.Update(() =>
            {
                var updatedLongRunningItemsQuery = realm
                    .All<MyTask>()
                    .Where(i => i.Status == "completed" && i.ProgressMinutes > 130);
                realm.Subscriptions
                    .Add(updatedLongRunningItemsQuery,
                        new SubscriptionOptions() { Name = "longRunningItems" });
            });
            // :snippet-end:
            // :snippet-start:remove-subscription-by-query
            realm.Subscriptions.Update(() =>
            {
                // remove a subscription by it's query
                var query = realm.All<MyTask>().Where(i => i.Owner == "Ben");
                realm.Subscriptions.Remove(query);
            });
            // :snippet-end:
            // :snippet-start:remove-subscription-by-name
            realm.Subscriptions.Update(() =>
            {
                // remove a named subscription
                var subscriptionName = "longRunningItemsSubscription";
                realm.Subscriptions.Remove(subscriptionName);
            });
            // :snippet-end:
            // :snippet-start:remove-all-subscriptions-of-object-type
            realm.Subscriptions.Update(() =>
            {
                // remove all subscriptions of the "Team" Class Name
                realm.Subscriptions.RemoveAll("Team");

                // Alernatively, remove all subscriptions of the "Team" object type
                realm.Subscriptions.RemoveAll<Team>();
            });
            // :snippet-end:
            // :snippet-start:remove-all-subscriptions
            realm.Subscriptions.Update(() =>
            {
                // remove all subscriptions, including named subscriptions
                realm.Subscriptions.RemoveAll(true);
            });
            // :snippet-end:
        }

        [Test]
        public async Task TestOpenFSRealm()
        {
            var app = App.Create(Config.FSAppId);
            var user = await app.LogInAsync(Credentials.Anonymous(false));
            Realm realm;

            // :snippet-start: open-fs-realm
            // :replace-start: {
            //  "terms": {
            //   "Config.FSAppId": "\"myRealmAppId\"",
            //   "Credentials.Anonymous(false)": "Credentials.Anonymous()"}
            // }
            var config = new FlexibleSyncConfiguration(user)
            {
                PopulateInitialSubscriptions = (realm) =>
                {
                    var allTasks = realm.All<MyTask>();
                    realm.Subscriptions.Add(allTasks,
                        new SubscriptionOptions { Name = "allTasks" });
                }
            };
            try
            {
                realm = await Realm.GetInstanceAsync(config);
                // :remove-start:
                var session = realm.SyncSession;
                Assert.NotNull(session);
                // :remove-end:
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error creating or opening the
                    realm file. {ex.Message}");
            }
            // :replace-end:
            // :snippet-end:
            await user.LogOutAsync();
        }

        [Test]
        public async Task TestOpenFSRealmOffline()
        {
            var app = App.Create(Config.FSAppId);
            // :snippet-start: open-fs-realm-offline
            // :replace-start: {
            //  "terms": {
            //   "Config.FSAppId": "\"myRealmAppId\"",
            //   "Credentials.Anonymous(false)": "Credentials.Anonymous()"
            //  }
            // }

            // :remove-start:
            // Another app.CurrentUser was carrying over into this test
            // causing issues with opening the FS realm. This resolves the
            // test failure but there should probably be stronger cleanup
            // between tests to negate the need for this.
            if (app.CurrentUser != null)
            {
                await app.RemoveUserAsync(app.CurrentUser);
                await app.LogInAsync(Credentials.Anonymous(false));
            };
            User user;
            FlexibleSyncConfiguration config;
            Realm realm;
            // :remove-end:

            if (app.CurrentUser == null)
            {
                // App must be online for user to authenticate
                user = await app.LogInAsync(Credentials.Anonymous(false));
                config = new FlexibleSyncConfiguration(user);
                realm = Realm.GetInstance(config);
                // Go on to add or update subscriptions and use the realm
                // :remove-start:
                var session = realm.SyncSession;
                Assert.NotNull(session);
                // :remove-end:
            }
            else
            {
                // This works whether online or offline
                // It requires a user to have been previously authenticated
                user = app.CurrentUser;
                config = new FlexibleSyncConfiguration(user);
                realm = Realm.GetInstance(config);
                // Go on to add or update subscriptions and use the realm
            }
            // :replace-end:
            // :snippet-end:
        }

        [Test]
        public async Task TestCancelAsyncOperationsOnNonFatalErrors()
        {
            var app = App.Create(Config.FSAppId);
            var user = await app.LogInAsync(Credentials.Anonymous());

            // :snippet-start: appconfigsettings
            // :replace-start: {
            //  "terms": {
            //   "Config.FSAppId": "\"myRealmAppId\""}
            // }
            AppConfiguration configuration = new AppConfiguration(Config.FSAppId)
            {
                SyncTimeoutOptions = new SyncTimeoutOptions()
                {
                    ConnectTimeout = TimeSpan.FromMinutes(2),
                    ConnectionLingerTime = TimeSpan.FromSeconds(30),
                    PingKeepAlivePeriod = TimeSpan.FromMinutes(1),
                    PongKeepAliveTimeout = TimeSpan.FromMinutes(1),
                    FastReconnectLimit = TimeSpan.FromMinutes(1),
                },
            };
            // :replace-end:
            // :snippet-end:
            // :snippet-start: cancelasync
            var config = new FlexibleSyncConfiguration(app.CurrentUser!)
            {
                CancelAsyncOperationsOnNonFatalErrors = true,
            };

            // These operations will throw an exception
            // on timeout or other transient sync session errors.
            // :uncomment-start:
            //var realm = await Realm.GetInstanceAsync(config);

            //var session = realm.SyncSession;
            //await session.WaitForUploadAsync();
            //await session.WaitForDownloadAsync();
            // :uncomment-end:
            // :snippet-end:
        }
        public void WeeSnippetForDocs()
        {
            var realm = Realm.GetInstance();
            // :snippet-start: example_sub
            realm.Subscriptions.Update(() =>
            {
                var completedItemsQuery = realm
                    .All<MyTask>()
                    .Where(i => i.Status == "completed");
                realm.Subscriptions
                    .Add(completedItemsQuery,
                        new SubscriptionOptions() { Name = "completedItems" });
            });
            // :snippet-end:
        }
    }


    partial class MyTask : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("name")]
        public string Name { get; set; }

        [MapTo("status")]
        public string Status { get; set; }

        [MapTo("owner")]
        public string Owner { get; set; }

        [MapTo("progressMinutes")]
        public int ProgressMinutes { get; set; }

    }
    public enum ItemStatus
    {
        Open,
        InProgress,
        Complete
    }
    partial class Team : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("name")]
        public string Name { get; set; }

        [MapTo("description")]
        public string Description { get; set; }

    }
}