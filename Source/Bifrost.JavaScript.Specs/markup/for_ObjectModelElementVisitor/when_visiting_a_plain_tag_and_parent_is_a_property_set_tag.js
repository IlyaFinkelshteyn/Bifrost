describe("when visiting a plain tag and parent is a property set tag", function() {
	var instance = { some: "instance"};
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

	var parentObjectModelNode = {
	};

	var parentElement = { localName: "something.property", __objectModelNode: parentObjectModelNode, isKnownType: sinon.stub().returns(false) };
	var element = { localName: "somethingelse", attributes: [], parentElement: parentElement, isKnownType: sinon.stub().returns(false) };
	visitor.visit(element);

	it("should ask for an object by tag name", function() {
	    expect(objectModelManager.beginResolve.calledWith("somethingelse")).toBe(true);
	});

	it("should set the object instance to the parent elements object model node", function() {
		expect(parentObjectModelNode.property).toBe(instance);
	});
});