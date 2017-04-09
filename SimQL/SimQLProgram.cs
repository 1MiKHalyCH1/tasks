using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimQLProgram
{
	public class SimQLProgram
	{
		static void Main(string[] args)
		{
            var json = Console.In.ReadToEnd();
            //var json = "{'data':{'a':{'x':3.14,'b':{'c':15},'c':{'c':9}},'z':42},'queries':['a.b.c','z','a.x']}";
            foreach (var result in ExecuteQueries(json))
				Console.WriteLine(result);
		}

		public static IEnumerable<string> ExecuteQueries(string json)
		{
			var jObject = JObject.Parse(json);
			var queries = jObject["queries"].ToObject<string[]>();

		    return queries
		            .Select(q => ExecuteQuery(jObject, "data", q));
		}

        public static string ExecuteQuery(JObject mainObject, string rootName , string query)
        {
            var data = mainObject[rootName];
            try
            {
                var token = data.SelectToken(query).ToString();
                return $"{query} = {token}";
            }
            catch
            {
                return $"{query}";
            }
        }
    }
}
