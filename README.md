# CalcFileExpressions
Simple demonstration of interaction with windows calculator

This app is just for short demonstration of Automation abilities in .NET.
Not all code is covered with tests but it's a way to future improvements.

In code you can find several implementations of the interfaces - it's just for example.

For example you can use:

`private IExpressionEvaluator _expressionEvaluator = new SilentExpressionEvaluator();`

or

`private readonly IExpressionEvaluator _expressionEvaluator = new ScreenCalcExpressionEvaluator();`


DI to be added.


# Run
Before using of calculator interaction please run it and make sure if it is in the basic view.
Also **0** should be on the calculator screen.

