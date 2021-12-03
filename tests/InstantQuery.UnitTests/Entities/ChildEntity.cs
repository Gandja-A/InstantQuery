using System;

namespace InstantQuery.UnitTests.Entities
{
    public class ChildEntity
    {
        public long Id { get; set; }

        public DateTime? Date { get; set; }

        public Entity Entity { get; set; }

        public long EntityId { get; set; }
    }
}
