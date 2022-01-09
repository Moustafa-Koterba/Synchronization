# Synchronization

Synchronization is a Unity3D package which allow to execute synchronized tasks.

## Features

- Execute synchronized tasks
- Execute asynchronized tasks
- Define ordered tasks queue with synchronization


## Usage

Define your task by extending `Executable` class:
```csharp
public class MyTask : Executable {
    
}
```
### Asynchronized Tasks
For asynchronized task, implements `Execute` parameters less method :
```csharp
public override void Execute() {
    // Do async task
}
```
Next task in the queue will be executed right after this method execution.

### Synchronized Tasks
For synchronized task, implements `Execute` with parameters method :
```csharp
public override void Execute(TaskCompletionSource<bool> completion)
{
    // Do sync task
}
```
When your synchronized task is completed, set completion result to true. It will trigger the next task.
```csharp
completion.SetResult(true);
```

### Execute tasks

Add [SynchronizedTaskExecutor](https://github.com/Moustafa-Koterba/Synchronization/blob/master/Assets/Runtime/SynchronizedTaskExecutor.cs) class to a gameobject, and define your queue by adding task to `ExecutableParameters` variable.
You can choose execution mode for each task:
</br>
![image info](https://github.com/Moustafa-Koterba/Synchronization/blob/master/synchro.PNG?raw=true)
</br>

Then run the `Execute` method of [SynchronizedTaskExecutor](https://github.com/Moustafa-Koterba/Synchronization/blob/master/Assets/Runtime/SynchronizedTaskExecutor.cs) from a script or from Unity Inspector.</br>
For example with a Button:</br>
![image info](https://github.com/Moustafa-Koterba/Synchronization/blob/master/button.PNG?raw=true)

## Releases

Clone or download this repository.

## License

MIT