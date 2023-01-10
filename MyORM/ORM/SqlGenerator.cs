using Assignment4;
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class SqlGenerator<G,T>
    {
        public static List<string> InsertCommand(T item)
        {
            var keyValue = "";
            
            var type = item.GetType();
            var keyName = $"{type.Name}Id";
            

            DataSetOfObject<G, T>.convertObject(item, keyName,keyValue);
            var insertCommands = new List<string>();

            foreach (KeyValuePair<string, List<Dictionary<string, string>>> kvp in DataSetOfObject<G, T>.dataSets)
            {
                //Console.WriteLine($"Table Name: {kvp.Key}");
                //Console.WriteLine("----------------------");
                string sql = "";
                if (kvp.Key[kvp.Key.Length - 1] == 's')
                    sql += $"insert into {kvp.Key}es (";
                else
                    sql += $"insert into {kvp.Key}s (";
                foreach(var list in kvp.Value)
                {
                    foreach (KeyValuePair<string, string> ele in list)
                    {
                        //Console.WriteLine($"{ele.Key} : {ele.Value}");
                        sql += $"[{ele.Key}],";
                    }
                    break;
                    
                }
                sql = sql.Remove(sql.Length - 1);
                sql += ") values";

                foreach (var list in kvp.Value)
                {
                    sql += "(";
                    foreach (KeyValuePair<string, string> ele in list)
                    {
                        //Console.WriteLine($"{ele.Key} : {ele.Value}");
                        sql += $"'{ele.Value}',";
                    }
                    sql=sql.Remove(sql.Length - 1);
                    sql += "), ";
                }

                sql = sql.Remove(sql.Length - 2);
                insertCommands.Add(sql);
            }



            return insertCommands;

        }

        public static List<string> UpdateCommand(T item)
        {
            var keyValue = "";
            var type = item.GetType();
            var keyName = $"{type.Name}Id";
            DataSetOfObject<G, T>.convertObject(item, keyName,keyValue);
            var updateCommands = new List<string>();

            foreach (KeyValuePair<string, List<Dictionary<string, string>>> kvp in DataSetOfObject<G, T>.dataSets)
            {
                //Console.WriteLine($"Table Name: {kvp.Key}");
                //Console.WriteLine("----------------------");
                string sql = "";
                if (kvp.Key[kvp.Key.Length - 1] == 's')
                    sql += $"update {kvp.Key}es set";
                else
                    sql += $"update {kvp.Key}s set";
                
                foreach (var list in kvp.Value)
                {
                    foreach (KeyValuePair<string, string> ele in list)
                    {
                        //Console.WriteLine($"{ele.Key} : {ele.Value}");
                        sql += $"[{ele.Key}] = '{ele.Value}',";
                    }
                    sql = sql.Remove(sql.Length - 1);
                    sql += ", ";
                }

                sql = sql.Remove(sql.Length - 2);
                sql += " where ";
                foreach (var keyList in DataSetOfObject<G, T>.keySets[$"{kvp.Key}"])
                {
                    foreach(KeyValuePair<string, string> ele in keyList)
                    {
                        sql += $"[{ele.Key}]= '{ele.Value}' and ";
                    }
                }
                sql = sql.Remove(sql.Length - 5);
                updateCommands.Add(sql);
            }

            
            return updateCommands;

        }

        public static List<string> DeleteCommand(T item)
        {
            var keyValue = "";
            var type = item.GetType();
            var keyName = $"{type.Name}Id";
            DataSetOfObject<G, T>.convertObject(item, keyName,keyValue);
            var deleteCommands = new List<string>();

            foreach (KeyValuePair<string, List<Dictionary<string, string>>> kvp in DataSetOfObject<G, T>.dataSets)
            {
                //Console.WriteLine($"Table Name: {kvp.Key}");
                //Console.WriteLine("----------------------");
                string sql = "";
                if (kvp.Key[kvp.Key.Length - 1] == 's')
                    sql += $"delete from {kvp.Key}es ";
                else
                    sql += $"delete from {kvp.Key}s ";

                sql += " where ";
                foreach (var keyList in DataSetOfObject<G, T>.keySets[$"{kvp.Key}"])
                {
                    foreach (KeyValuePair<string, string> ele in keyList)
                    {
                        sql += $"[{ele.Key}]= '{ele.Value}' and ";
                    }
                }
                sql = sql.Remove(sql.Length - 5);
                deleteCommands.Add(sql);
            }


            return deleteCommands;

        }

        public static List<string> DeleteByClassCommand(T item)
        {
            var deleteCommands = new List<string>();
            var keyValue = "";
            var type = item.GetType();
            var keyName = $"Id";
            
            var tableName = type.Name;
            if (tableName[tableName.Length - 1] == 's') tableName += "es";
            else tableName += 's';

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static))
            {
                var propertyType = property.PropertyType;
                var key = property.Name;

                if (propertyType == typeof(string) ||
                    propertyType == typeof(Guid) ||
                    propertyType.IsPrimitive)

                { 

                    if (key[key.Length - 2] == 'I' && key[key.Length - 1] == 'd')
                    {
                        keyValue = $"{property.GetValue(item)}";
                    }


                }
            }
            var query = $"delete from {tableName} where [{keyName}] = '{keyValue}'";
            deleteCommands.Add(query);
            keyName = $"{type.Name}Id";
            AccessData<G, T>.GetRelationalKeySet(item.GetType(), keyName, keyValue);
            foreach (var kvp in AccessData<Guid, Course>.foreignKeys)
            {
                tableName = $"{kvp.Key}";
                if (tableName[tableName.Length - 1] == 's') tableName += "es";
                else tableName += 's';
                query = $"delete from {tableName} ";
                foreach (var key in kvp.Value)
                {
                    //Console.WriteLine("1");
                    //Console.WriteLine($"{key.Key} --> {key.Value}");
                    query += $"where [{key.Key}] = '{key.Value}' ";
                }
                deleteCommands.Add(query);
            }
            

            return deleteCommands;
        }

        public static List<string> DeleteByIdCommand(G id)
        {
            var deleteCommands = new List<string>();
            var keyValue = $"{id}";
            var type = typeof(T);
            var keyName = $"Id";

            var tableName = type.Name;
            if (tableName[tableName.Length - 1] == 's') tableName += "es";
            else tableName += 's';

            var query = $"delete from {tableName} where [{keyName}] = '{keyValue}'";
            deleteCommands.Add(query);
            keyName = $"{type.Name}Id";
            AccessData<G, T>.GetRelationalKeySet(type, keyName, keyValue);
            foreach (var kvp in AccessData<Guid, Course>.foreignKeys)
            {
                tableName = $"{kvp.Key}";
                if (tableName[tableName.Length - 1] == 's') tableName += "es";
                else tableName += 's';
                query = $"delete from {tableName} ";
                foreach (var key in kvp.Value)
                {
                    query += $"where [{key.Key}] = '{key.Value}' ";
                }
                deleteCommands.Add(query);
            }


            return deleteCommands;
        }

        public static List<string> GetByIdCommand(G id)
        {
            var getCommands = new List<string>();
            var keyValue = $"{id}";
            var type = typeof(T);
            var keyName = $"Id";

            var tableName = type.Name;
            if (tableName[tableName.Length - 1] == 's') tableName += "es";
            else tableName += 's';

            var query = $"select * from {tableName} where [{keyName}] = '{keyValue}'";
            getCommands.Add(query);
            keyName = $"{type.Name}Id";
            AccessData<G, T>.GetRelationalKeySet(type, keyName, keyValue);
            foreach (var kvp in AccessData<Guid, Course>.foreignKeys)
            {
                tableName = $"{kvp.Key}";
                if (tableName[tableName.Length - 1] == 's') tableName += "es";
                else tableName += 's';
                query = $"select * from {tableName} ";
                foreach (var key in kvp.Value)
                {
                    query += $"where [{key.Key}] = '{key.Value}' ";
                }
                getCommands.Add(query);
            }


            return getCommands;
        }

        public static Dictionary<string,string> GetById(G id)
        {
            var getCommands = new Dictionary<string, string>();
            var keyValue = $"{id}";
            var type = typeof(T);
            var keyName = $"Id";

            var tableName = type.Name;
            if (tableName[tableName.Length - 1] == 's') tableName += "es";
            else tableName += 's';

            var query = $"select * from {tableName} where [{keyName}] = '{keyValue}'";
            getCommands.Add($"{tableName}",query);
            keyName = $"{type.Name}Id";
            AccessData<G, T>.GetRelationalKeySet(type, keyName, keyValue);
            foreach (var kvp in AccessData<Guid, Course>.foreignKeys)
            {
                tableName = $"{kvp.Key}";
                if (tableName[tableName.Length - 1] == 's') tableName += "es";
                else tableName += 's';
                query = $"select * from {tableName} ";
                foreach (var key in kvp.Value)
                {
                    query += $"where [{key.Key}] = '{key.Value}' ";
                }
                getCommands.Add($"{tableName}", query);
            }


            return getCommands;
        }

    }
}
