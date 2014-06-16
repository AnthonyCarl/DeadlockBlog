namespace DynamicDispatch
{
    public interface IHaveMessage
    {
        string Message { get; set; }
    }

    public class MessageObject : IHaveMessage
    {
        public string Message { get; set; }
    }

    public class AnotherMessageObject : IHaveMessage
    {
        public string Message { get; set; }
    }

    public class NoMethodForThisObject : IHaveMessage
    {
        public string Message { get; set; }
    }

    public abstract class MessageBase : IHaveMessage
    {
        public abstract string Message { get; set; }
    }

    public class InheritMessage : MessageBase
    {
        public override string Message { get; set; }
    }

    public class NoOverloadInheritMessage : MessageBase
    {
        public override string Message { get; set; }
    }
}