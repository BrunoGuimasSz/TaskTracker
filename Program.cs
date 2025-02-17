using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace TaskTracker
{
    class Task
    {
        public string description { get; set; }
        public string status { get; set; }
        public int id { get; set; }

        public Task(string description, string status, int id)
        {
            this.description = description;
            this.status = status;
            this.id = id;
        }
    }
    class Program
    {
        static void Main()
        {
            Console.WriteLine("O que você deseja?");
            Console.WriteLine("1 - Criar tarefa\n2 - Deletar tarefa\n3 - Atualizar tarefas\n4 - Listar tarefas\n5 - Sair");

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    AddTask(false);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    // ListTask();
                    LoadTask();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        }
        static void AddTask(bool returnTaskList)
        {
            const string defaultStatus = "todo";

            Console.Clear();
            Console.WriteLine("Qual a descrição da tarefa?");
            string description = Console.ReadLine();

            List<Task> TaskList = new List<Task>();
            int id = TaskList.Count + 1;
            TaskList.Add(new Task(description, defaultStatus, id));

            Console.WriteLine("\nDigite qualquer tecla para continuar");
            Console.ReadKey();
            
            SaveTask(TaskList);
        }
        // static void ListTask()
        // {
        //     List<Task> taskList = LoadTask();

        //     Console.Clear();
        //     Console.WriteLine("ID | DESCRIÇÃO | STATUS");
        //     foreach (var task in taskList)
        //     {
        //         Console.WriteLine(task.Id + " - \"" + task.description + "\" - " + task.status);
        //     }

        //     Console.WriteLine("\nDigite qualquer tecla para continuar");
        //     Console.ReadKey();
        //     Main();
        // }
        static void LoadTask()
        {
            var jsonFile = File.ReadAllText("TaskListSave");
            var jsonList = JsonSerializer.Deserialize<Task>(jsonFile);
            Console.WriteLine(jsonFile);
        }
        static void SaveTask(List<Task> TaskList)
        {
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonList = JsonSerializer.Serialize(TaskList, jsonOptions);
            LoadTask();
            File.WriteAllText("TaskListSave", jsonList);
        }
    }
}
