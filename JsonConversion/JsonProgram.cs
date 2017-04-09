using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonConversion
{
	public static class JsonProgram
	{
		static void Main()
		{
            string json = Console.In.ReadToEnd();
            //var json = "{\"version\":\"2\",\"products\":{\"1\":{\"name\":\"Pen\",\"price\":12,\"count\":100},\"2\":{\"name\":\"Pencil\",\"price\":8,\"count\":1000},\"3\":{\"name\":\"Box\",\"price\":12.1,\"count\":50}}}";
            var bv3 = ConverJson(json);
		    Console.Write(bv3);
		}

	    public static JObject ConverJson(string json)
	    {
	        JObject v2 = JObject.Parse(json);
	        var products = v2["products"].ToObject<Dictionary<string, JObject>>();
	        var _products = new List<JObject>();
	        foreach (var key in products.Keys)
	        {
	            var obj = products[key];
	            obj.Add("id", int.Parse(key));
	            _products.Add(obj);
	        }

	        if (v2["constants"] == null)
	        {
	            var bv3 = new JObject(
	                new JProperty("version", "3"),
	                new JProperty("products", _products)
	            );

	            return bv3;
            }
	        return null;
	    }
	}
}
