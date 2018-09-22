using System;
using System.Threading.Tasks;

namespace PCOR.NET
{
    public abstract class IChainItem<TParameterType,TReturnType> 
        where TParameterType: class 
        where TReturnType : class
    {

        public abstract Task<BreakableChainReturnValue<TReturnType>> ExecuteDown(ExecArgs<TParameterType, TReturnType> execArgs);

        public virtual Task<ChainReturnValue<TReturnType>> ExecuteUp(ExecArgs<TParameterType, TReturnType> execArgs){
            return Task.FromResult(new ChainReturnValue<TReturnType>()
            {
                Value = execArgs.PreviousReturnValue
            });
        }
    }
}
