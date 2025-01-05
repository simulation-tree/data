using System;
using System.Collections.Generic;
using Unmanaged;

namespace Data
{
    public struct Address : IEquatable<Address>
    {
        public FixedString value;

        public Address(FixedString value)
        {
            this.value = value;
        }

        public Address(string value)
        {
            this.value = new(value);
        }

        public Address(USpan<char> value)
        {
            this.value = new(value);
        }

        public Address(IEnumerable<char> value)
        {
            this.value = new(value);
        }

        public readonly override string ToString()
        {
            return value.ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return value.CopyTo(buffer);
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Address address && Equals(address);
        }

        public readonly bool Equals(Address other)
        {
            return value == other.value;
        }

        public readonly bool Equals(string other)
        {
            return Equals(other.AsUSpan());
        }

        public readonly bool Equals(USpan<char> other)
        {
            USpan<char> self = stackalloc char[(int)FixedString.Capacity];
            uint length = value.CopyTo(self);
            for (uint i = 0; i < length; i++)
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

        public readonly bool EndsWith(USpan<char> other)
        {
            USpan<char> self = stackalloc char[(int)FixedString.Capacity];
            uint length = value.CopyTo(self);
            for (uint i = other.Length - 1; i != uint.MaxValue; i--)
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

        public readonly bool Matches(string other)
        {
            USpan<char> buffer = stackalloc char[other.Length];
            for (uint i = 0; i < other.Length; i++)
            {
                buffer[i] = other[(int)i];
            }

            return Matches(buffer);
        }

        public readonly bool Matches(Address other)
        {
            USpan<char> buffer = stackalloc char[(int)FixedString.Capacity];
            uint length = other.value.CopyTo(buffer);
            return Matches(buffer.Slice(0, length));
        }

        public readonly bool Matches(USpan<char> other)
        {
            USpan<char> self = stackalloc char[(int)FixedString.Capacity];
            uint length = value.CopyTo(self);
            if (other[0] == '*')
            {
                //todo: fault: what about * in the middle? or .. tokens?
                if (other[1] == '/' || other[0] == '\\')
                {
                    other = other.Slice(2);
                }
                else
                {
                    other = other.Slice(1);
                }

                return new Address(self.Slice(0, length)).EndsWith(other);
            }
            else
            {
                return new Address(self.Slice(0, length)).Equals(other);
            }
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

        public static implicit operator Address(string value)
        {
            return new(value);
        }

        public static Address Get<T>() where T : unmanaged, IDataReference
        {
            return default(T).Value;
        }
    }
}