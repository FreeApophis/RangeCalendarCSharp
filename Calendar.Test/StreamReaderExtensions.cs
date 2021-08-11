﻿using Funcky;
using Funcky.Monads;
using System;
using System.Collections.Generic;
using System.IO;

namespace Calendar.Test
{
    public static class StreamReaderExtensions
    {
        public static IEnumerable<string> ReadLines(this StreamReader streamReader)
            => Sequence
                .Generate(string.Empty, NextLine(streamReader));

        private static Func<string, Option<string>> NextLine(StreamReader streamReader)
            => _
                => Option.FromNullable(streamReader.ReadLine());
    }
}