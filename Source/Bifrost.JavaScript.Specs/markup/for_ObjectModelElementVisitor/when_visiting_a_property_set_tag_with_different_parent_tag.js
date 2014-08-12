describe("when visiting a property set tag with different parent tag", function() {
	var instance = { some: "instance" };
	var objectModelManager = {
	    canResolve: sinon.stub().returns(true),
	    beginResolve: sinon.stub().returns({
	        continueWith: function (callback) {
	            callback(instance);
	        }
	    })
	};

	var visitor = Bifrost.markup.ObjectModelElementVisitor.create({
		objectModelManager: objectModelManager,
		markupExtensions: {},
		typeConverters: {}	
	});

	var exception = null;
	var parentElement = { localName: "somethingelse" };
	var element = { localName: "something.property", attributes: [], parentElement: parentElement, isKnownType: sinon.stub().returns(false) };
    try {
        visitor.visit(element);
    } catch (e) {
	    exception = e;
	} 

	it("should throw parent tagname mismatched exception", function() {
	    expect(exception instanceof Bifrost.markup.ParentTagNameMismatched).toBe(true);
	});
});