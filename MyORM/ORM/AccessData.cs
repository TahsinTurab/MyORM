using Assignment4;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class AccessData<G,T>
    {
        public static Dictionary<string, List<Dictionary<string, string>>> keySets =
            new Dictionary<string, List<Dictionary<string, string>>>();

        public static Dictionary<string, List<Dictionary<string, string>>> dataSets =
            new Dictionary<string, List<Dictionary<string, string>>>();

        public static Dictionary<string, Dictionary<string, string>> foreignKeys = new Dictionary<string, Dictionary<string, string>>();
        
        public static void GetDataById(G id)
        {
            var tablename = typeof(T).Name;
            var idName = $"{tablename}Id";
            var idValue = $"{id}";
            var data = new Dictionary<string, string>();
            data.Add(idName,idValue);
            var list = new List<Dictionary<string, string>>();
            list.Add(data);
            keySets.Add(tablename, list);

            GetRelationalKeySet(typeof(T), idName, idValue);

        }

        public static void GetRelationalKeySet(Type classType, string idName, string idValue)
        {
            try
            {

                foreach (var property in classType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.Static))
                {
                    var tempDic = new Dictionary<string, string>();
                    tempDic.Add(idName, idValue);
                    var propertyType = property.PropertyType;
                    var key = property.Name;
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition()
                        == typeof(List<>))
                    {
                        Type itemType = propertyType.GetGenericArguments()[0];
                        //Console.WriteLine(itemType.Name);
                        var tableName = $"{itemType.Name}";
                        if (tableName[tableName.Length - 1] == 's') tableName += "es";
                        else tableName += 's';
                        //if (idValue != "") {

                        //    if(foreignKeys.ContainsKey($"{itemType.Name}")==false)
                        //        foreignKeys.Add($"{itemType.Name}", tempDic);
                        //}
                        if (foreignKeys.ContainsKey($"{itemType.Name}") == false)
                            foreignKeys.Add($"{itemType.Name}", tempDic);
                        var newIdName = $"{itemType.Name}Id";
                        var query = $"select Id from {tableName} where {idName}='{idValue}'";
                        var newIdData = DataUtility.ExecuteGetIdQuery(query);
                        var newIdValue = newIdData["Id"];
                        GetRelationalKeySet(itemType, newIdName, newIdValue);
                    }
                    else if (propertyType.IsClass && propertyType != typeof(string))
                    {
                        //Console.WriteLine(propertyType.Name);
                        var tableName = $"{propertyType.Name}";
                        if (tableName[tableName.Length - 1] == 's') tableName += "es";
                        else tableName += 's';
                        //if (idValue != "")
                        //{

                        //    if (foreignKeys.ContainsKey($"{propertyType.Name}") == false)
                        //        foreignKeys.Add($"{propertyType.Name}", tempDic);
                        //}
                        if (foreignKeys.ContainsKey($"{propertyType.Name}") == false)
                            foreignKeys.Add($"{propertyType.Name}", tempDic);
                        var newIdName = $"{propertyType.Name}Id";
                        var query = $"select Id from {tableName} where [{idName}]='{idValue}'";
                        var newIdData = DataUtility.ExecuteGetIdQuery(query);
                        var newIdValue = newIdData["Id"];
                        GetRelationalKeySet(propertyType, newIdName, newIdValue);

                    }

                }
            }
            catch
            {
                Console.WriteLine("Your given data is absent");
            }


        }
        public static void GetAllDatafromTable()
        {
            var tableName = typeof(T).Name;
            
            if (tableName[tableName.Length - 1] == 's')
                tableName += "es";
            else 
                tableName += 's';

            var query = $"select * from {tableName}";
            var list = DataUtility.ExecuteQuery(query);
            dataSets.Add(tableName, list);
        }


    }
}
