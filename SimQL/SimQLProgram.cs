using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimQLProgram
{
    public class SimQLProgram
    {
        static void Main(string[] args)
        {
            var json = Console.In.ReadToEnd();
            //var json ="{\"data\":{\"empty\":[],\"x\":[0.1,0.2,0.3],\"a\":[{\"b\":10,\"c\":[1,2,3]},{\"b\":30,\"c\":[4]},{\"d\":500}]},\"queries\":[\"sum(empty)\",\"sum(a.b)\",\"sum(a.c)\",\"sum(a.d)\",\"sum(x)\"]}";
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

        public static string ExecuteQuery(JObject mainObject, string rootName, string query)
        {
            var data = mainObject[rootName];
            var regexp = new Regex(@"(.*)\((.*)\)");
            string operation = null;
            var methods = new Dictionary<string, Func<IEnumerable<double>, double>>
            {
                ["sum"] = Sum,
                ["min"] = Min,
                ["max"] = Max
            };


            if (regexp.IsMatch(query))
            {
                var match = regexp.Match(query);
                operation = match.Groups[1].ToString();
                query = match.Groups[2].ToString();
            }
            try
            {
                if (operation == "")
                {
                    var token = data.SelectToken(query);
                    return $"{query} = {token}";
                }
                else
                {
                    var method = methods[operation];

                    var dotIndex = query.LastIndexOf('.');
                    if (dotIndex == -1)
                    {
                        var token = data.SelectToken(query);
                        var arr = token as JArray;
                        return arr == null
                            ? GetResult(query, method, token)
                            : GetResult(query, method, arr);
                    }
                    var firstQueryPart = query.Substring(0, dotIndex);
                    var secondQueryPart = query.Substring(dotIndex + 1);

                    var firstPart = data.SelectToken(firstQueryPart) as JArray;
                    return GetAll(secondQueryPart, method, firstPart);
                }
            }
            catch
            {
                return $"{query}";
            }
        }

        private static double Max(IEnumerable<double> data) => data.Max();

        private static double Min(IEnumerable<double> data) => data.Min();

        private static double Sum(IEnumerable<double> data) => data.Sum();

        private static double? TryParseDoubleFromJtoken(JToken token)
        {
            double value;
            if (double.TryParse(token.ToString(), out value))
                return value;
            return null;
        }

        private static List<double> TryParseListFromJtoken(string query, JArray token)
        {
            var results = new List<double>();
            if (query == "")
                results.AddRange(token.ToObject<List<double>>());
            else
                results.AddRange(token.Select(e => double.Parse(e.ToString())));
            return results;
        }

        private static string GetResult(string query, Func<IEnumerable<double>, double> method , JToken token)
        {
            var doubleFromJtoken = TryParseDoubleFromJtoken(token);
            return doubleFromJtoken != null ?
                $"{query} = {method(new List<double> { doubleFromJtoken.Value })}" :
                $"{query}";
        }

        private static string GetResult(string query, Func<IEnumerable<double>, double> method, JArray token)
        {
            if (!token.HasValues) return $"{query}";
            return $"{query} = {method(TryParseListFromJtoken(query, token))}";
        }

        private static string GetAll(string query, Func<IEnumerable<double>, double> method, JArray token)
        {
            var results = new List<double>();
            foreach (var e in token)
            {
                try
                {
                    results.Add(double.Parse(e.SelectToken(query).ToString()));
                }
                catch (FormatException eX)
                {
                    var aa = e.SelectToken(query);
                    var arr = aa.ToObject<List<double>>();
                    results.AddRange(arr);
                }
                catch
                {
                    // ignored
                }
            }
            return $"{query} = {method(results)}";
        }
    }
}