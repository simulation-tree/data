# Data
Abstraction of data fetching and references.

### Requesting with addresses
Entities with the `IsDataRequest` component imply that the entity will have a list of bytes
fetched from whatever source is available:
```csharp
using World world = new();
DataRequest request = new(world, "*/Assets/text.txt");
while (!request.Is())
{
    world.Submit(new DataUpdate());
    world.Poll();
}

ReadOnlySpan<byte> data = request.GetBytes();
Assert.That(Encoding.UTF8.GetString(data), Is.EqualTo("Hello, World!"));
```

### Address mechanisms
* Able to use `*` as a wildcard, for skipping text until the next matching character
* Finding embedded resources with file-system-like paths

### Sources
By default, the minimum source expected to be available are entities with the `IsData` component,
that are wrapped around in the `DataSource` type.
```csharp
DataSource source = new(world, "MyProject/Assets/text.txt");
source.Write("Hello, World!");
```

Systems that implement fetching are encouraged to implement loading from other sources too,
to not limit it to just entities. The [data-systems](https://github.com/game-simulations/data-systems) implementation project is an example of this.