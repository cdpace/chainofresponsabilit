using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCOR.NET
{
    public abstract class BaseChain<TParameterType, TReturnType> 
        where TParameterType: class
        where TReturnType : class
    {
        readonly List<IChainItem<TParameterType, TReturnType>> items = new List<IChainItem<TParameterType, TReturnType>>();

        public int ChainItemsCount => items.Count;

        //EventHandlers
        public Action<Exception, Guid> OnHandleException;

        protected BaseChain()
        {
            
        }

        protected void AddChainItem(IChainItem<TParameterType, TReturnType> item){
            items.Add(item);
        }

        public async Task<TReturnType> ExecuteAsync(TParameterType parameter, Guid correlationId){

            var lastReturnValue = default(TReturnType);
            var lastExecutedItem = 0;

            //Run down chain
            for (int i = 0; i < ChainItemsCount; i++)
            {
                try
                {
                    //Execute Chain Item
                    var returnValue = await items[i].ExecuteDown(new ExecArgs<TParameterType, TReturnType>(parameter, lastReturnValue));

                    lastExecutedItem = i;

                    if (returnValue != null)
                    {
                        lastReturnValue = returnValue.Value;

                        if (returnValue.StopChain)
                            break;
                    }
                    else
                        throw new InvalidOperationException("ChainItem return value can't be null.");
                    
                }
                catch (Exception ex)
                {
                    if (OnHandleException != null)
                    {
                        OnHandleException(ex, correlationId);
                    }
                    else
                        throw;
                }
            }

            //Run up the chain
            for (int i = lastExecutedItem; i >= 0; i--)
            {
                try
                {
                    //Execute Chain Item
                    var returnValue = await items[i].ExecuteUp(new ExecArgs<TParameterType, TReturnType>(parameter, lastReturnValue));

                    if (returnValue != null)
                    {
                        lastReturnValue = returnValue.Value;
                    }
                }catch(Exception ex){

                    if (OnHandleException != null)
                        OnHandleException(ex, correlationId);
                    else
                        throw;
                }
            }

            return lastReturnValue;

        }
    }
}
