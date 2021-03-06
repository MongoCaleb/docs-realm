List<Document> pipeline = Arrays.asList(
        new Document("$group", new Document("_id", "$type")
                .append("totalCount", new Document("$sum", 1))));
RealmResultTask<MongoCursor<Document>> aggregationTask = mongoCollection.aggregate(pipeline).iterator();
aggregationTask.getAsync(task -> {
    if (task.isSuccess()) {
        MongoCursor<Document> results = task.get();
        Log.d("EXAMPLE", "successfully aggregated the plants by type. Type summary:");
        while (results.hasNext()) {
            Log.v("EXAMPLE", results.next().toString());
        }
        expectation.fulfill();
    } else {
        Log.e("EXAMPLE", "failed to aggregate documents with: ", task.getError());
    }
});