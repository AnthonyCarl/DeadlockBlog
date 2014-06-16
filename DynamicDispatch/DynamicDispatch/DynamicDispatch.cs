using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ImpromptuInterface.Dynamic;

namespace DynamicDispatch
{
    internal class DynamicDispatch
    {
        private static readonly List<object> TestList = new List<object>
        {
            1,
            (byte) 0x11,
            "Text",
            new MessageObject {Message = "A Message"},
            new AnotherMessageObject {Message = "Another Message"},
            new NoMethodForThisObject {Message = "No overload for this concrete type"},
            new object(),
            new InheritMessage {Message = "This inherits"},
            new NoOverloadInheritMessage {Message = "No overload for this inherited type."},
            (int?)null, //meaningless casts
            (string)null,
            (MessageObject)null,
            null,
            (int?) 99,
            123L,
            1.23D,
            2.22F,
            3.33M,
            new[] {1} //no overload for this type
        };

        public static void Main(string[] args)
        {
            TimeIterations(LoopTypeMatching, "type matching");
            TimeIterations(LoopDynamic, "dynamic");
            TimeIterations(LoopImpromptu, "impromptu");
            TimeIterations(LoopReflection, "reflection");

            Console.WriteLine("Hit enter to quit");
            Console.ReadLine();
        }

        private static void TimeIterations(Action<ObjectProcessors> timedAction, string timedActionName)
        {
            Console.WriteLine("Output from {0}:", timedActionName);
            timedAction(new ObjectProcessors(true));
            const int iterations = 200000;
            var nonLoggingProcessors = new ObjectProcessors(false);

            Console.Write("Timing {0:N0} iterations using {1}... ", iterations, timedActionName);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                timedAction(nonLoggingProcessors);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.WriteLine();
        }

        public static void LoopReflection(ObjectProcessors processors)
        {
            var objectProcessorsType = processors.GetType();
            foreach (var obj in TestList)
            {
                if (obj == null)
                {
                    processors.ProcessUnknownType();
                }
                else
                {
                    var mi = objectProcessorsType.GetMethod("Process", new[] {obj.GetType()});
                    mi.Invoke(processors, new[] {obj});
                }
            }
        }

        public static void LoopDynamic(ObjectProcessors processors)
        {
            foreach (var obj in TestList)
            {
                if (obj == null)
                {
                    processors.ProcessUnknownType();
                }
                else
                {
                    processors.Process((dynamic)obj);
                }
            }
        }

        public static void LoopImpromptu(ObjectProcessors processors)
        {
            var ci = new CacheableInvocation(InvocationKind.InvokeMemberAction, "Process", 1);

            foreach (var obj in TestList)
            {
                if (obj == null)
                {
                    processors.ProcessUnknownType();
                }
                else
                {
                    ci.Invoke(processors, obj);
                }
            }
        }

        public static void LoopTypeMatching(ObjectProcessors processors)
        {
            foreach (var obj in TestList)
            {
                if (obj == null)
                {
                    processors.ProcessUnknownType();
                    continue;
                }

                var type = obj.GetType();

                if (type == typeof (int?))
                {
                    processors.Process((int?) obj);
                    continue;
                }
                if (type == typeof (int))
                {
                    processors.Process((int?) obj);
                    continue;
                }
                if (type == typeof (long))
                {
                    processors.Process((long) obj);
                    continue;
                }
                if (type == typeof (double))
                {
                    processors.Process((double) obj);
                    continue;
                }
                if (type == typeof (float))
                {
                    processors.Process((float) obj);
                    continue;
                }
                if (type == typeof (decimal))
                {
                    processors.Process((decimal) obj);
                    continue;
                }
                if (type == typeof (byte))
                {
                    processors.Process((byte) obj);
                    continue;
                }
                if (type == typeof (string))
                {
                    processors.Process((string) obj);
                    continue;
                }
                if (type == typeof (InheritMessage))
                {
                    processors.Process((InheritMessage) obj);
                    continue;
                }
                if (type == typeof (MessageObject))
                {
                    processors.Process((MessageObject) obj);
                    continue;
                }
                if (type == typeof (AnotherMessageObject))
                {
                    processors.Process((AnotherMessageObject) obj);
                    continue;
                }
                if (type.BaseType == typeof (MessageBase))
                {
                    processors.Process((MessageBase) obj);
                    continue;
                }
                if (type.GetInterfaces().Contains(typeof (IHaveMessage)))
                {
                    processors.Process((IHaveMessage) obj);
                    continue;
                }
                processors.Process(obj);
            }
        }
    }
}