package com.mongodb.realm.examples.kotlin

import android.util.Log
import com.mongodb.realm.examples.Expectation
import com.mongodb.realm.examples.RealmTest
import com.mongodb.realm.examples.model.kotlin.Dog
import com.mongodb.realm.examples.model.kotlin.Person
import io.realm.Realm
import io.realm.RealmConfiguration
import io.realm.kotlin.where
import org.bson.types.ObjectId
import org.junit.Assert
import org.junit.Test
import java.util.*

class RealmQueryTest : RealmTest() {
    @Test
    fun testFindObjectByPrimaryKey() {
        val expectation : Expectation = Expectation()
        activity?.runOnUiThread {
            // :code-block-start: find-object-by-primary-key-local
            val config = RealmConfiguration.Builder()
                .allowWritesOnUiThread(true)
                .allowQueriesOnUiThread(true)
                .build()

            Realm.getInstanceAsync(config, object : Realm.Callback() {
                override fun onSuccess(realm: Realm) {
                    Log.v(
                        "EXAMPLE",
                        "Successfully opened a realm with reads and writes allowed on the UI thread."
                    )

                    // :hide-start:
                    var PRIMARY_KEY_VALUE : String? = null
                    realm.executeTransaction { transactionRealm ->
                        val newTask = Task()
                        newTask.name = "test task" + Random().nextLong()
                        transactionRealm.insert(newTask)
                        PRIMARY_KEY_VALUE = newTask.name
                    }
                    // :hide-end:

                    realm.executeTransaction { transactionRealm ->
                        val task = transactionRealm.where<Task>().equalTo("name", PRIMARY_KEY_VALUE).findFirst()
                        Log.v("EXAMPLE", "Found object by primary key: $task")
                        // :hide-start:
                        Assert.assertEquals(task?.name, PRIMARY_KEY_VALUE)
                        // :hide-end:
                    }
                    // :hide-start:
                    expectation.fulfill()
                    // :hide-end:
                    realm.close()
                }
            })
            // :code-block-end:
        }
        expectation.await()
    }

    @Test
    fun testQueryARelationship() {
        val expectation : Expectation = Expectation()
        activity?.runOnUiThread {
            // :code-block-start: query-a-relationship-local
            val config = RealmConfiguration.Builder()
                .allowQueriesOnUiThread(true)
                .allowWritesOnUiThread(true)
                .build()

            Realm.getInstanceAsync(config, object : Realm.Callback() {
                override fun onSuccess(realm: Realm) {
                    Log.v(
                        "EXAMPLE",
                        "Successfully opened a realm with reads and writes allowed on the UI thread."
                    )

                    // :hide-start:
                    realm.executeTransaction { transactionRealm -> ;
                        val newDog = Dog("henry")
                        val newPerson = Person("dwayne")
                        newPerson.dog = newDog
                        transactionRealm.insert(newPerson)
                    }
                    // :hide-end:

                    realm.executeTransaction { transactionRealm ->
                        val owner = transactionRealm.where<Person>().equalTo("dog.name", "henry").findFirst()
                        val dog = owner?.dog
                        Log.v("EXAMPLE", "Queried for people with dogs named 'henry'. Found $owner, owner of $dog")
                        // :hide-start:
                        Assert.assertEquals(dog?.name, "henry")
                        Assert.assertEquals(owner?.name, "dwayne")
                        // :hide-end:
                    }
                    // :hide-start:
                    expectation.fulfill()
                    // :hide-end:
                    realm.close()
                }
            })
            // :code-block-end:
        }
        expectation.await()
    }

    @Test
    fun testQueryAnInverseRelationship() {
        val expectation : Expectation = Expectation()
        activity?.runOnUiThread {
            // :code-block-start: query-an-inverse-relationship-local
            val config = RealmConfiguration.Builder()
                .allowQueriesOnUiThread(true)
                .allowWritesOnUiThread(true)
                .build()

            Realm.getInstanceAsync(config, object : Realm.Callback() {
                override fun onSuccess(realm: Realm) {
                    Log.v(
                        "EXAMPLE",
                        "Successfully opened a realm with reads and writes allowed on the UI thread."
                    )

                    // :hide-start:
                    realm.executeTransaction { transactionRealm ->
                        val newDog = Dog("henry")
                        val newPerson = Person("dwayne")
                        newPerson.dog = newDog
                        transactionRealm.insert(newPerson)
                    }
                    // :hide-end:

                    realm.executeTransaction { transactionRealm ->
                        val dog = transactionRealm.where<Dog>().equalTo("owner.name", "dwayne").findFirst()
                        val owner = dog?.owner
                        Log.v("EXAMPLE", "Queried for dogs with owners named 'dwayne'. Found $dog, owned by $owner")
                        // :hide-start:
                        //Assert.assertEquals(dog?.name, "henry")
                        //Assert.assertEquals(owner?.name, "dwayne")
                        // :hide-end:
                    }
                    // :hide-start:
                    expectation.fulfill()
                    // :hide-end:
                    realm.close()
                }
            })
            // :code-block-end:
        }
        expectation.await()
    }

}