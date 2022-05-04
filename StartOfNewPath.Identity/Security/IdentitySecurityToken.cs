﻿using Microsoft.IdentityModel.Tokens;
using System;

namespace StartOfNewPath.Identity.Security
{
    internal class IdentitySecurityToken : SecurityToken
    {
        private readonly SecurityToken _parent;

        public IdentitySecurityToken(SecurityToken parent)
        {
            _parent = parent;
        }

        public override string Id => Guid.NewGuid().ToString();

        public override string Issuer => _parent.Issuer;

        public override SecurityKey SecurityKey => _parent.SecurityKey;

        public override SecurityKey SigningKey { get => _parent.SigningKey; set => _parent.SigningKey = value; }

        public override DateTime ValidFrom => _parent.ValidFrom;

        public override DateTime ValidTo => _parent.ValidTo;
    }
}
