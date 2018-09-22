using System;
namespace PCOR.NET
{
    public class ExecArgs<TParamaterType,TReturnType>
        where TParamaterType : class
    {
        public ExecArgs(TParamaterType paramater, TReturnType previousReturnValue)
        {
            Paramater = paramater;
            PreviousReturnValue = previousReturnValue;
        }

        public TReturnType PreviousReturnValue { get; private set; }
        public TParamaterType Paramater { get; private set; }

    }
}
