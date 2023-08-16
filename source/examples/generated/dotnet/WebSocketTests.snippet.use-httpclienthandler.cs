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
