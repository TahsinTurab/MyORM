using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ORM;

namespace Assignment4
{
    public class MyORM<G,T>
    {

        public void Insert(T item)
        {
            var insertCommands = SqlGenerator<G, T>.InsertCommand(item);
            for (int i = insertCommands.Count-1; i>=0;i--)
            {
                //Console.WriteLine(insertCommands[i]);
                //Console.WriteLine();
                DataUtility.ExecuteCommand(insertCommands[i]);
            }
            Console.WriteLine("Data inserted successfully");
        }

        public void Update(T item)
        {
            var updateCommands = SqlGenerator<G, T>.UpdateCommand(item);
            for (int i = updateCommands.Count - 1; i >= 0; i--)
            {
                //Console.WriteLine(updateCommands[i]);
                //Console.WriteLine();
                DataUtility.ExecuteCommand(updateCommands[i]);
            }
            Console.WriteLine("Updatation Completed Successfully");

        }

        public void Delete(T item)
        {
            var deleteCommands=SqlGenerator<G,T>.DeleteByClassCommand(item);
            for(int i = deleteCommands.Count - 1; i >= 0; i--)
            {
                //Console.WriteLine(deleteCommands[i]);
                DataUtility.ExecuteCommand(deleteCommands[i]);
            }
            Console.WriteLine("Deleted Successfully");
        }

        public void Delete(G id)
        {
            var deleteCommands = SqlGenerator<G, T>.DeleteByIdCommand(id);
            for (int i = deleteCommands.Count - 1; i >= 0; i--)
            {
                //Console.WriteLine(deleteCommands[i]);
                DataUtility.ExecuteCommand(deleteCommands[i]);
            }
            Console.WriteLine("Deleted Successfully");


        }

        public void GetById(G id)
        {
            var getCommands = SqlGenerator<G, T>.GetById(id);
            foreach(var command in getCommands)
            {
                //Console.WriteLine(command.Value);
                Console.WriteLine($"{command.Key}");
                Console.WriteLine("***********************************");
                var dataSets = DataUtility.ExecuteQuery(command.Value);
                foreach (var dataSet in dataSets)
                {
                    foreach (var kvp in dataSet)
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                    Console.WriteLine("-----------------------------------");
                }
                //Console.WriteLine("***********************************");
            }
        }

        public void GetAll()
        {
            //DataSetOfObject<G,T>.GetAllDatafromDB();
            AccessData<G, T>.GetAllDatafromTable();
            foreach (KeyValuePair<string, List<Dictionary<string, string>>> kvp in AccessData<G,T>.dataSets)
            {
                Console.WriteLine($"Table Name: {kvp.Key}");
                Console.WriteLine("*************************************************************************");
                foreach (var list in kvp.Value)
                {
                    foreach (KeyValuePair<string, string> ele in list)
                    {
                        Console.WriteLine($"{ele.Key} : {ele.Value}");
                    }
                    Console.WriteLine("*************************************************************************");
                }

                Console.WriteLine("----------------------");
            }
        }

    }
}
