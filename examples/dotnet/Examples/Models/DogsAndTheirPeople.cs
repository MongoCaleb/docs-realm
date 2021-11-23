using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Realms;

namespace Examples.Models
{

    // :code-block-start: primary-key
    // :replace-start: {
    //  "terms": {
    //      "PersonA": "Person",
    //      "DogA": "Dog",
    //      "//[NotPrimaryKey]": "[PrimaryKey]" }
    // }
    public class DogA : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        //[NotPrimaryKey]
        public string Name { get; set; }
        public int Age { get; set; }
        public PersonA Owner { get; set; }
    }
    //:replace-end:
    // :code-block-end:

    // :code-block-start: rel-to-one
    // :replace-start: {
    //     "terms": {
    //      "Person30": "Person",
    //      "DogB": "Dog" }
    // }
    public class DogB : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // ... other property declarations
        public Person30 Owner { get; set; }
    }
    //:replace-end:
    // :code-block-end:

    // :code-block-start: rel-to-many
    // :replace-start: {
    //  "terms": {
    //   "PersonF": "Person",
    //   "DogC" : "Dog" }
    // }
    public class DogC : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // ... other property declarations
        public string Name { get; set; }
    }
    //:replace-end:
    // :code-block-end:

    // :code-block-start: inverse
    //  :replace-start: {
    //  "terms": {
    //   "PersonG": "Person",
    //   "DogD":"Dog" }
    // }
    public class DogD : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // To-one relationship from the Dog to its owner
        public PersonG Owner { get; set; }
    }

    public class PersonG : RealmObject
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
    public class Hobby : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // An inverse relationship that returns all Person instances that have the current Hobby
        // instance in their Hobbies list.
        [Backlink(nameof(PersonG.Hobbies))]
        public IQueryable<PersonG> PeopleWithThatHobby { get; }
    }
    // :replace-end:
    // :code-block-end:

    // :code-block-start: dog_class
    // :replace-start: {
    //  "terms": {
    //   "DogE": "Dog",
    //   "PersonB" : "Person" }
    // }
    public class DogE : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }
        public string Breed { get; set; }
        public IList<PersonB> Owners { get; }
    }

    public class PersonB : RealmObject
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


    public class WriteDog : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }
        public string Breed { get; set; }
        public WritePerson Owner { get; set; }
    }

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


    // :code-block-start: index
    // :replace-start: {
    //  "terms": {
    //      "PersonC": "Person",
    //      "DogA": "Dog"}
    // }
    public class PersonC : RealmObject
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

    // :code-block-start:
    // :replace-start: {
    //     "terms": {
    //      "PersonF": "Person",
    //      "DogC" : "Dog" }
    // }
    public class PersonF : RealmObject
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

}

