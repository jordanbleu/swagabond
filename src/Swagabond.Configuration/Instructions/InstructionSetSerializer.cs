using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Swagabond.Configuration.Instructions;

public static class InstructionSetSerializer
{
    private static IDeserializer YamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();
    
    private static ISerializer YamlSerializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();


    public static InstructionSet Deserialize(string yaml)
    {
        return YamlDeserializer.Deserialize<InstructionSet>(yaml);
    }
    
    public static string Serialize(InstructionSet instructionSet)
    {
        return YamlSerializer.Serialize(instructionSet);
    }

}