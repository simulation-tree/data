# Data
Abstraction of data fetching and references.

### Requesting with addresses
Entities with the `IsDataRequest` component imply that the entity will also have a list of bytes,
fetched from implemented sources:
```csharp
using World world = new();
DataRequest request = new(world, "*/Assets/text.txt");
ReadOnlySpan<byte> data = request.GetBytes();
Assert.That(Encoding.UTF8.GetString(data), Is.EqualTo("Hello, World!"));
```

### Address mechanisms
* Able to use `*` as a wildcard, for skipping text until the next matching character
* Able to match against a C# embedded resource path

### Sources
By default, the only source available are entities with the `IsData` component:
```csharp
DataSource source = new(world, "MyProject/Assets/text.txt");
source.Write("Hello, World!");
```

Systems that implement fetching are encouraged to implement loading from other sources as well,
as to not limit it to just entities alone. The [data-systems](https://github.com/game-simulations/data-systems) implementation project is an example of this.