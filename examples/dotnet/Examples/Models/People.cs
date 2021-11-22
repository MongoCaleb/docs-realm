using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Realms;

namespace Examples.Models
{

    // :code-block-start: required
    // :replace-start: {
    //  "terms": {
    //      "PersonA": "Person",
    //      "DogA": "Dog"}
    // }
    public class PersonA : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        [Required]
        public string Name { get; set; }
        public IList<DogA> Dogs { get; }
    }
    //:replace-end:
    // :code-block-end:

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

    // :code-block-start: index
    // :replace-start: {
    //  "terms": {
    //      "Person20": "Person",
    //      "DogA": "Dog"}
    // }
    public class Person20 : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        [Indexed]
        public string Name { get; set; }
        public IList<DogA> Dogs { get; }
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



    public class Person40 : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // ... other property declarations
        public IList<DogC> Dogs { get; }
    }
    // :replace-end:
    // :code-block-end:



    public class Person50 : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // An inverse relationship that returns all Dog instances that have Dog.Owner set to
        // the current Person.
        [Backlink(nameof(DogD.Owner))]
        public IQueryable<DogD> Dogs { get; }

        // To-many relationship, containing a collection of all hobbies the current person enjoys
        public IList<Hobby> Hobbies { get; }
    }
    // :replace-end:
    // :code-block-end:

    // :code-block-start: rename
    //:replace-start: {
    // "terms": {
    //   "Person60": "Person"}
    // }
    public class Person60 : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        [MapTo("moniker")]
        public string Name { get; set; }
    }
    //:replace-end:
    // :code-block-end:


    // :code-block-start: rename-class
    //:replace-start: {
    // "terms": {
    //   "Person70": "Person",
    //      "DogA": "Dog"}
    // }
    [MapTo("Human")]
    public class Person70 : RealmObject
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
    //:replace-start: {
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
    //:replace-end:
    // :code-block-end:

    // :code-block-start: ro2
    //:replace-start: {
    // "terms": {
    //   "Person200": "Person"
    // }}
    public class Person200 : RealmObject
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
    //:replace-start: {
    // "terms": {
    //   "Person300": "Person"
    // }}
    public class Person300 : RealmObject
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
    //:replace-start: {
    // "terms": {
    //   "Person400": "Person"
    // }}
    public class Person400 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        public string FullName { get; set; }
        public DateTimeOffset Birthday { get; set; }
    }
    // :replace-end:
    // :code-block-end:


    //TODO: why is there no code start on this one???

    public class Person1000 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; }
        //etc...

        /* To add items to the IList<T>:

        var dog = new Dog();
        var caleb = new Person { Name = "Caleb" };
        dog.Owners.Add(caleb);

        */
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
