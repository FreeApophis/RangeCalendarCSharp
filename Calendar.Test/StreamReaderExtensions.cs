using Funcky;
using Funcky.Monads;

namespace Calendar.Test;

public static class StreamReaderExtensions
{
    extension(StreamReader streamReader)
    {
        public IEnumerable<string> ReadLines()
            => Sequence
                .Successors(ReadLineOrNone(streamReader), NextLine(streamReader));
    }

    private static Func<string, Option<string>> NextLine(StreamReader streamReader)
        => _
            => ReadLineOrNone(streamReader);

    private static Option<string> ReadLineOrNone(StreamReader streamReader)
        => Option.FromNullable(streamReader.ReadLine());
}
