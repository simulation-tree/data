using Data.Components;
using System.Threading;
using System.Threading.Tasks;
using Unmanaged;
using Worlds;

namespace Data
{
    public static class DataExtensions
    {
        public static USpan<byte> GetBytes<T>(this T data) where T : unmanaged, IData
        {
            return data.AsEntity().GetArray<BinaryData>().As<byte>();
        }

        public static BinaryReader CreateBinaryReader<T>(this T data) where T : unmanaged, IData
        {
            return new(data.GetBytes());
        }

        public static bool IsLoaded<T>(this T data) where T : unmanaged, IDataRequest
        {
            return data.AsEntity().GetComponent<IsDataRequest>().status == RequestStatus.Loaded;
        }

        public static async Task UntilLoaded<T>(this T data, Update action, CancellationToken cancellation = default) where T : unmanaged, IDataRequest
        {
            while (!IsLoaded(data))
            {
                await action(data.GetWorld(), cancellation);
            }
        }

        public static Address GetSourceAddress<T>(this T data) where T : unmanaged, IDataSource
        {
            return data.AsEntity().GetComponent<IsDataSource>().address;
        }

        public static Address GetRequestAddress<T>(this T data) where T : unmanaged, IDataRequest
        {
            return data.AsEntity().GetComponent<IsDataRequest>().address;
        }

        public static void Clear<T>(this T data) where T : unmanaged, IData
        {
            data.AsEntity().ResizeArray<BinaryData>(0);
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public static void WriteUTF8<T>(this T data, USpan<char> text) where T : unmanaged, IData
        {
            using BinaryWriter writer = new(4);
            writer.WriteUTF8(text);
            Write(data, writer.AsSpan());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public static void WriteUTF8<T>(this T data, FixedString text) where T : unmanaged, IData
        {
            using BinaryWriter writer = new(4);
            writer.WriteUTF8(text);
            Write(data, writer.AsSpan());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public static void WriteUTF8<T>(this T data, string text) where T : unmanaged, IData
        {
            using BinaryWriter writer = new(4);
            writer.WriteUTF8(text);
            Write(data, writer.AsSpan());
        }

        /// <summary>
        /// Appends the given bytes.
        /// </summary>
        public static void Write<T>(this T data, USpan<byte> bytes) where T : unmanaged, IData
        {
            uint length = data.AsEntity().GetArrayLength<BinaryData>();
            USpan<BinaryData> array = data.AsEntity().ResizeArray<BinaryData>(bytes.Length + length);
            bytes.CopyTo(array.As<byte>());
        }
    }
}