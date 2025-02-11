using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unmanaged;

namespace Data
{
    [SkipLocalsInit]
    public struct Address : IEquatable<Address>
    {
        private FixedString value;

        public readonly byte Length => value.Length;

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
            return Equals(other.AsSpan());
        }

        public readonly bool Equals(USpan<char> other)
        {
            USpan<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
            for (uint i = 0; i < self.Length; i++)
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
            USpan<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
            for (uint i = other.Length - 1; i != uint.MaxValue; i--)
            {
                char s = self[self.Length - 1];
                char o = other[i];
                self = self.Slice(0, self.Length - 1);

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

        public readonly uint IndexOf(char character)
        {
            return value.IndexOf(character);
        }

        public readonly uint LastIndexOf(char character)
        {
            return value.LastIndexOf(character);
        }

        public readonly bool TryIndexOf(char character, out uint index)
        {
            return value.TryIndexOf(character, out index);
        }

        public readonly bool TryLastIndexOf(char character, out uint index)
        {
            return value.TryLastIndexOf(character, out index);
        }

        public readonly Address Slice(uint start, uint length)
        {
            return new(value.Slice(start, length));
        }

        public readonly Address Slice(uint start)
        {
            return new(value.Slice(start));
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
            USpan<char> buffer = stackalloc char[other.value.Length];
            other.value.CopyTo(buffer);
            return Matches(buffer);
        }

        public readonly bool Matches(USpan<char> other)
        {
            USpan<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
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

                return new Address(self).EndsWith(other);
            }
            else
            {
                return new Address(self).Equals(other);
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

        public static implicit operator FixedString(Address address)
        {
            return address.value;
        }
    }
}