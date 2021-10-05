using System;
using NUnit.Framework;
using Realms;
using Realms.Schema;

namespace Examples
{
    public class Schemas
    {
        public Schemas()
        {
        }

        [Test]
        public void TestSchemas()
        {
            // :code-block-start: schema_property
<<<<<<< HEAD
            // Construct the config automatically from C# classes
=======
            // By default, all loaded RealmObject classes are included.
            // Use the RealmConfiguration when you want to 
            // construct a schema for only specific C# classes:
>>>>>>> 599a3f0f (Update to reflect ObjectClass is now Schema property (#1383))
            var config = new RealmConfiguration
            {
                Schema = new[] { typeof(ClassA), typeof(ClassB) }
            };

<<<<<<< HEAD
            // More advanced: construct the configuration manually
=======
            // More advanced: construct the schema manually
>>>>>>> 599a3f0f (Update to reflect ObjectClass is now Schema property (#1383))
            var manualConfig = new RealmConfiguration
            {
                Schema = new RealmSchema.Builder
                {
                    new ObjectSchema.Builder("ClassA", isEmbedded: false)
                    {
<<<<<<< HEAD
                        Property.Primitive("Id", RealmValueType.Guid, isPrimaryKey: true),
                        Property.Primitive("LastName", RealmValueType.String, isNullable: true, isIndexed: true)
=======
                        Property.Primitive("Id",
                            RealmValueType.Guid,
                            isPrimaryKey: true),
                        Property.Primitive("LastName",
                            RealmValueType.String,
                            isNullable: true,
                            isIndexed: true)
>>>>>>> 599a3f0f (Update to reflect ObjectClass is now Schema property (#1383))
                    }
                }
            };

<<<<<<< HEAD
            // Most advanced: Mix and match
            var mixedSchema = new ObjectSchema.Builder(typeof(ClassA));
            mixedSchema.Add(Property.FromType<int>("ThisIsNotInTheCSharpClass"));
            // mixedSchema now has all properties on the ClassA class
=======
            // Most advanced: mix and match
            var mixedSchema = new ObjectSchema.Builder(typeof(ClassA));
            mixedSchema.Add(Property.FromType<int>("ThisIsNotInTheCSharpClass"));
            // `mixedSchema` now has all of the properties of the ClassA class
>>>>>>> 599a3f0f (Update to reflect ObjectClass is now Schema property (#1383))
            // and an extra integer property called "ThisIsNotInTheCSharpClass"

            var mixedConfig = new RealmConfiguration
            {
                Schema = new[] { mixedSchema.Build() }
            };
            // :code-block-end:
<<<<<<< HEAD
=======

            Assert.AreEqual(2, config.Schema.Count);
            Assert.AreEqual(1, manualConfig.Schema.Count);
            Assert.AreEqual(1, mixedConfig.Schema.Count);
            ObjectSchema foo;
            mixedConfig.Schema.TryFindObjectSchema("ClassA", out foo);
            if (foo != null)
            {
                Property newProp;
                Assert.IsTrue(foo.TryFindProperty("ThisIsNotInTheCSharpClass", out newProp));
            }
>>>>>>> 599a3f0f (Update to reflect ObjectClass is now Schema property (#1383))
        }
    }

    class ClassA : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public string Id { get; set; }
    }
    class ClassB : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public string Id { get; set; }
    }
}
