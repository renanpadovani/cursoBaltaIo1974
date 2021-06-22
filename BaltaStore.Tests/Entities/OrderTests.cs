using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaltaStore.Domain.StoreContext.Entities;
using BaltaStore.Domain.StoreContext.ValueObjects;
using BaltaStore.Domain.StoreContext.Enums;

namespace BaltaStore.Tests
{
    [TestClass]
    public class OrderTests
    {
        private Product _mouse;
        private Product _keyboard;
        private Product _chair;
        private Product _monitor;
        private Customer _customer;
        private Order _order;
        
        public OrderTests()
        {
            var name = new Name("renan","padovani");
            var document = new Document("12345678901");
            var email = new Email("renan@flext.com.br");
            _customer = new Customer(name, document, email, "19982447253");
            _order = new Order(_customer);
            _mouse = new Product("Mouse","Mouse","mouse.jpg",99M,10);
            _keyboard = new Product("Teclado","Teclado","Teclado.jpg",100M,10);
            _chair = new Product("Cadeira","Cadeira","Cadeira.jpg",500M,10);
            _monitor = new Product("Monitor","Monitor","Monitor.jpg",600M,10);

        }

        [TestMethod]
        public void ShouldCreateOrderWhenValid()
        {
            Assert.AreEqual(true, _order.IsValid);
        }

        [TestMethod]
        public void StatusShouldBeCreatedWhenOrderCreated()
        {
            Assert.AreEqual(EOrderStatus.Created, _order.Status);
        }

        [TestMethod]
        public void ShouldReturnTwoWhenAddedTwoValidItems()
        {
            _order.AddItem(_monitor, 5);
            _order.AddItem(_mouse, 5);
            Assert.AreEqual(2, _order.Items.Count);
        }

        [TestMethod]
        public void ShouldReturnFiveWhenAddedPurchasedFiveItem()
        {
            _order.AddItem(_monitor, 5);
            Assert.AreEqual(_mouse.QuantityOnHand, 5);
        }

        [TestMethod]
        public void ShouldReturnANumberWhenOrderPlaced()
        {
            _order.Place();
            Assert.AreNotEqual("", _order.Number);
        }

        [TestMethod]
        public void ShouldReturnPaidWhenOrderPaid()
        {
            _order.Pay();
            Assert.AreEqual(EOrderStatus.Paid, _order.Status);
        }

        [TestMethod]
        public void ShouldReturnTwoShippingsWhenPurchasedTenProducts()
        {
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);

            _order.Ship();

            Assert.AreEqual(2, _order.Deliveries.Count);
        }

        [TestMethod]
        public void StatusShouldBeCanceledWhenOrderCanceled()
        {
            _order.Cancel();
            Assert.AreEqual(EOrderStatus.Canceled, _order.Status);
        }

        [TestMethod]
        public void ShouldCancelShippingWhenOrderCanceled()
        {
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);
            _order.AddItem(_monitor, 1);
            _order.AddItem(_mouse, 1);

            _order.Ship();
            _order.Cancel();

            foreach(var x in _order.Deliveries)
            {
                Assert.AreEqual(EDeliveryStatus.Canceled, x.Status);
            }
        }
    }
}
