using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FlashThunder.Utilities
{
    /// <summary>
    /// A utility class to simplify the serialization and de-serialization of objects,
    /// as well as basic loading/saving. (TBA)
    /// </summary>
    public static class DataLoader
    {
        public static readonly string Prefix = "../../../Data/";

        //ensure proper naming conventions and keep the json files like readable brah
        public static readonly JsonSerializerOptions Options  = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        /// <summary>
        /// Loads object of type T from the specified file.
        /// </summary>
        /// <remarks>
        /// if the object is too complex to directly de-serialize (nested objs, dependency, blah blah),
        /// consider using a data transfer object as a medium between them for the ser/deser process
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file name (with full extension)</param>
        /// <returns>The instantiated object</returns>
        public static T LoadObject<T>(string filePath)
        {
            try
            {
                using StreamReader reader = new(Prefix + filePath);
                string jsonFile = reader.ReadToEnd();
                return JsonSerializer.Deserialize<T>(jsonFile, Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"something went wrong while loading definitions!! {ex.Message}");
                return default;
            }
        }

        public static string SerObject<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, Options);
        }

    }
}
