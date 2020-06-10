using System;

namespace BossSystem
{
    public class BadRequestException: Exception
    {
        public BadRequestException() : base("Bad Request") { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, Exception ex) : base(message, ex) { }
    }
}