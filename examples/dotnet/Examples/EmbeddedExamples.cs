using System;
using System.Linq;
using Examples.Models;
using NUnit.Framework;
using Realms;
using Task = System.Threading.Tasks.Task;

namespace Examples
{
    public class EmbeddedExamples
    {
        RealmConfiguration config;

        [OneTimeSetUp]
        public async Task Setup()
        {
            config = new RealmConfiguration();// "myPart", user);

            // Synchronous here because setup and tear down don't support async
            var realm = Realm.GetInstance(config);

            realm.Write(() =>
            {
                realm.RemoveAll<Contact>();
                realm.RemoveAll<Business>();
            });

            // :code-block-start:create
            var address = new Address() // Create an Address
            {
                Street = "123 Fake St.",
                City = "Springfield",
                Country = "USA",
                PostalCode = "90710"
            };

            var contact = new Contact() // Create a Contact
            {
                Name = "Nick Riviera",
                // :hide-start:
                Partition = "myPart",
                // :hide-end:
                Address = address // Embed the Address Object
            };

            realm.Write(() =>
            {
                realm.Add(contact);
            });
            //:code-block-end:

            var contacts = realm.All<Contact>();
            // Test that the Contact document has been created
            Assert.AreEqual(1, contacts.Count());

            // Test that the first (and only) Contact document has an embedded Address with a Street of "123 Fake St."
            Assert.AreEqual(contacts.FirstOrDefault().Address.Street, "123 Fake St.");
            return;
        }

        [Test]
        public async Task UpdateEmbeddedObject()
        {
            using (var realm = await Realm.GetInstanceAsync(config))
            {

                // :code-block-start:update
                var resultContact = realm.All<Contact>() // Find the First Contact (Sorted By Name)
                    .OrderBy(c => c.Name)
                    .FirstOrDefault();

                // Update the Result Contact's Embedded Address Object's Properties
                realm.Write(() =>
                {
                    resultContact.Address.Street = "Hollywood Upstairs Medical College";
                    resultContact.Address.City = "Los Angeles";
                    resultContact.Address.PostalCode = "90210";
                });
                //:code-block-end:

                // Test that the Contact embedded Address's Street has been updated
                Assert.AreEqual(resultContact.Address.Street, "Hollywood Upstairs Medical College");
            }

        }

        [Test]
        public async Task OverwriteEmbeddedObject()
        {
            using (var realm = await Realm.GetInstanceAsync(config))
            {

                // :code-block-start:overwrite
                var oldContact = realm.All<Contact>() // Find the first contact
                .OrderBy(c => c.Name)
                .FirstOrDefault();


                var newAddress = new Address() // Create an Address
                {
                    Street = "100 Main Street",
                    City = "Los Angeles",
                    Country = "USA",
                    PostalCode = "90210"
                };

                realm.Write(() =>
                {
                    oldContact.Address = newAddress;
                });
                //:code-block-end:

                /* Test that the Contact field's Embedded Address has been overwritten
                 * with the new Address by checking the Address Street. */
                Assert.AreEqual(oldContact.Address.Street, "100 Main Street");
            }
        }


        [Test]
        public async Task QueryEmbeddedObject()
        {
            using (var realm = await Realm.GetInstanceAsync(config))
            {
                var address = new Address() // Create an Address
                {
                    Street = "123 Fake St.",
                    City = "Los Angeles",
                    Country = "USA",
                    PostalCode = "90710"
                };

                var newContact = new Contact() // Create a Contact
                {
                    Name = "Foo Barbaz",
                    Partition = "myPart",
                    Address = address // Embed the Address Object
                };

                realm.Write(() =>
                {
                    realm.Add(newContact);
                });

                // :code-block-start:query
                // Find All Contacts with an Address of "Los Angeles"
                var losAngelesContacts = realm.All<Contact>()
                    .Filter("address.city == 'Los Angeles'");

                foreach (var contact in losAngelesContacts)
                {
                    Console.WriteLine("Los Angeles Contact:");
                    Console.WriteLine(contact.Name);
                    Console.WriteLine(contact.Address.Street);
                }
                //:code-block-end:

                // Test that the query worked and that the Contacts returned 
                // actually are from 'Los Angeles'. 
                Assert.AreEqual(losAngelesContacts.FirstOrDefault()
                    .Address.City, "Los Angeles");
            }
        }


        [OneTimeTearDown]
        public async Task TearDown()
        {
            using (var realm = await Realm.GetInstanceAsync(config))
            {
                realm.Write(() =>
                {
                    realm.RemoveAll<Contact>();
                    realm.RemoveAll<Business>();
                });
            }
            return;
        }


    }
}