# CalcFileExpressions
Simple demonstration of interaction with windows calculator

This app is just for short demonstration of Automation abilities in .NET.
Not all code is covered with tests but it's a way to future improvements.

In code you can find several implementations of the interfaces - it's just for example.

For example you can use:

`private IExpressionEvaluator _expressionEvaluator = new SilentExpressionEvaluator();`

or

`private readonly IExpressionEvaluator _expressionEvaluator = new ScreenCalcExpressionEvaluator();`


# Run app prerequisites
- Before using of calculator interaction please run it and make sure if it is in the basic view.
- **0** should be on the calculator screen.
- In the App.config, set "CalcName" to be equal to a caption on the your calculator's window 

# Extensibility
It is possible to use any evaluation engine, any parsing expressions engine

# Future steps
- DI to be added.
- More tests
- Automatically choose calculator type
- Replace regex parsing with a proper grammar parsing (see BNF)
- Maybe graphic recognition feature for handling calc


# Notes
It was an option to use ETL-pattern but it looks too complicated here

# Changelog
**v. 1.0.1.0** - Added support of a Windows 10 calculator. See *ScreenMetroCalcExpressionEvaluator*


