namespace remikub.Domain
{
    using System;

    public class RemikubException : Exception
    {
        public RemikubException(RemikubExceptionCode code, params string[] details) : base(code.ToString())
        {
            Details = details;
        }

        public string[] Details { get; }
    }

    public enum RemikubExceptionCode
    {
        PlayOrDraw,
        InvalidCardAddedOrRemoved,
        InvalidCombination,
    }
}
