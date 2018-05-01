<Query Kind="Program" />

void Main()
{
	ValueTypePredicate<int> p1 = delegate (int x) { return false; };
	p1(21).Dump("custom");
	
	// better syntax through lambda functions
	Predicate<int> p2 = x => false;
	p2(22).Dump("system");
	
	// discard syntax
	Func<int, bool> p3 = _ => false;
	p3(24).Dump("system preferred");
	
	// signature compatibility does not make delegates compatible
	//p2 = p3;
	// neither does casting
	// p2 = (Predicate<int>)p3;
	// or cheat casting
	//p2 = (Predicate<int>)((object)p3);
	
	// a "conversion" would do the trick, though
	p2 = new Predicate<int>(p3);
	p2(0).Dump("converted");
}

// Define other methods and classes here
delegate bool ValueTypePredicate<T>(T arg) where T : struct;
