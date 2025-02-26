# TaskTracker: Task Manager App

## Description

TaskTracker is a CLI program 100% made in C#, that allows you to manage simple daily life tasks. The program allows you to add, remove, update or mark tasks as "done", "in progress" or "to do", after all that, you can list all tasks. Managing tasks is easier than never with a nice CLI.
This project is part of the Backend roadmap by https://roadmap.sh/projects/task-tracker.

## Features

TaskTracker has the following features:

- **Add task** - Users can add a task with a description, it will be created with the default status "todo" and receive a unique id.
- **Update task** - Users can update a task description, changing the task description to whatever they want.
- **Mark task** - Users can mark a task as "done", "in-progress" or "todo".
- **List tasks** - Users can list all tasks, or filter by status.
- **Remove task** - Users can remove a task by its id.

## Installation

Installation can be simply done by cloning the repository:
```
$ git clonehttps://github.com/BrunoGuimasSz/TaskTracker.git
```

## Usage

- **Add task** 
```
$ dotnet run add "buy groceries"
Task buy groceries created with success! Id 198
```
- **Update task**
```
$ dotnet run update 198 "buy groceries and fruits"
Task 607 changed from "Buy groceries" to "buy groceries and fruits"!
```
- **Mark task**
```
$ dotnet run mark 198 done
Task 198 marked as done
```
- **List tasks**
```
$ dotnet run list
ID  |           DESCRIPTION           |    STATUS   |      CREATED AT     |      UPDATED AT    
607   buy groceries and fruits          done          26/02/2025 10:48:07   26/02/2025 10:50:43
283   Fill up the car                   todo          26/02/2025 10:54:42   never              
792   Fix bathroom toilet               in-progress   26/02/2025 11:01:46   26/02/2025 11:02:28
115   Visit mom                         todo          26/02/2025 11:01:56   never              
dotnet run list todo
ID  |           DESCRIPTION           |    STATUS   |      CREATED AT     |      UPDATED AT     
283   Fill up the car                  todo         26/02/2025 10:54:42   never                 
115   Visit mom                        todo         26/02/2025 11:01:56   never                 
``` 
- **Remove task**
```
$ dotnet run remove 283
Task 283 removed with success!
```
