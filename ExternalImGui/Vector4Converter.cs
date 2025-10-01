using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExternalImGui
{
    public class Vector4Converter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float[] values = JsonSerializer.Deserialize<float[]>(ref reader, options);
            if (values == null || values.Length != 4)
                return Vector4.Zero;

            return new Vector4(values[0], values[1], values[2], values[3]);
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, new float[] { value.X, value.Y, value.Z, value.W }, options);
        }
    }
}
