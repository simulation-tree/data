using Data;
using Data.Components;
using System;
using Unmanaged;

public static class DataFunctions
{
    public static Span<byte> GetBytes<T>(this T data) where T : IData
    {
        return data.GetList<T, byte>().AsSpan();
    }

    public static FixedString GetAddress<T>(this T data) where T : IData
    {
        return data.GetComponent<T, IsData>().address;
    }

    public static void Clear<T>(this T data) where T : IData
    {
        data.GetList<T, byte>().Clear();
    }

    /// <summary>
    /// Appends the given text as UTF8 formatted bytes.
    /// </summary>
    public static void Write<T>(this T data, ReadOnlySpan<char> text) where T : IData
    {
        using BinaryWriter writer = BinaryWriter.Create();
        writer.WriteUTF8Span(text);
        data.Write(writer.AsSpan());
    }

    /// <summary>
    /// Appends the given bytes.
    /// </summary>
    public static void Write<T>(this T data, ReadOnlySpan<byte> bytes) where T : IData
    {
        data.GetList<T, byte>().AddRange(bytes);
    }
}
