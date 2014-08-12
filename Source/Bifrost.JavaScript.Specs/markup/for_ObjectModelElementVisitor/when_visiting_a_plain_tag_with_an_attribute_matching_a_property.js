describe("when visiting a plain tag with an attribute matching a property", function() {
	var obj = {
		someProperty: 0
	};

	var objectModelManager = {
	    canResolve: sinon.stub().returns(true),
	    beginResolve: sinon.stub().returns({
	        continueWith: function (callback) {
	            callback(obj);
	        }
	    })
	};
	var typeConverters = {
		convert: sinon.stub().returns(42)
	}

	var visitor = Bifrost.markup.ObjectModelElementVisitor.create({
		objectModelManager: objectModelManager,
		markupExtensions: {},
		typeConverters: typeConverters
	});

	var element = { 
		localName: "something",
		attributes: [
			{localName:"someProperty", value:"42"}
		],
		isKnownType: sinon.stub().returns(false)
	};
	visitor.visit(element);

	it("should ask the type converters for a conversion", function() {
		expect(typeConverters.convert.calledWith("number","42")).toBe(true);
	});

	it("should set the converted value on the object", function() {
		expect(obj.someProperty).toBe(42);
	});
});