using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using GvasFormat;
using GvasFormat.Serialization;
using GvasFormat.Serialization.UETypes;

namespace OctopathTraveler.GvasFormat
{
    class GvasConverter
    {
        public static (bool, Exception) Convert2JsonFile(string outputPath, Stream gvasStream)
        {
            (Gvas save, Exception e) = UESerializer.Read(gvasStream);
            if (save == null) return (false, e);

            var jsonNode = JsonSerializer.SerializeToNode<Gvas>(save, new JsonSerializerOptions
            {
                IncludeFields = true,
                MaxDepth = 64,
                Converters = { new UEPropJsonConvert() }
            });
            File.WriteAllText(outputPath, jsonNode.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            }));
            return (true, e);
        }
    }

    /// <summary>
    /// Optimize json output
    /// </summary>
    public class UEPropJsonConvert : JsonConverter<UEProperty>
    {
        public override UEProperty? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, UEProperty value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value.ToObject(), options));
        }
    }
}
