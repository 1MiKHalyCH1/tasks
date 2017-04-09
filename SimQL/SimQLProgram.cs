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
            foreach (var result in ExecuteQueries(json))
				Console.WriteLine(result);
		}

		public static IEnumerable<string> ExecuteQueries(string json)
		{
			var jObject = JObject.Parse(json);
			var data = (JObject)jObject["data"];
			var queries = jObject["queries"].ToObject<string[]>();

		    return queries
		            .Select(q => ExecuteQuery(data, q));
		}

        private static string ExecuteQuery(JObject data, string query)
        {
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
