using System;

namespace StartOfNewPath.DataAccessLayer.Entities
{
    public class RefreshToken
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}
