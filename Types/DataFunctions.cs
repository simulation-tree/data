using Data;
using Data.Components;
using System;
using Unmanaged;

public static class DataFunctions
{
    public static Span<byte> GetBytes<T>(this T data) where T : unmanaged, IData
    {
        return data.GetList<T, byte>().AsSpan();
    }

    public static FixedString GetAddress<T>(this T data) where T : unmanaged, IData
    {
        return data.GetComponent<T, IsData>().address;
    }

    public static void Clear<T>(this T data) where T : unmanaged, IData
    {
        data.GetList<T, byte>().Clear();
    }

    public static void Write<T>(this T data, ReadOnlySpan<char> text) where T : unmanaged, IData
    {
        using BinaryWriter writer = BinaryWriter.Create();
        writer.WriteUTF8Span(text);
        Span<byte> bytes = writer.AsSpan();
        data.GetList<T, byte>().AddRange(bytes);
    }

    public static void Write<T>(this T data, ReadOnlySpan<byte> bytes) where T : unmanaged, IData
    {
        data.GetList<T, byte>().AddRange(bytes);
    }
}
