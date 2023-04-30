using System;
using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh.SourceGenerator.Aspect.Core;

namespace Scellecs.Morpeh.SourceGenerator.Aspect.Logger;

public class Logger
{
#if DEBUG
    private readonly List<string> _logs = new();
    private readonly char[] _lineSeparator = { '\n' };
#endif

    public void Flush(Action<GeneratedFile> flush)
    {
#if DEBUG
        flush.Invoke(new GeneratedFile
        {
            Content = string.Join("\n\n", _logs),
            FileName = "_logs.debug.g.cs"
        });
#endif
    }

    public void Log(string message)
    {
#if DEBUG
        var lines = message.Split(_lineSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Insert(0, "// "));

        message = string.Join("\n", lines);

        _logs.Add(message);
#endif
    }
}