using System;

namespace KTI.Moo.Extensions.Core.Model
{
    public class ClientTokensBase
    {
        public virtual string AccessToken { get; set; }
        public virtual string RefreshToken { get; set; }

        public virtual DateTime AccessExpiration { get; set; }
        public virtual DateTime? RefreshExpiration { get; set; }
        public virtual string Id { get; set; }
    }
}