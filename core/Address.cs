using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unmanaged;

namespace Data
{
    /// <summary>
    /// Data address value.
    /// </summary>
    [SkipLocalsInit]
    public struct Address : IEquatable<Address>
    {
        private ASCIIText256 value;

        /// <summary>
        /// Length of the address.
        /// </summary>
        public readonly int Length => value.Length;

        /// <summary>
        /// Creates an address from the given <paramref name="value"/>.
        /// </summary>
        public Address(ASCIIText256 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates an address from the given <paramref name="value"/>.
        /// </summary>
        public Address(string value)
        {
            this.value = new(value);
        }

        /// <summary>
        /// Creates an address from the given <paramref name="value"/>.
        /// </summary>
        public Address(ReadOnlySpan<char> value)
        {
            this.value = new(value);
        }

        /// <summary>
        /// Creates an address from the given <paramref name="value"/>.
        /// </summary>
        public Address(IEnumerable<char> value)
        {
            this.value = new(value);
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// Copies the contents of the address to the <paramref name="destination"/>.
        /// </summary>
        public readonly int CopyTo(Span<char> destination)
        {
            return value.CopyTo(destination);
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            return obj is Address address && Equals(address);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Address otherText)
        {
            return value == otherText.value;
        }

        /// <summary>
        /// Checks if this address forgivingly equals the <paramref name="otherText"/>.
        /// </summary>
        public readonly bool Equals(string otherText)
        {
            return Equals(otherText.AsSpan());
        }

        /// <summary>
        /// Checks if this address forgivingly equals the <paramref name="otherText"/>.
        /// </summary>
        public readonly bool Equals(ReadOnlySpan<char> otherText)
        {
            Span<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
            for (int i = 0; i < self.Length; i++)
            {
                char s = self[i];
                char o = otherText[i];
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

        /// <summary>
        /// Checks if this address forgivingly ends with the given <paramref name="suffix"/>.
        /// </summary>
        public readonly bool EndsWith(ReadOnlySpan<char> suffix)
        {
            Span<char> self = stackalloc char[value.Length];
            value.CopyTo(self);
            for (int i = suffix.Length - 1; i >= 0; i--)
            {
                char s = self[self.Length - 1];
                char o = suffix[i];
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

        /// <summary>
        /// Retrieves the index of the first <paramref name="character"/> in this address content.
        /// </summary>
        public readonly int IndexOf(char character)
        {
            return value.IndexOf(character);
        }

        /// <summary>
        /// Retrieves the index of the last <paramref name="character"/> in this address content.
        /// </summary>
        public readonly int LastIndexOf(char character)
        {
            return value.LastIndexOf(character);
        }

        /// <summary>
        /// Tries to retrieve the first index of the <paramref name="character"/> in this address content.
        /// </summary>
        public readonly bool TryIndexOf(char character, out int index)
        {
            return value.TryIndexOf(character, out index);
        }

        /// <summary>
        /// Tries to retrieve the last index of the <paramref name="character"/> in this address content.
        /// </summary>
        public readonly bool TryLastIndexOf(char character, out int index)
        {
            return value.TryLastIndexOf(character, out index);
        }

        /// <summary>
        /// Slices the contents of this address into a new address.
        /// </summary>
        public readonly Address Slice(int start, int length)
        {
            return new(value.Slice(start, length));
        }

        /// <summary>
        /// Slices the contents of this address into a new address from the <paramref name="start"/>.
        /// </summary>
        public readonly Address Slice(int start)
        {
            return new(value.Slice(start));
        }

        /// <summary>
        /// Checks if this address forgivingly matches the <paramref name="other"/> address.
        /// </summary>
        public readonly bool Matches(string other)
        {
            Span<char> buffer = stackalloc char[other.Length];
            for (int i = 0; i < other.Length; i++)
            {
                buffer[i] = other[i];
            }

            return Matches(buffer);
        }

        /// <summary>
        /// Checks if this address forgivingly matches the <paramref name="other"/> address.
        /// </summary>
        public readonly bool Matches(Address other)
        {
            Span<char> buffer = stackalloc char[other.value.Length];
            other.value.CopyTo(buffer);
            return Matches(buffer);
        }

        /// <summary>
        /// Checks if this address forgivingly matches the <paramref name="other"/> address.
        /// </summary>
        public readonly bool Matches(ReadOnlySpan<char> other)
        {
            if (other[0] == '*')
            {
                Span<char> self = stackalloc char[value.Length];
                value.CopyTo(self);

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
                return Equals(other);
            }
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(Address left, Address right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(Address left, Address right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public static implicit operator Address(string value)
        {
            return new(value);
        }

        /// <inheritdoc/>
        public static implicit operator ASCIIText256(Address address)
        {
            return address.value;
        }
    }
}