﻿namespace SqlStreamStore.Postgres.PgSqlScripts
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;

    internal class Scripts
    {
        internal readonly string Schema;

        internal readonly bool UseJsonB;

        private readonly ConcurrentDictionary<string, string> _scripts
            = new ConcurrentDictionary<string, string>();

        internal Scripts(string schema, bool useJsonB = false)
        {
            Schema = schema;
            UseJsonB = useJsonB;
        }

        internal string AppendStreamExpectedVersionAny => GetScript(nameof(AppendStreamExpectedVersionAny));

        internal string AppendStreamExpectedVersion => GetScript(nameof(AppendStreamExpectedVersion));

        internal string AppendStreamExpectedVersionNoStream => GetScript(nameof(AppendStreamExpectedVersionNoStream));

        internal string DeleteStreamAnyVersion => GetScript(nameof(DeleteStreamAnyVersion));

        internal string DeleteStreamMessage => GetScript(nameof(DeleteStreamMessage));

        internal string DeleteStreamExpectedVersion => GetScript(nameof(DeleteStreamExpectedVersion));

        internal string DropAll => GetScript(nameof(DropAll));

        internal string GetStreamMessageCount => GetScript(nameof(GetStreamMessageCount));

        internal string GetStreamMessageBeforeCreatedCount => GetScript(nameof(GetStreamMessageBeforeCreatedCount));

        internal string InitializeStore => GetScript(nameof(InitializeStore));

        internal string ReadAllForward => GetScript(nameof(ReadAllForward));

        internal string ReadHeadPosition => GetScript(nameof(ReadHeadPosition));

        internal string ReadAllBackward => GetScript(nameof(ReadAllBackward));

        internal string ReadStreamForward => GetScript(nameof(ReadStreamForward));

        internal string ReadStreamBackward => GetScript(nameof(ReadStreamBackward));

        internal string SetStreamMetadata => GetScript(nameof(SetStreamMetadata));

        private string GetScript(string name)
        {
            return _scripts.GetOrAdd(name,
                key =>
                {
                    using (Stream stream = typeof(Scripts)
                        .Assembly
                        .GetManifestResourceStream("SqlStreamStore.PgSqlScripts." + key + ".pgsql"))
                    {
                        if (stream == null)
                        {
                            throw new Exception($"Embedded resource, {name}, not found. BUG!");
                        }
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader
                                .ReadToEnd()
                                .Replace("$schema$", Schema)
                                .Replace("$jsonType$", UseJsonB ? "jsonb" : "json");
                        }
                    }
                });
        }
    }
}