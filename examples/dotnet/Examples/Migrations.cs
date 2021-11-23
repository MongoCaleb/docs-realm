﻿using System;
using System.Linq;
using Examples.Models;
using MongoDB.Bson;
using Realms;

namespace Examples
{
    public class Migrations
    {
        public Migrations()
        {
            // :code-block-start: migrate
            var config = new RealmConfiguration
            {
                SchemaVersion = 4,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    //:replace-start: {
                    // "terms": {
                    //   "PersonD": "Person",
                    //   "PersonE":"Person"}
                    // }
                    var oldPeople = migration.OldRealm.DynamicApi.All("PersonD");
                    var newPeople = migration.NewRealm.All<PersonE>();
                    // :replace-end:

                    // Migrate Person objects
                    for (var i = 0; i < newPeople.Count(); i++)
                    {
                        var oldPerson = oldPeople.ElementAt(i);
                        var newPerson = newPeople.ElementAt(i);

                        // Changes from version 1 to 2 (adding LastName) will occur automatically when Realm detects the change

                        // Migrate Person from version 2 to 3: replace FirstName and LastName with FullName
                        // LastName doesn't exist in version 1
                        if (oldSchemaVersion < 2)
                        {
                            newPerson.FullName = oldPerson.FirstName;
                        }
                        else if (oldSchemaVersion < 3)
                        {
                            newPerson.FullName = $"{oldPerson.FirstName} {oldPerson.LastName}";
                        }

                        // Migrate Person from version 3 to 4: replace Age with Birthday
                        if (oldSchemaVersion < 4)
                        {
                            var birthYear = DateTimeOffset.UtcNow.Year - oldPerson.Age;
                            newPerson.Birthday = new DateTimeOffset(birthYear, 1, 1, 0, 0, 0, TimeSpan.Zero);
                        }
                    }
                }
            };
            var realm = Realm.GetInstance(config);
            // :code-block-end:
        }

    }
}
