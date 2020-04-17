namespace remikub.Domain
{
    using System;

    public class RemikubException : Exception
    {
        public RemikubException(RemikubExceptionCode code, params string[] details) : base(code.ToString())
        {
            Code = code;
            Details = details;
        }
        public RemikubExceptionCode Code { get; }
        public string[] Details { get; }
    }

    public enum RemikubExceptionCode
    {
        PlayOrDraw,
        InvalidCardAddedOrRemoved,
        InvalidCombination,
        HandsAreDifferent,
    }
}
