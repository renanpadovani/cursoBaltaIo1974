using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaltaStore.Domain.StoreContext.Entities;
using BaltaStore.Domain.StoreContext.ValueObjects;
using BaltaStore.Domain.StoreContext.CustomerCommands.Inputs;

namespace BaltaStore.Tests.Commands
{
    [TestClass]
    public class CreateCustomerCommandTests
    {
        [TestMethod]
        public void ShouldValidateWhenCommandIsValid()
        {
            var command = new CreateCustomerCommand();

            command.FirstName = "Renan";
            command.LastName = "Padovani";
            command.Document = "12345678901";
            command.Email = "renan@flext.com.br";
            command.Phone = "19982447253";

            Assert.AreEqual(true, command.Valid());
        }
    }
}
