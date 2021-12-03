using System;
using System.Collections.Generic;

namespace InstantQuery.UnitTests.Entities
{
    public class Entity
    {
        public long Id { get; set; }

        public long LongValue { get; set; }

        public long? LongNullableValue { get; set; }

        public int IntValue { get; set; }

        public int? IntNullableValue { get; set; }

        public string StringValue { get; set; }

        public Guid GuidValue { get; set; }

        public Guid? GuidNullableValue { get; set; }

        public DateTime DateTimeValue { get; set; }

        public DateTime? DateTimeNullable { get; set; }

        public DateTimeOffset DateTimeOffsetValue { get; set; }
        public DateTimeOffset? DateTimeOffsetNullableValue { get; set; }

        public List<ChildEntity> ChildEntities { get; set; }
    }
}
