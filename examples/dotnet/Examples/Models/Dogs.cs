using System;
using System.Collections.Generic;
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
    //   "Person40": "Person",
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
    //   "Person50": "Person",
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
        public Person50 Owner { get; set; }
    }
    //:replace-end:
    // :code-block-end:

    // :code-block-start: dog_class
    // :replace-start: {
    //  "terms": {
    //   "DogE": "Dog",
    //   "Person1000" : "Person" }
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
        public IList<Person1000> Owners { get; }
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

}

