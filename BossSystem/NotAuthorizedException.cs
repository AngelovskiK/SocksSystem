using System;

namespace BossSystem
{
    public class NotAuthorizedException: Exception
    {
        public NotAuthorizedException() : base("Not authorized") { }

        public NotAuthorizedException(string message) : base(message) { }

        public NotAuthorizedException(string message, Exception ex) : base(message, ex) { }
    }
}