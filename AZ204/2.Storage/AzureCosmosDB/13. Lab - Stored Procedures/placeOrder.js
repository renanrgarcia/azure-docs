function placeOrder(order, audit) {
    var context = getContext(); // Get the context of the stored procedure
    var collection = context.getCollection(); // Get the collection reference
    var response = context.getResponse(); // Get the response object to set the response body

    collection.createDocument(collection.getSelfLink(), order,
        function (err, createdOrder) {
            if (err) throw err;

            collection.createDocument(collection.getSelfLink(), audit,
                function (err2, createdAudit) {
                    if (err2) throw err2;

                    response.setBody({
                        orderId: createdOrder.id,
                        auditId: createdAudit.id
                    });
                });
        });
}
