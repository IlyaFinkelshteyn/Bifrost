﻿describe("when left hand is not satisfied and right hand is", function () {
    
    var leftHandSideEvaluator = sinon.stub().returns(false);
    var leftHandSide = Bifrost.rules.Rule.create()
    leftHandSide.evaluator = leftHandSideEvaluator;

    var rightHandSideEvaluator = sinon.stub().returns(true);
    var rightHandSide = Bifrost.rules.Rule.create();
    rightHandSide.evaluator = rightHandSideEvaluator;

    var instance = { something: 42 };
    var rule = Bifrost.rules.And.create({
        leftHandSide: leftHandSide,
        rightHandSide: rightHandSide
    });
    rule.evaluate(instance);

    var satisfied = rule.isSatisfied();

    it("should not be considered satisfied", function () {
        expect(satisfied).toBe(false);
    });
});