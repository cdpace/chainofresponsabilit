using System;
using System.Threading.Tasks;
using Xunit;


namespace PCOR.NET.Tests
{
    public class BaseChainTests
    {
        #region Common Items
        class TestParameterType{
            public string Value { get; set; }
        }


        class TestChainItem : IChainItem<TestParameterType, string>
        {
            public override Task<BreakableChainReturnValue<string>> ExecuteDown(ExecArgs<TestParameterType, string> execArgs)
            {
                return Task.FromResult(new BreakableChainReturnValue<string>()
                {
                    Value = execArgs.Paramater.Value + " Test Chain Item reached"
                });
            }
        }

        class TestChain : BaseChain<TestParameterType, string>{
            public TestChain()
            {
                //Define Chain Items
                AddChainItem(new TestChainItem());
            }
        }


        #endregion


        [Fact(DisplayName = nameof(AddItemToChain_Valid))]
        public Task AddItemToChain_Valid(){
            //Arrange / Act
            var chain = new TestChain();

            //Assert
            Assert.Equal<int>(1, chain.ChainItemsCount);

            return Task.CompletedTask;
        }

        [Fact(DisplayName = nameof(ReturnValidResult_Valid))]
        public async Task ReturnValidResult_Valid(){
            //Arrange
            var chain = new TestChain();
            var param = "Hello World!";

            //Act

            var result = await chain.ExecuteAsync(new TestParameterType()
            {
                Value = param
            }, Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal($"{param} Test Chain Item reached", result);
        }
    }
}
