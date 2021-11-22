using System;
using System.Linq;
using MongoDB.Bson;
using Realms;

namespace Examples.Models
{
    public class Hobby : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // An inverse relationship that returns all Person instances that have the current Hobby
        // instance in their Hobbies list.
        [Backlink(nameof(Person50.Hobbies))]
        public IQueryable<Person50> PeopleWithThatHobby { get; }
        // :replace-end:
    }
    // :code-block-end:
}
