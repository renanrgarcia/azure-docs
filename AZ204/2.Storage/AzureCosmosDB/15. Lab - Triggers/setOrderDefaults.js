function setOrderDefaults() {
    var context = getContext();
    var request = context.getRequest();

    var doc = request.getBody();

    doc.createdUtc = new Date().toISOString();
    doc.type = "order";

    request.setBody(doc);
}
