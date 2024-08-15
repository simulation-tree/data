using Data;
using Data.Components;
using System;
using Unmanaged;

public static class DataRequestFunctions
{
    public static FixedString GetAddress<T>(this T request) where T : IDataRequest
    {
        IsDataRequest component = request.GetComponent<T, IsDataRequest>();
        return component.address;
    }

    public static bool IsLoaded<T>(this T request) where T : IDataRequest
    {
        return request.ContainsList<T, byte>();
    }

    public static ReadOnlySpan<byte> GetBytes<T>(this T request) where T : IDataRequest
    {
        return request.GetList<T, byte>().AsSpan();
    }
}