using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

namespace Examples.Models
{
    // NOTE: These classes left here because they
    // are embedded and must be kept together
    // for the code snippet

    // :code-block-start:embedded-classes
    public class Address : EmbeddedObject
    {
        [MapTo("street")]
        public string Street { get; set; }

        [MapTo("city")]
        public string City { get; set; }

        [MapTo("country")]
        public string Country { get; set; }

        [MapTo("postalCode")]
        public string PostalCode { get; set; }

    }
    public class Contact : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("name")]
        public string Name { get; set; }

        [MapTo("address")]
        public Address Address { get; set; } // embed a single address 

    }
    public class Business : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("name")]
        public string Name { get; set; }

        [MapTo("addresses")]
        public IList<Address> Addresses { get; }
    }
    //:code-block-end:

    // :code-block-start: embedded
    // :replace-start: {
    //  "terms": {
    //      "AddressB": "Address",
    //       "Contact10": "Contact"}
    // }
    public class AddressB : EmbeddedObject
    {
        public ObjectId Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
    public class Contact10 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public AddressB Address { get; set; } // embed a single address 
    }
    // :replace-end:
    // :code-block-end:
}
