using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assignment4
{
    public class DataSetOfObject<G,T>
    {

        public static Dictionary<string, List<Dictionary<string, string>>> dataSets = 
            new Dictionary<string, List<Dictionary<string, string>>>();

        public static Dictionary<string, List<Dictionary<string, string>>> keySets =
            new Dictionary<string, List<Dictionary<string, string>>>();

        public static Dictionary<string, Dictionary<string, string>> foreignKeys = new Dictionary<string, Dictionary<string, string>>();
        public static void convertObject(object instance,string keyName, string keyValue)
        {

            var type = instance.GetType();
            var tempList = new List<Dictionary<string, string>>();
            var tempDictionary = new Dictionary<string, string>();
            var tempKeyList = new List<Dictionary<string, string>>();
            var tempKeyDictionary = new Dictionary<string, string>();
            //var keyName = $"{type.Name}Id";
            //var keyValue = "";
            if(keyValue != "") tempDictionary.Add(keyName, keyValue);
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static))
            {
                var propertyType = property.PropertyType;
                var key = property.Name;

                if (propertyType == typeof(string) || 
                    propertyType == typeof(Guid) ||
                    propertyType.IsPrimitive)

                {

                    tempDictionary.Add(key, $"{property.GetValue(instance)}");

                    if (key[key.Length - 2] == 'I' && key[key.Length - 1] == 'd')
                    {
                        tempKeyDictionary.Add(key, $"{property.GetValue(instance)}");
                        keyName = $"{type.Name}Id";
                        keyValue = $"{property.GetValue(instance)}";
                    }


                }

                else if(propertyType == typeof(DateTime))
                {
                    var value = (DateTime)property.GetValue(instance);
                    string valueString = value.ToString();
                    tempDictionary.Add(key, valueString);
                }

                else if(propertyType.IsClass)
                {
                    var list = property.GetValue(instance);
                    var tt = list.GetType();
                    var objType = tt.GetGenericArguments().FirstOrDefault();
                    if (objType != null)
                    {
                        var IListRef = typeof(List<>);
                        Type[] IListParam = { objType };
                        object Result = Activator.CreateInstance(IListRef.MakeGenericType(IListParam));
                        Result = property.GetValue(instance);

                        IList collection = (IList)Result;

                        for (int i = 0; i < collection.Count; i++)
                        {
                            convertObject(collection[i], keyName,keyValue);
                        }

                    }
                    else
                    {
                        var obj = Activator.CreateInstance(propertyType);
                        obj = property.GetValue(instance);
                        convertObject(obj, keyName,keyValue);
                    }
                }
            }
            tempList.Add(tempDictionary);
            tempKeyList.Add(tempKeyDictionary);
            if(dataSets.ContainsKey($"{type.Name}"))
            {
                dataSets[$"{type.Name}"].Add(tempDictionary);
            }
            else
            {
                dataSets.Add($"{type.Name}", tempList);
            }

            if (keySets.ContainsKey($"{type.Name}"))
            {
                keySets[$"{type.Name}"].Add(tempKeyDictionary); 
            }

            else
            {
                keySets.Add($"{type.Name}", tempKeyList);
            }

        }

       
        public static void GetAllDatafromDB()
        {
            var getAllCommand = new List<string>();
            foreach (var i in DataUtility.TableList())
            {
                string sql = $"select * from {i}";
                var listofData = DataUtility.ExecuteQuery(sql);
                dataSets.Add($"{i}",listofData);
            }
        }
    }


}
