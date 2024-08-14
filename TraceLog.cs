
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogvideoRecorder
{
    public class Root
    {
        [JsonProperty("ts_begin")]
        public long TsBegin { get; set; }

        [JsonProperty("ts_begin_timestring")]
        public string TsBeginTimeString { get; set; }

        [JsonProperty("ts_video_start")]
        public long TsVideoStart { get; set; }

        [JsonProperty("ts_video_start_timestring")]
        public string TsVideoStartTimeString { get; set; }

        [JsonProperty("ts_end")]
        public long TsEnd { get; set; }

        [JsonProperty("ts_end_timestring")]
        public string TsEndTimeString { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("traceLogs")]
        public List<TraceLog> TraceLogs { get; set; }

        public Root() 
        {
            TraceLogs = new List<TraceLog>(); 
        }
    }

    public class TraceLog
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("payload")]
        [JsonConverter(typeof(RawJsonConverter))]
        public string Payload { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("timestamp_timestring")]
        public string TimestampTimeString { get; set; }

        [JsonProperty("entryId")]
        public string EntryId { get; set; }
    }

    public class RawJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var json = value as string;
            if (!string.IsNullOrEmpty(json))
            {
                var token = JToken.Parse(json);
                token.WriteTo(writer);
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return JToken.ReadFrom(reader).ToString();
        }
    }
}
