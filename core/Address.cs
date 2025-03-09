using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unmanaged;

namespace Data
{
    [SkipLocalsInit]
    public struct Address : IEquatable<Address>
    {
        private ASCIIText256 value;

        public readonly byte Length => value.Length;

        public Address(ASCIIText256 value)
        {
            this.value = value;
        }

        public Address(string value)
        {
            this.value = new(value);
        }

        public Address(System.Span<char> value)
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

        public readonly int ToString(Span<char> destination)
        {
            return value.CopyTo(destination);
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

        public readonly bool Equals(ReadOnlySpan<char> other)
        {
            Span<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
            for (int i = 0; i < self.Length; i++)
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
            Span<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
            for (int i = other.Length - 1; i >= 0; i--)
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

        public readonly int IndexOf(char character)
        {
            return value.IndexOf(character);
        }

        public readonly int LastIndexOf(char character)
        {
            return value.LastIndexOf(character);
        }

        public readonly bool TryIndexOf(char character, out int index)
        {
            return value.TryIndexOf(character, out index);
        }

        public readonly bool TryLastIndexOf(char character, out int index)
        {
            return value.TryLastIndexOf(character, out index);
        }

        public readonly Address Slice(int start, int length)
        {
            return new(value.Slice(start, length));
        }

        public readonly Address Slice(int start)
        {
            return new(value.Slice(start));
        }

        public readonly bool Matches(string other)
        {
            Span<char> buffer = stackalloc char[other.Length];
            for (int i = 0; i < other.Length; i++)
            {
                buffer[i] = other[i];
            }

            return Matches(buffer);
        }

        public readonly bool Matches(Address other)
        {
            Span<char> buffer = stackalloc char[other.value.Length];
            other.value.CopyTo(buffer);
            return Matches(buffer);
        }

        public readonly bool Matches(ReadOnlySpan<char> other)
        {
            Span<char> self = stackalloc char[value.Length];
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

        public static implicit operator ASCIIText256(Address address)
        {
            return address.value;
        }
    }
}