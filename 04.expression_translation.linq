<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Driver.Linq</Namespace>
</Query>

void Main()
{
	IMongoQueryable<Entity> query = new MongoClient()
			.GetDatabase("test")
			.GetCollection<Entity>("test")
			.AsQueryable();
			
	Expression<Func<Entity, bool>> greatherThanTwo = i => i.Id > 2;
	
	query.Where(greatherThanTwo.Dump("Expression", 1))
		.ToString().Dump("Query");
}

// Define other methods and classes here
public class Entity
{
	public long Id { get; set; }
}
