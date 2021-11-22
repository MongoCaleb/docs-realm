using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Models;
using MongoDB.Bson;
using Realms;

namespace ObjectExamples
{
    class Rename
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        // :code-block-start: ignore
        // Rather than store an Image in Realm,
        // store the path to the Image...
        public string ThumbnailPath { get; set; }

        // ...and the Image itself can be
        // in-memory when the app is running:
        [Ignored]
        public Image Thumbnail { get; set; }
        // :code-block-end:

        public class Image
        {
        }
    }

    // :code-block-start: subset
    //:replace-start: {
    // "terms": {
    //      "DogA": "Dog"}
    // }
    // Declare your schema
    class LoneClass : RealmObject
    {
        //:hide-start:
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId ID { get; set; }
        //:hide-end:
        public string Name { get; set; }
    }

    class AnotherClass
    {
        private void SetUpMyRealmConfig()
        {
            // Define your config with a single class
            var config = new RealmConfiguration("RealmWithOneClass.realm");
            config.Schema = new[] { typeof(LoneClass) };

            // Or, specify multiple classes to use in the Realm
            config.Schema = new[] { typeof(DogA), typeof(Cat) };
        }
    }
    // :replace-end:
    // :code-block-end:

    class Cat
    { }

}
