<Query Kind="Program">
  <NuGetReference>Testing.Commons</NuGetReference>
  <Namespace>Testing.Commons.Time</Namespace>
</Query>

void Main()
{
	EventEdit subject = new EventEdit
	{
		Start = 1.April(2018),
		End = 30.April(2018),
		
		EnableX = true,
		X_Start = 1.April(2018),
		X_End = 7.April(2018),
		
		EnableY = true,
		Y_Start = 1.March(2018),
		Y_End = 30.June(2018)
	};
	
	DateTimeOffset start = subject.Start;
	start.Dump("direct invocation");
	
	Func<EventEdit, DateTimeOffset> getter = e => e.Start;
	getter(subject).Dump("indirect invocation");
	
	Expression<Func<EventEdit, DateTimeOffset>> getterExpression = e => e.Start;
	getterExpression.Dump("how does invoking a property look like?");

	/* ------------- REAL WORLD® ------------- */

	/* FROM CONFIGURATION:

<property name="EnableX">
	<validator
		startName="X_Start"
		endName="X_End"
		lifespanStart="Start"
		lifespanEnd="End"
		type="OptionalBoundedPeriodValidator`1[[EventEdit]]"
		name="OptionalX" />
</property>
				
<property name="EnableY">
	<validator
		startName="Y_Start"
		endName="Y_End"
		lifespanStart="Start"
		lifespanEnd="End"
		type="OptionalBoundedPeriodValidator`1[[EventEdit]]"
		name="OptionalY" />
</property>

*/
	// property getters (generated once and ready to use throughout app)
	Func<EventEdit, DateTimeOffset> LifeTimeStart, LifeTimeEnd, OptionalStart, OptionalEnd;
	Func<EventEdit, bool> EnableOptionalPeriod;
	
	// property names would come from config
	LifeTimeStart = CodeGenerator.Getter<EventEdit, DateTimeOffset>("Start");
	LifeTimeEnd = CodeGenerator.Getter<EventEdit, DateTimeOffset>("End");
	
	EnableOptionalPeriod = CodeGenerator.Getter<EventEdit, bool>("EnableX");
	OptionalStart = CodeGenerator.Getter<EventEdit, DateTimeOffset>("X_Start");
	OptionalEnd = CodeGenerator.Getter<EventEdit, DateTimeOffset>("X_End");
	
	// time for validation
	DateTimeOffset beginning = LifeTimeStart(subject), end =  LifeTimeEnd(subject);
	Debug.Assert(beginning <= end, "illegal event lifespan");
	if (EnableOptionalPeriod(subject))
	{
		DateTimeOffset lowerBound = OptionalStart(subject), upperBound = OptionalEnd(subject);
		Debug.Assert(lowerBound >= beginning && lowerBound <= end, "lowerbound outside lifespan");
		Debug.Assert(upperBound >= beginning && upperBound <= end, "upperbound outside lifespan");
		Debug.Assert(lowerBound <= upperBound, "optional period backwards");
	}
	
}

class CodeGenerator
{
	public static Func<TSubject, TPropertyType> Getter<TSubject, TPropertyType>(string propertyName)
	{
		ParameterExpression param = Expression.Parameter(typeof(TSubject), "e");
		MemberExpression prop = Expression.Property(param, propertyName);
		LambdaExpression lambda = Expression.Lambda(prop, param);
		Delegate @delegate = lambda.Compile();
		return (Func<TSubject, TPropertyType>)@delegate;
	}
}




public class EventEdit
{
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }
	
	public bool EnableX { get; set;}
	public DateTimeOffset X_Start { get; set; }
	public DateTimeOffset X_End { get; set; }

	public bool EnableY { get; set; }
	public DateTimeOffset Y_Start { get; set; }
	public DateTimeOffset Y_End { get; set; }
}