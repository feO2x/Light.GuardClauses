using System;

namespace Light.GuardClauses.Performance
{
    public sealed class SampleEntity
    {
        public readonly Guid Id;

        public SampleEntity(Guid id) => Id = id;
    }
}