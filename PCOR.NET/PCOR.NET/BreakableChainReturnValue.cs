using System;
namespace PCOR.NET
{
    public class BreakableChainReturnValue<TReturnType> : ChainReturnValue<TReturnType>
    {
        public bool StopChain { get; set; } = false;
    }
}
