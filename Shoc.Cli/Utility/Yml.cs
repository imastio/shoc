using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shoc.Cli.Utility
{
    /// <summary>
    /// The YAML extensions
    /// </summary>
    public class Yml
    {
        /// <summary>
        /// Serializes the input data
        /// </summary>
        /// <param name="input">The input to serialize</param>
        /// <returns></returns>
        public static string Serialize(object input)
        {
            // build a serializer
            var serializer = new SerializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // serialize the data
            return serializer.Serialize(input);
        }

        /// <summary>
        /// Deserializes the input data
        /// </summary>
        /// <param name="input">The input to serialize</param>
        /// <returns></returns>
        public static T Deserialize<T>(string input)
        {
            // build a serializer
            var serializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // serialize the data
            return serializer.Deserialize<T>(input);
        }
    }
}