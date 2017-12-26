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
            var list = ReflectiveEnumerator.GetInheritedClasses<Functionality>();
            FileOperations.AppendLine("testing.log", JsonToFrom.ToJson(list));
            Assert.True(list.Count > 0);
        }
    }
}
