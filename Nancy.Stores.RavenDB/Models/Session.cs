namespace Nancy.Stores.RavenDB.Models
{
    using System;
    using System.Collections.Generic;

    public class Session
    {
        public Guid Id { get; set; }

        public IDictionary<string, object> Items { get; set; }
    }
}
