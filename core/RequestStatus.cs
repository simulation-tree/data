namespace Data
{
    public enum RequestStatus : byte
    {
        Unknown = 0,
        Submitted = 1,
        Loading = 2,
        Loaded = 3,
        NotFound = 4
    }
}