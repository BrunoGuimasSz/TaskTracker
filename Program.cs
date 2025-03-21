using System.Collections.Generic;
using System.Text.Json;
using Cocona;
using ConsoleTables;

var app = CoconaApp.Create();

app.AddCommand("add", ([Argument] string description) =>
    TaskTracker.Program.AddTask(description))
    .WithDescription("Creates a new task");

app.AddCommand("remove", ([Argument] string id) =>
    TaskTracker.Program.DeleteTask(id))
    .WithDescription("Remove a task");

app.AddCommand("list", ([Option('s', Description = "Only show tasks with a specific status")] string? status) =>
    TaskTracker.Program.ListTask(status))
    .WithDescription("List all tasks");

app.AddCommand("mark-todo", ([Argument] string id) =>
    TaskTracker.Program.MarkTask(id, "todo"))
    .WithDescription("Changes the status of a task");

app.AddCommand("mark-in-progress", ([Argument] string id) =>
    TaskTracker.Program.MarkTask(id, "in-progress"))
    .WithDescription("Changes the status of a task");

app.AddCommand("mark-done", ([Argument] string id) =>
    TaskTracker.Program.MarkTask(id, "done"))
    .WithDescription("Changes the status of a task");

app.AddCommand("update", ([Argument] string id, [Argument] string description) =>
    TaskTracker.Program.UpdateTask(id, description))
    .WithDescription("Changes the description of a task");
app.Run();

namespace TaskTracker
{
    class Constants
    {
        public const string DEFAULT_STATUS = "todo";
        public const string DEFAULT_UPDATED_AT_DATE_TIME = "never";
        public const string TASK_LIST_SAVE_PATH = "TaskListSave.json";
    }

    class Task
    {
        public string Description { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public Task(string description, string status, int id, string createdAt, string updatedAt)
        {
            this.Description = description;
            this.Status = status;
            this.Id = id;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
        }
    }

    class Program
    {
        public static void AddTask(string description)
        {
            if(description.Length > 80)
            {
                Console.Clear();
                Console.WriteLine("Error: Description should have a maximum of 80 characters");
                return;
            }

            description = description.Replace("\n", "");

            List<Task> TaskList = LoadTask();
            int id = GenerateTaskId(TaskList);
            string createdAt = DateTime.Now.ToString();
            TaskList.Add(new Task(description, Constants.DEFAULT_STATUS, id, createdAt, Constants.DEFAULT_UPDATED_AT_DATE_TIME));

            Console.Clear();
            Console.WriteLine($"Task {description} created with success! Id {id}");

            SaveTask(TaskList);
        }

        public static void DeleteTask(string idToRemove)
        {
            List<Task> TaskList = LoadTask();
            int taskIndex = GetTaskIndex(Convert.ToInt32(idToRemove), TaskList);
            if(taskIndex == -1)
            {
                Console.Clear();
                Console.WriteLine("Error: Task not found");
                return;
            }
            string taskDescription = TaskList[taskIndex].Description;
            TaskList.RemoveAt(taskIndex);

            Console.Clear();
            Console.WriteLine($"Task \"{taskDescription}\" deleted with sucess!");

            SaveTask(TaskList);
        }

        public static List<Task> LoadTask()
        {
            if(!File.Exists(Constants.TASK_LIST_SAVE_PATH))
            {
                File.WriteAllText(Constants.TASK_LIST_SAVE_PATH, null);
                return new List<Task>();
            }

            var jsonFile = File.ReadAllText(Constants.TASK_LIST_SAVE_PATH);

            if(jsonFile == "" || jsonFile == "[]")
                return new List<Task>();

            var jsonFilePath = File.ReadAllText(Constants.TASK_LIST_SAVE_PATH);
            return JsonSerializer.Deserialize<List<Task>>(jsonFilePath) ?? new List<Task>();
        }

        static void SaveTask(List<Task> TaskList)
        {
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonList = JsonSerializer.Serialize(TaskList, jsonOptions);

            File.WriteAllText(Constants.TASK_LIST_SAVE_PATH, jsonList);
        }

        public static void ListTask(string status)
        {
            HashSet<string> statusList = new HashSet<string> { "todo", "in-progress", "done"};

            List<Task> TaskList = LoadTask();

            var jsonFile = File.ReadAllText("TaskListSave.json");

            if(!File.Exists(Constants.TASK_LIST_SAVE_PATH) || jsonFile == "" || jsonFile == "[]")
            {
                Console.Clear();
                Console.WriteLine("No tasks found");
                return;
            }
            if(!statusList.Contains(status) && status != null)
            {
                Console.Clear();
                Console.WriteLine("Error: Invalid status");
                return;
            }

            Console.Clear();
            var table = new ConsoleTable("Id", "Description", "Status", "Created at", "Updated at");
            foreach (var task in TaskList)
            {
                if(task.Status == status || status == null)
                    table.AddRow(task.Id, task.Description, task.Status, task.CreatedAt, task.UpdatedAt);
            }
                    table.Write();
        }

        public static void MarkTask(string id, string status)
        {
            List<Task> TaskList = LoadTask();
            int taskIndex = GetTaskIndex(Convert.ToInt32(id), TaskList);
            if(taskIndex == -1)
            {
                Console.Clear();
                Console.WriteLine("Error: Task not found");
                return;
            }
            TaskList[taskIndex].Status = status;

            DateTime updatedAt = DateTime.Now;
            TaskList[taskIndex].UpdatedAt = updatedAt.ToString();

            Console.Clear();
            Console.WriteLine($"Task {id} marked as {status}");

            SaveTask(TaskList);
        }

        public static void UpdateTask(string id, string description)
        {
            List<Task> TaskList = LoadTask();
            int taskIndex = GetTaskIndex(Convert.ToInt32(id), TaskList);
            if(taskIndex == -1)
            {
                Console.Clear();
                Console.WriteLine("Error: Task not found");
                return;
            }

            string oldDescription = TaskList[taskIndex].Description;

            TaskList[taskIndex].Description = description;
            DateTime updatedAt = DateTime.Now;
            TaskList[taskIndex].UpdatedAt = updatedAt.ToString();

            Console.Clear();
            Console.WriteLine($"Task {id} changed from \"{oldDescription}\" to \"{TaskList[taskIndex].Description}\"!");

            SaveTask(TaskList);
        }

        static int GetTaskIndex(int id, List<Task> TaskList)
        {
            return TaskList.FindIndex(task => task.Id == id);
        }

        static int GenerateTaskId(List<Task> TaskList)
        {
            int maxId = 0;

            foreach(var task in TaskList)
                if(task.Id > maxId)
                    maxId = task.Id;

            return maxId + 1;
        }
    }
}
