<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Driver.Linq</Namespace>
</Query>

void Main()
{
	var entities = new MongoClient()
		.GetDatabase("test")
		.GetCollection<Entity>("test")
		.AsQueryable();
		
	// if we know Id is the property to sort by, we hard-code it
	entities.OrderBy(e => e.Id);
		
	IQueryable query = from p in entities
					   where p.Id > 2
					   select p;
					   
	// these values are coming from the UI
	string sortFromUI = nameof(Entity.Id);
	SortDirection direction = SortDirection.Desc;
	
	
	ParameterExpression purchaseParam = Expression.Parameter(typeof(Entity), "p");
	MemberExpression member = Expression.PropertyOrField(purchaseParam, sortFromUI);
	LambdaExpression lambda = Expression.Lambda(member, purchaseParam);
	
	Type[] exprArgTypes = { query.ElementType, lambda.Body.Type };
	
	string methodName = direction == SortDirection.Asc ?
		nameof(Queryable.OrderBy) :
		nameof(Queryable.OrderByDescending);
	MethodCallExpression methodCall = Expression.Call(typeof(Queryable), methodName, exprArgTypes, query.Expression, lambda);
	
	IQueryable orderedQuery = query.Provider.CreateQuery(methodCall);
	orderedQuery.Dump();
}

// Define other methods and classes here
public class Entity
{
	public long Id { get; set; }
}

public enum SortDirection : byte
{
	Asc,
	Desc
}