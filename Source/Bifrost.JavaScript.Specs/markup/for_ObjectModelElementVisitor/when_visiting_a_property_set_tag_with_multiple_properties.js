describe("when visiting a property set tag with multiple properties", function() {
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
	var element = { localName: "something.property.otherProperty", attributes: [], isKnownType: sinon.stub().returns(false) };
	try {
		visitor.visit(element);
	} catch( e ) {
		exception = e;
	} 

	it("should throw multiple property references not allowed exception", function() {
	    expect(exception instanceof Bifrost.markup.MultiplePropertyReferencesNotAllowed).toBe(true);
	});
});