realm.Subscriptions.Update(() =>
{
    // remove a subscription by it's query
    var query = realm.All<MyTask>().Where(i => i.Owner == "Ben");
    realm.Subscriptions.Remove(query);
});
