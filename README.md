# Data

Abstraction of data fetching and data sources.

### Requesting data with addresses

Entities with the `IsDataRequest` component imply that the entity wants to fetch bytes
from the address requested, within the timeout specified:
```cs
using World world = new();
DataRequest request = new(world, "Assets/text.txt");

//after work is done to load it

Assert.That(request.IsLoaded, Is.True);
ReadOnlySpan<byte> utf8Bytes = request.GetBytes();
Assert.That(new ASCIIText256(utf8Bytes).ToString(), Is.EqualTo("Hello, World!"));
```

### Address mechanisms

* Can use the `*` as a wildcard, for skipping text until the next matching character
* Can refer to embedded resources with file system like paths

### Embedded resources

If a project contains embedded resources, and are mean't to be findable with an address.
They need to be registered with the `EmbeddedResourceRegistry`. This can be done
manually, or automatically with `IEmbeddedResource` types and the `EmbeddedResourceRegistryLoader`:
```cs
public readonly struct TextAsset : IEmbeddedResource
{
    readonly Address IEmbeddedResource.Address => "Assets/text.txt";
}

static void Main() 
{
    EmbeddedResourceRegistryLoader.Load();
}
```

### Sources

By default, the minimum source expected to be available are entities with the `IsDataSource` component.
These are wrapped around in the `DataSource` type.
```cs
DataSource source = new(world, "Assets/text.txt");
source.WriteUTF8("Hello, World!");
```

Systems that implement fetching are encouraged to implement loading from other sources too,
as to not limit it to just entities. The [data-systems](https://github.com/simulation-tree/data-systems) implementation project is an example of this.
