using System;
using ExpressionEvaluator;

namespace EvalTask
{
	class EvalProgram
	{
		static void Main(string[] args)
		{
            //string input = Console.In.ReadToEnd();
		    string expresionInput = Console.ReadLine();
		    expresionInput = expresionInput.Replace("-", " - ");

		    var variables = expresionInput.Split(new char[] {' ', '+', '-', '*', '/', '(', ')', '{', '}',',','.'}, StringSplitOptions.RemoveEmptyEntries);

		  //  var json = Console.In.ReadToEnd();
		    var json = @"{
		    a: 5
		}";
		    foreach (var result in SimQLProgram.SimQLProgram.ExecuteQueries(json))
		        Console.WriteLine(result);

          //  var expresion = new CompiledExpression(expresionInput);
		 //   var output = expresion.Eval();
		//	Console.WriteLine(output);
		}
	}
}
