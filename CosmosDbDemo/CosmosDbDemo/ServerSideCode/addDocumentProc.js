// Stored Procedure used to insert a new document.
function addDocumentProc(person) {
    var context = getContext();
    var collection = context.getCollection();
    var response = context.getResponse();

    // When user does not define id, a GUID will be automatically generated as id.
    var options = { disableAutomaticIdGeneration: false };

    // Calling method to create a document
    var isAccepted = collection.createDocument(
        collection.getSelfLink(),   // _selflink of collection
        person,                     // document being inserted
        options, 
        function (err, feed) {
            if (err) throw err;

            if (!feed) {
                response.setBody('No person is created.');
            } else {
                // Document was added successfully. Set response body as the created result object.
                response.setBody(feed);
            }
        });

    // Time out
    if (!isAccepted) throw new Error('The person could not be created.');
}
