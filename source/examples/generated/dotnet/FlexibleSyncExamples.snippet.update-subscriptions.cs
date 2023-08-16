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
