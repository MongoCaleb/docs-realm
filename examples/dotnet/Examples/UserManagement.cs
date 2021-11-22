using System;
using System.Linq;
using NUnit.Framework;
using Realms.Sync;
using Task = System.Threading.Tasks.Task;

namespace Examples
{
    public class UserManagement
    {
        App app;
        User user;
        string myRealmAppId = _RealmAppConfigurationHelper.appid;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var appConfig = new AppConfiguration(myRealmAppId)
            {
                DefaultRequestTimeout = TimeSpan.FromMilliseconds(1500)
            };

            app = App.Create(appConfig);
            user = await app.LogInAsync(Credentials.EmailPassword("foo@foo.com", "foobar"));
            return;
        }

        public async void ManageUsers()
        {
            string userEmail = "bob@bob.com";

            // :code-block-start: initialize-realm
            var myRealmAppId = "<your_app_id>";
            var app = App.Create(myRealmAppId);
            //:code-block-end:
            // :code-block-start: appConfig
            var appConfig = new AppConfiguration(myRealmAppId)
            {
                //LogLevel = LogLevel.Debug,
                DefaultRequestTimeout = TimeSpan.FromMilliseconds(1500)
            };

            app = App.Create(appConfig);
            //:code-block-end:
            // :code-block-start: register-user
            await app.EmailPasswordAuth.RegisterUserAsync(userEmail, "sekrit");
            //:code-block-end:
            // :code-block-start: confirm-user
            await app.EmailPasswordAuth.ConfirmUserAsync("<token>", "<token-id>");
            //:code-block-end:
            // :code-block-start: reset-user-1
            await app.EmailPasswordAuth.SendResetPasswordEmailAsync(userEmail);
            //:code-block-end:
            string myNewPassword = "";
            // :code-block-start: reset-user-2
            await app.EmailPasswordAuth.ResetPasswordAsync(
                myNewPassword, "<token>", "<token-id>");
            //:code-block-end:
            // :code-block-start: reset-user-3
            await app.EmailPasswordAuth.CallResetPasswordFunctionAsync(
                userEmail, myNewPassword,
                "<security-question-1-answer>",
                "<security-question-2-answer>");
            //:code-block-end:

            user = await app.LogInAsync(Credentials.EmailPassword("foo@foo.com", "foobar"));
        }

        [Test]
        public async Task MultiUser()
        {
            myRealmAppId = _RealmAppConfigurationHelper.appid;
            var app = App.Create(myRealmAppId);

            {
                foreach (var user in app.AllUsers)
                {
                    await user.LogOutAsync();
                }
                Assert.AreEqual(0, app.AllUsers.Count());
                //:code-block-start:multi-add
                var aimee = await app.LogInAsync(Credentials.EmailPassword(
                    "aimee@example.com", "sekrit"));
                Assert.IsTrue(aimee.Id == app.CurrentUser.Id, "aimee is current user");

                var elvis = await app.LogInAsync(Credentials.EmailPassword(
                    "elvis@example.com", "sekrit2"));
                Assert.IsTrue(elvis.Id == app.CurrentUser.Id, "elvis is current user");
                //:code-block-end:

                //:code-block-start:multi-list
                foreach (var user in app.AllUsers)
                {
                    Console.WriteLine($"User {user.Id} is logged on via {user.Provider}");
                }
                Assert.AreEqual(2, app.AllUsers.Count());
                //:code-block-end:
                //:code-block-start:multi-switch
                app.SwitchUser(aimee);
                Assert.IsTrue(aimee.Id == app.CurrentUser.Id, "aimee is current user");
                //:code-block-end:

                //:code-block-start:multi-remove
                await app.RemoveUserAsync(elvis);
                var noMoreElvis = app.AllUsers.FirstOrDefault(u => u.Id == elvis.Id);
                Assert.IsNull(noMoreElvis);
                Console.WriteLine("Elvis has left the application.");
                //:code-block-end:
            }

            return;
        }
    }
}