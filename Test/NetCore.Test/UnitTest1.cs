using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.Library.Identity.Commands;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCore.Test
{
    [TestClass]
    public class UnitTest1 : Testbase
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            int result = await Mediator.Send(new CreateUserCommand.Request { FullName = "Andres Mappe", UserName = "afmappe" });
            //IUserRepository repository = Provider.GetService<IUserRepository>();

            //var user = await repository.Find(1);

            Trace.WriteLine("Hello world");
        }
    }
}