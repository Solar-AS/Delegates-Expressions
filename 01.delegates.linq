<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	typeof(toString).Dump("delegates are types, classes to be more precise");
	
	// variables can be of a given delegate
	toString variable;
	
	// compatible methods can be assigned to such variables
	variable = compatibleMethod;
	// the type can be instantiated (not that common)
	variable = new toString(compatibleMethod);
	
	// incompatible methods can't be assigned --> type safety
	//variable = incompatibleMethod;
	
	variable.Invoke(42).Dump("variables of a given delegate type can be invoked");
	variable(int.MinValue).Dump("shortcut invocation");
	
	// instances of delegates can be passed as argument
	bigNumberPrinter(variable);
	bigNumberPrinter(compatibleMethod);
	bigNumberPrinter(delegate (int x)
	{ 
		return "old school in-line anonymous method passing";
	});

	// delegates can be composed
	toString usingArg = new toString(delegate (int x) { return $"using_arg_{x}".Dump(); }),
		notUsingArg = new toString(delegate (int x) { return "not_using_arg".Dump(); });
	toString composed = usingArg + notUsingArg;
	composed(1).Dump("composition returns last in invocation list");
	composed.GetInvocationList().Dump("multi invocation");
	variable.GetInvocationList().Dump("single invocation");
	
	Delegate lessSafe = variable;
	lessSafe.DynamicInvoke(42).Dump("lucky this time");
	lessSafe.DynamicInvoke("4").Dump("not so much now --> Exception");
	
	MulticastDelegate multi = variable;
}

// Define other methods and classes here
delegate string toString(int x);

string compatibleMethod(int y)
{
	return "the number is " + y.ToString(CultureInfo.InvariantCulture);
}

int incompatibleMethod(int a)
{
	return 0;	
}

void bigNumberPrinter(toString printer)
{
	printer(int.MaxValue).Dump("invocation of a parameter");
}