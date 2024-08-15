using System;
using Unmanaged;

namespace Data
{
    public struct Address : IEquatable<Address>
    {
        private FixedString value;

        public Address(FixedString value)
        {
            this.value = value;
        }

        public Address(ReadOnlySpan<char> value)
        {
            this.value = new(value);
        }

        public readonly override string ToString()
        {
            return value.ToString();
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Address address && Equals(address);
        }

        public readonly bool Equals(Address other)
        {
            return value == other.value;
        }

        public readonly bool Equals(FixedString other)
        {
            Span<char> buffer = stackalloc char[FixedString.MaxLength];
            int length = other.CopyTo(buffer);
            return Equals(buffer[..length]);
        }

        public readonly bool Equals(ReadOnlySpan<char> other)
        {
            Span<char> self = stackalloc char[FixedString.MaxLength];
            int length = value.CopyTo(self);
            for (int i = 0; i < length; i++)
            {
                char s = self[i];
                char o = other[i];
                if (s != o)
                {
                    if (s == '/' && (o == '\\' || o == '.'))
                    {
                        continue;
                    }
                    else if (s == '\\' && (o == '/' || o == '.'))
                    {
                        continue;
                    }
                    else if (s == '.' && (o == '/' || o == '\\' || o == ' '))
                    {
                        continue;
                    }
                    else if (s == '_' && o == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public readonly bool EndsWith(ReadOnlySpan<char> other)
        {
            Span<char> self = stackalloc char[FixedString.MaxLength];
            int length = value.CopyTo(self);
            for (int i = other.Length - 1; i >= 0; i--)
            {
                char s = self[length - 1];
                char o = other[i];
                length--;
                if (s != o)
                {
                    if (s == '/' && (o == '\\' || o == '.'))
                    {
                        continue;
                    }
                    else if (s == '\\' && (o == '/' || o == '.'))
                    {
                        continue;
                    }
                    else if (s == '.' && (o == '/' || o == '\\' || o == ' '))
                    {
                        continue;
                    }
                    else if (s == '_' && o == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public readonly bool Matches(ReadOnlySpan<char> other)
        {
            Span<char> self = stackalloc char[FixedString.MaxLength];
            int length = value.CopyTo(self);
            if (other[0] == '*')
            {
                if (other[1] == '/' || other[0] == '\\')
                {
                    other = other[2..];
                }
                else
                {
                    other = other[1..];
                }

                return new Address(self[..length]).EndsWith(other);
            }
            else
            {
                return new Address(self[..length]).Equals(other);
            }
        }

        public readonly bool Matches(FixedString other)
        {
            Span<char> buffer = stackalloc char[FixedString.MaxLength];
            int length = other.CopyTo(buffer);
            return Matches(buffer[..length]);
        }

        public readonly override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Address left, Address right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !(left == right);
        }

        public static FixedString Get<T>() where T : unmanaged, IDataReference
        {
            return default(T).Value;
        }
    }
}
