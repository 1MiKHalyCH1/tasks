using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionEvaluator;
using Newtonsoft.Json.Linq;

namespace EvalTask
{
    public class EvalProgram
    {

        public static string[] GetVariablesFromExpression(string expression)
        {
            return expression.Split(new char[] {' ', '+', '-', '*', '/', '(', ')', '{', '}', ',', '.','1','2','3','4','5','6','7','8','9','0', '\r'},
                    StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToArray();
        }



        public static string EvaluateStringExpression(string expression)
        {
            expression = expression.Replace("-", " - ");
            var compiledExpression = new CompiledExpression(expression);
            var result = compiledExpression.Eval();
            return result.ToString();
        }


        public static string EvaluateStringExpression(string expression, Dictionary<string, string> variablesDictionary)
        {
            expression = expression.Replace("-", " - ");
            foreach (var v in variablesDictionary.OrderByDescending(x => x.Key.Length))
            {
                expression = expression.Replace(v.Key, v.Value);
            }
            var compiledExpression = new CompiledExpression(expression);
            var result = compiledExpression.Eval();
            return result.ToString();
        }

        public static Dictionary<string, string> GetDictionaryWihVariablesFromJson(JObject json, string[] variables)
        {
            var jObject = new JObject(new JProperty("data", json));
            var dictionary = new Dictionary<string, string>();

            foreach (var variable in variables)
            {
                var value = SimQLProgram.SimQLProgram.ExecuteQuery(jObject, "data", variable);
                var temp = value.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                dictionary.Add(temp[0], temp[1]);
            }
            return dictionary;
        }

        static void Main(string[] args)
        {
            try
            {
                var input = Console.In.ReadToEnd().Split('\n');

                string expression = input[0];


                var jsonInput = String.Join("\n", input.Skip(1).ToArray());
                if (jsonInput == "")
                    jsonInput = "{}";
                //var jsonInput = "{'a':50, 'b':45}";
                var jObject = JObject.Parse(jsonInput);
                var variables = GetVariablesFromExpression(expression);
                var dictionary = GetDictionaryWihVariablesFromJson(jObject, variables);
                var result = EvaluateStringExpression(expression, dictionary);
                double f;
                var answer = Double.TryParse(result, out f) ? "error":result;
                Console.WriteLine(result);
            }
            catch
            {
                Console.WriteLine("error");
            }
        }
    }
}
