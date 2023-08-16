using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Realms;
using Realms.Sync;
using RealmUser = Realms.Sync.User;
using User = Examples.Models.User;
using Realms.Sync.Exceptions;
using Realms.Sync.Testing;
using Realms.Sync.ErrorHandling;
using static Realms.Sync.SyncConfigurationBase;
using System.Net.Http;

namespace Examples
{
    public class WebSocketTests
    {
        App app;
        RealmUser user;

        const string myRealmAppId = Config.FSAppId;
        App fsApp = null!;
        Realm fsRealm = null!;
        RealmUser fsUser = null!;

        public WebSocketTests()
        {
            fsRealm = Realm.GetInstance();
        }

        [Test]
        public async Task TestWebSocket()
        {
            // :snippet-start: use-httpclienthandler
            var messageHandler = new HttpClientHandler()
            {
                UseProxy = true,
                UseCookies = false,
            };

            var appConfig = new AppConfiguration(myRealmAppId)
            {
                HttpClientHandler = messageHandler
            };

            app = App.Create(appConfig);
            // :snippet-end:

            Assert.AreEqual(myRealmAppId, app.Id);
        }
    }
}