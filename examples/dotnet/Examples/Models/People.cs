using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Realms;

namespace Examples.Models
{

    // :code-block-start: default
    // :replace-start: {
    //  "terms": {
    //      "Person1": "Person" }
    // }
    public class Person1 : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        public string Name { get; set; } = "foo";
    }
    // :replace-end:
    // :code-block-end:



    // :code-block-start: rel-to-one
    // :replace-start: {
    //     "terms": {
    //      "Person30": "Person"}
    // }
    public class Person30 : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // ... other property declarations
        public string Name { get; set; }
    }
    // :replace-end:
    // :code-block-end:



    // :code-block-start: rename
    //:replace-start: {
    // "terms": {
    //   "PersonH": "Person"}
    // }
    public class PersonH : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        [MapTo("moniker")]
        public string Name { get; set; }
    }
    // :replace-end:
    // :code-block-end:


    // :code-block-start: rename-class
    // :replace-start: {
    // "terms": {
    //   "PersonI": "Person"
    // }
    [MapTo("Human")]
    public class PersonI : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        public string Name { get; set; }
    }
    // :replace-end:
    // :code-block-end:

    // :code-block-start: ro1
    // :replace-start: {
    // "terms": {
    //   "Person100": "Person"}
    // }
    public class Person100 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }
        public int Age { get; set; }
    }
    // :replace-end:
    // :code-block-end:

    // :code-block-start: ro2
    // :replace-start: {
    // "terms": {
    //   "PersonD": "Person"
    // }}
    public class PersonD : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
    // :replace-end:
    // :code-block-end:

    // :code-block-start: ro3
    // :replace-start: {
    // "terms": {
    //   "PersonJ": "Person"
    // }}
    public class PersonJ : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        public string FullName { get; set; }
        public int Age { get; set; }
    }
    // :replace-end:
    // :code-block-end:

    // :code-block-start: ro4
    // :replace-start: {
    // "terms": {
    //   "PersonE": "Person"
    // }}
    public class PersonE : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        public string FullName { get; set; }
        public DateTimeOffset Birthday { get; set; }
    }
    // :replace-end:
    // :code-block-end:

    public class WritePerson : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public int Id { get; set; }

        public string Name { get; set; }

        [Backlink("Owner")]
        public IQueryable<WriteDog> Dogs { get; }
    }
}
