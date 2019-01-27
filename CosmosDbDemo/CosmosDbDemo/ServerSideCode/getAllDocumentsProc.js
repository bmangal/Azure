// Stored Procedure getting documents by partition key.
function getAllDocumentsProc() {
    var context = getContext();
    var collection = context.getCollection();
    var response = context.getResponse();

    // Calling method to query documents
    var  isAccepted = collection.queryDocuments(
        collection.getSelfLink(),   // _selflink of collection
        'SELECT * FROM c',          // input query
        function  (err, feed, options) {
            if  (err)  throw  err;

            // Undefined result or Zero records
            if  (!feed || !feed.length) {
                response.setBody('No documents are found.');
            } else {
                // Documents were retrieved successfully. Set response body to list of documents.
                response.setBody(feed);
            }
        });

    // Time out
    if  (!isAccepted)  throw  new  Error('The query was not accepted by the server.');
}
