function placeOrder(order, audit) {
    var context = getContext();
    var collection = context.getCollection();
    var response = context.getResponse();

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
