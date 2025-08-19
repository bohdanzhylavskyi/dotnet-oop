using DocumentsSearch.Documents;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentsSearch
{
    public class JsonDateOnlyConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString()!, Format);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

    public class DocumentDeserializer
    {
        private DocumentTypesRegistry docTypesRegistry;
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Converters = { new JsonDateOnlyConverter(), new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };

        public DocumentDeserializer(DocumentTypesRegistry docTypesRegistry)
        {
            this.docTypesRegistry = docTypesRegistry;
        }

        public Document DeserializeFromJson(DocumentType type, string json)
        {
            return (Document)JsonSerializer.Deserialize(
                json,
                this.docTypesRegistry.ResolveImplementationType(type),
                this.serializerOptions
            );
        }
    }
}
