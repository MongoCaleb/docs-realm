using System;
using System.Collections.Specialized;
using System.Linq;
using Examples.Models;
using MongoDB.Bson;
using NUnit.Framework;
using Realms;
using Realms.Sync;
using System.Diagnostics;

namespace Examples
{
    public class Notifications
    {
        App app;
        Realms.Sync.User user;
        string myRealmAppId = _RealmAppConfigurationHelper.appid;

        [OneTimeSetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            var appConfig = new AppConfiguration(myRealmAppId)
            {
                DefaultRequestTimeout = TimeSpan.FromMilliseconds(1500)
            };

            app = App.Create(appConfig);
            user = await app.LogInAsync(Credentials.EmailPassword("foo@foo.com", "foobar"));
            return;
        }

        [Test]
        public void SubscribeToNotifications()
        {
            var realm = Realm.GetInstance("");

            //:code-block-start:notifications
            // Observe realm notifications.
            realm.RealmChanged += (sender, eventArgs) =>
            {
                // sender is the realm that has changed.
                // eventArgs is reserved for future use.
                // ... update UI ...
            };
            //:code-block-end:

            //:code-block-start:collection-notifications
            // :replace-start: {
            //  "terms": {
            //   "DogE": "Dog",
            //   "PersonA" : "Person" }
            // }
            // Observe collection notifications. Retain the token to keep observing.
            var token = realm.All<DogE>()
                .SubscribeForNotifications((sender, changes, error) =>
                {
                    if (error != null)
                    {
                        // Show error message
                        return;
                    }

                    if (changes == null)
                    {
                        // This is the case when the notification is called
                        // for the first time.
                        // Populate tableview/listview with all the items
                        // from `collection`
                        return;
                    }

                    // Handle individual changes

                    foreach (var i in changes.DeletedIndices)
                    {
                        // ... handle deletions ...
                    }

                    foreach (var i in changes.InsertedIndices)
                    {
                        // ... handle insertions ...
                    }

                    foreach (var i in changes.NewModifiedIndices)
                    {
                        // ... handle modifications ...
                    }
                });

            // Later, when you no longer wish to receive notifications
            token.Dispose();
            // :replace-end:
            //:code-block-end:


            realm.Write(() =>
            {
                realm.Add(new PersonA { Id = ObjectId.GenerateNewId(), Name = "Elvis Presley" });
            });
            //:code-block-start:object-notifications
            // :replace-start: {
            //  "terms": {
            //   "DogE": "Dog",
            //   "PersonA" : "Person" }
            // }
            var theKing = realm.All<PersonA>()
                .FirstOrDefault(p => p.Name == "Elvis Presley");

            theKing.PropertyChanged += (sender, eventArgs) =>
            {
                Debug.WriteLine("New value set for The King: " +
                    eventArgs.PropertyName);
            };
        }
        // :replace-end:
        //:code-block-end:

        class NotificationUnsub
        {
            Realm realm;


            public NotificationUnsub()
            {
                realm = Realm.GetInstance("");
            }
            //:code-block-start:unsubscribe
            private IQueryable<Task> tasks;

            public void LoadUI()
            {
                tasks = realm.All<Task>();

                // Subscribe for notifications - since tasks is IQueryable<Task>, we're
                // using the AsRealmCollection extension method to cast it to IRealmCollection
                tasks.AsRealmCollection().CollectionChanged += OnTasksChanged;
            }

            public void UnloadUI()
            {
                // Unsubscribe from notifications
                tasks.AsRealmCollection().CollectionChanged -= OnTasksChanged;
            }

            private void OnTasksChanged(object sender, NotifyCollectionChangedEventArgs args)
            {
                // Do something with the notification information
            }
            // :code-block-end:
        }
    }
}
