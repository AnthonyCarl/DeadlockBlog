using System;

namespace DynamicDispatch
{
    public class ObjectProcessors
    {
        private readonly bool _printToConsole;

        public ObjectProcessors(bool printToConsole)
        {
            _printToConsole = printToConsole;
        }

        public void ProcessUnknownType()
        {
            if (_printToConsole)
            {
                Console.WriteLine("Unknown type - NULL");
            }
        }

        public void Process(object value)
        {
            LogMessage("object");
        }

        public void Process(string value)
        {
            LogMessage("string");
        }

        //This shouldn't get called
        public void Process(int? value)
        {
            LogMessage("int?");
        }

        //Also matches byte if there is no byte overload
        public void Process(int value)
        {
            LogMessage("int");
        }

        public void Process(MessageObject value)
        {
            LogMessage("MessageObject");
        }

        public void Process(AnotherMessageObject value)
        {
            LogMessage("AnotherMessageObject");
        }

        public void Process(IHaveMessage value)
        {
            LogMessage("IHaveMessage");
        }

        public void Process(InheritMessage value)
        {
            LogMessage("InheritMessage");
        }

        public void Process(MessageBase value)
        {
            LogMessage("MessageBase");
        }

        public void Process(byte value)
        {
            LogMessage("byte");
        }

        public void Process(long value)
        {
            LogMessage("long");
        }

        //Also matches float if there is no float overload
        public void Process(double value)
        {
            LogMessage("double");
        }

        public void Process(decimal value)
        {
            LogMessage("decimal");
        }

        public void Process(float value)
        {
            LogMessage("float");
        }

        private void LogMessage(string typeName)
        {
            if (_printToConsole)
            {
                Console.WriteLine("Processed a value of type {0}.", typeName);
            }
        }
    }
}