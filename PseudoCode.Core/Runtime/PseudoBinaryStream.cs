using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;

namespace PseudoCode.Core.Runtime;

public class PseudoBinaryStream
{
    public readonly Dictionary<int, Instance> Memory = new();
    [JsonIgnore] public int CurrentAddress;
    public static JsonSerializer Serializer { get; } = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
    };

    public void Put(Instance i)
    {
        if (!i.Type.Serializable)
            throw new InvalidTypeError($"{i.Type} is not serializable", null);
        Memory[CurrentAddress++] = i.RealInstance;
    }

    public Instance Get() => Memory[CurrentAddress ++];
    public void Seek(int address) => CurrentAddress = address;
    

    public void Write(MemoryStream memoryStream)
    {
        var writer = new BsonDataWriter(memoryStream);
        Serializer.Serialize(writer, this);
    }

    public static PseudoBinaryStream From(MemoryStream memoryStream)
    {
        using var reader = new BsonDataReader(memoryStream);
        return Serializer.Deserialize<PseudoBinaryStream>(reader);
    }
}