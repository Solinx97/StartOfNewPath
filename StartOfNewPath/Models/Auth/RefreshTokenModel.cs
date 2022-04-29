﻿using System;

namespace StartOfNewPath.Models.Auth
{
    public class RefreshTokenModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}