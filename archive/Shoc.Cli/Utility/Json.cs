using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Shoc.Cli.Utility
{
    /// <summary>
    /// The JSON utility
    /// </summary>
    public class Json
    {
        /// <summary>
        /// Serializes the input data
        /// </summary>
        /// <param name="input">The input to serialize</param>
        /// <returns></returns>
        public static string Serialize(object input)
        {
            // serialize the data
            return input == null ? null : JsonConvert.SerializeObject(input);
        }

        /// <summary>
        /// Deserializes the input data
        /// </summary>
        /// <param name="input">The input to serialize</param>
        /// <returns></returns>
        public static T Deserialize<T>(string input)
        {
            // deserialize the data
            return string.IsNullOrWhiteSpace(input) ? default : JsonConvert.DeserializeObject<T>(input);
        }
    }
}