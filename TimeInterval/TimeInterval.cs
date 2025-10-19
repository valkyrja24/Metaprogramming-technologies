using System;
using System.Globalization;

namespace TimeIntervalLib
{
    public sealed class TimeInterval : IEquatable<TimeInterval>
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public TimeInterval(DateTime start, DateTime end)
        {
            if (start > end) throw new ArgumentException("Start must be <= End.");
            Start = start;
            End = end;
        }

        public TimeInterval(string s)
        {
            var t = FromStringInternal(s);
            Start = t.Start;
            End = t.End;
        }

        public static TimeInterval FromString(string s) => FromStringInternal(s);

        private static TimeInterval FromStringInternal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) throw new FormatException("Input is empty.");
            s = s.Trim();
            char[] separators = new[] { '-', '–', '—' };
            int sepIndex = s.IndexOfAny(separators);
            if (sepIndex < 0) throw new FormatException("Range separator '-' not found.");
            var left = s.Substring(0, sepIndex).Trim();
            var right = s.Substring(sepIndex + 1).Trim();

            if (!TimeSpan.TryParseExact(left, "hh\\:mm", CultureInfo.InvariantCulture, out var tsLeft) &&
                !TimeSpan.TryParse(left, CultureInfo.InvariantCulture, out tsLeft))
                throw new FormatException($"Cannot parse left time '{left}'.");

            if (!TimeSpan.TryParseExact(right, "hh\\:mm", CultureInfo.InvariantCulture, out var tsRight) &&
                !TimeSpan.TryParse(right, CultureInfo.InvariantCulture, out tsRight))
                throw new FormatException($"Cannot parse right time '{right}'.");

            var baseDate = DateTime.MinValue.Date;
            var start = baseDate + tsLeft;
            var end = baseDate + tsRight;
            if (start > end) throw new FormatException("Start time must be <= End time.");

            return new TimeInterval(start, end);
        }

        public override string ToString()
        {
            return $"[{Start:HH\\:mm}–{End:HH\\:mm}]".Replace("\\:", ":");
        }

        public override bool Equals(object obj) => Equals(obj as TimeInterval);

        public bool Equals(TimeInterval other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return Start == other.Start && End == other.End;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Start.GetHashCode();
                hash = hash * 31 + End.GetHashCode();
                return hash;
            }
        }

        public int Length()
        {
            return (int)(End - Start).TotalMinutes;
        }

        public bool Overlaps(TimeInterval other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return Max(Start, other.Start) <= Min(End, other.End);
        }

        public bool Overlaps(DateTime moment)
        {
            return Start <= moment && moment <= End;
        }

        public bool Overlaps(int minuteFromMidnight)
        {
            if (minuteFromMidnight < 0 || minuteFromMidnight >= 24 * 60)
                throw new ArgumentOutOfRangeException(nameof(minuteFromMidnight));
            var baseDate = DateTime.MinValue.Date;
            var moment = baseDate + TimeSpan.FromMinutes(minuteFromMidnight);
            return Overlaps(moment);
        }

        public DateTime this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return Start;
                    case 1: return End;
                    default: throw new IndexOutOfRangeException("Indexer accepts only 0 (start) or 1 (end).");
                }
            }
        }

        public DateTime this[string name]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is null or empty.");
                var key = name.Trim().ToLowerInvariant();
                switch (key)
                {
                    case "start": return Start;
                    case "end": return End;
                    default: throw new ArgumentException("Indexer accepts only 'start' or 'end' (case-insensitive).");
                }
            }
        }

        public static bool operator ==(TimeInterval a, TimeInterval b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(TimeInterval a, TimeInterval b) => !(a == b);

        public static TimeInterval operator +(TimeInterval a, TimeInterval b)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            if (b is null) throw new ArgumentNullException(nameof(b));
            var start = Min(a.Start, b.Start);
            var end = Max(a.End, b.End);
            return new TimeInterval(start, end);
        }

        public static TimeInterval operator *(TimeInterval a, TimeInterval b)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            if (b is null) throw new ArgumentNullException(nameof(b));
            var start = Max(a.Start, b.Start);
            var end = Min(a.End, b.End);
            if (start <= end) return new TimeInterval(start, end);
            return null;
        }

        public static explicit operator int(TimeInterval t) => t?.Length() ?? throw new ArgumentNullException(nameof(t));

        public static bool operator true(TimeInterval t)
        {
            if (t is null) return false;
            return t.Length() > 0;
        }

        public static bool operator false(TimeInterval t)
        {
            if (t is null) return true;
            return t.Length() <= 0;
        }

        public static TimeInterval operator &(TimeInterval a, TimeInterval b) => a * b;

        public static TimeInterval operator |(TimeInterval a, TimeInterval b) => a + b;

        private static DateTime Max(DateTime a, DateTime b) => a >= b ? a : b;
        private static DateTime Min(DateTime a, DateTime b) => a <= b ? a : b;
    }
}
