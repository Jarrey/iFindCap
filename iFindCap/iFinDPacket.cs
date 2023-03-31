using Newtonsoft.Json;
using System;
using System.Text;

namespace iFindCap
{
    public class iFinDPacket
    {
        public iFinDPacket(int length, int dataLength, string value, uint seq)
        {
            Length = length;
            RemainLength = Math.Max(0, Length - dataLength);
            Data.Append(value);
            Timestamp = DateTime.Now;
            Seq = seq;
        }

        public uint Seq { get; }

        public DateTime Timestamp { get; }

        public StringBuilder Data { get; } = new StringBuilder();

        public int Length { get; }

        public int RemainLength { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool TryAppend(string value, int dataLength)
        {
            bool isCompleted = value.EndsWith(iFinDData.Suffix);
            if (isCompleted)
            {
                try
                {
                    JsonConvert.DeserializeObject<iFinDData>(Data.ToString() + value);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            Data.Append(value);
            RemainLength = Math.Max(0, RemainLength - dataLength);
            IsCompleted = isCompleted && RemainLength == 0;
            return true;
        }

        public iFinDData GetiFindDData()
        {
            try
            {
                string json = Data.ToString();
                iFinDData data = JsonConvert.DeserializeObject<iFinDData>(json);
                data.Json = json;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
