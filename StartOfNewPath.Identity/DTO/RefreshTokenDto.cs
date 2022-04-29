using System;

namespace StartOfNewPath.Identity.DTO
{
    public class RefreshTokenDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}
