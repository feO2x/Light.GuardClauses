using System;

namespace Light.GuardClauses.Performance
{
    public class SampleEntity
    {
        public readonly Guid Id;

        public SampleEntity(Guid id)
        {
            Id = id.MustNotBeEmpty(nameof(id));
        }
    }
}