<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

Func<char, string> anonymousMethodDelegate = delegate (char c) { return $"delegate: {c}";};
Func<char, string> lambdaDelegate = c => $"lambda: {c}";

// expressions can only be built using lambda syntax
Expression<Func<char, string>> expression = c => $"expression: {c}";

anonymousMethodDelegate.Dump("anonymous method");
lambdaDelegate.Dump("lambda delegate");

expression.Dump("expression");

Expression<Func<string, bool>> f = s => s.Length < 5;
f.Body.Type.Dump("return type");
f.Body.NodeType.Dump("<");
(f.Body as BinaryExpression).Left.Dump("Length property accessor");
((f.Body as BinaryExpression).Left as MemberExpression).Member.Name.Dump("Property Name");
(f.Body as BinaryExpression).Right.Dump("5");

f.Dump("binary expression");