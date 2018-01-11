using Bot.Contracts;
using Bot.Universal;
using Xunit;

namespace BotFixture
{
    public class ReflectiveEnumeratorFixture
    {
        [Fact]
        public void GetImplementationOrAbstractionFixture()
        {
            var list = ReflectiveEnumerator.GetInheritedFromAbstractClass<Functionality>();
            FileOperations.AppendLine("testing.log", JsonToFrom.ToJson(list));
            Assert.True(list.Count > 0);
        }

        [Fact]
        public void GetImplementationsOfInterfaces()
        {
            var implementations = ReflectiveEnumerator.GetInheritedFromInterface<ICommand>();
            Assert.True(implementations.Count > 0);
        }
    }
}
