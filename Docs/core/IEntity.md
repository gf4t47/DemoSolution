
```C#
public interface IEntity  
{  
    int Id { get; }  
}
```

Ideally `Id` should be a unique `Guid`. But for easy to demonstrate and unit test purpose, we will use unique `int` in this project.