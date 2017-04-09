using System;
using ExpressionEvaluator;

namespace EvalTask
{
	class EvalProgram
	{
		static void Main(string[] args)
		{
            //string input = Console.In.ReadToEnd();
		    string input = Console.ReadLine();
		    input = input.Replace("-", " - ");
            var expresion = new CompiledExpression(input);
		    var output = expresion.Eval();
			Console.WriteLine(output);
		}
	}
}
