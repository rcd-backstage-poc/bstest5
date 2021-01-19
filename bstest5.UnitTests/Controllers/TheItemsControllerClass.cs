using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using bstest5.Controllers;
using bstest5.Models;

namespace bstest5.UnitTests.Controllers
{
    public class TheItemsControllerClass
    {
        public class TheGetMethod
        {
            [Fact]
            public void Should_return_all_items()
            {
                // Arrange
                var controller = new ItemsController();
                const int itemId = 4;
                const string itemValue = "foo";
                controller.Put(itemId, new Item { Value = itemValue });

                // Act
                var actionResult = controller.Get();

                // Assert
                var results = ( actionResult.Result as ObjectResult )?.Value as IEnumerable<Item>;
                var result = results?.FirstOrDefault(item => item.Id == itemId);

                Assert.NotNull(result);
                Assert.Equal(itemId, result.Id);
                Assert.Equal(itemValue, result.Value);
            }

            [Fact]
            public void Should_return_an_item()
            {
                // Arrange
                var controller = new ItemsController();
                const int itemId = 5;
                const string itemValue = "foo";
                controller.Put(itemId, new Item { Value = itemValue });

                // Act
                var actionResult = controller.Get(itemId);

                // Assert
                var result = ( actionResult.Result as ObjectResult )?.Value as Item;

                Assert.NotNull(result);
                Assert.Equal(itemId, result.Id);
                Assert.Equal(itemValue, result.Value);
            }

            [Fact]
            public void Should_return_not_found_when_item_does_not_exist()
            {
                // Arrange
                var controller = new ItemsController();

                // Act
                var actionResult = controller.Get(99);

                // Assert
                var result = actionResult.Result as StatusCodeResult;

                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        public class ThePostMethod
        {
            [Fact]
            public void Should_add_an_item()
            {
                // Arrange
                var controller = new ItemsController();
                var item = new Item
                {
                    Value = "foo"
                };

                // Act
                var actionResult = controller.Post(item);

                // Assert
                var result = ( actionResult.Result as ObjectResult )?.Value as Item;

                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id);
                Assert.Equal(item.Value, result.Value);
            }
        }

        public class ThePutMethod
        {
            [Fact]
            public void Should_set_an_item()
            {
                // Arrange
                var controller = new ItemsController();
                const int itemId = 6;
                var item = new Item
                {
                    Value = "foo"
                };

                // Act
                var actionResult = controller.Put(itemId, item);

                // Assert
                var result = ( actionResult.Result as ObjectResult )?.Value as Item;

                Assert.NotNull(result);
                Assert.Equal(itemId, result.Id);
                Assert.Equal(item.Value, result.Value);
            }
        }

        public class TheDeleteMethod
        {
            [Fact]
            public void Should_remove_an_item()
            {
                // Arrange
                var controller = new ItemsController();
                const int itemId = 7;
                controller.Put(itemId, new Item());

                // Act
                controller.Delete(itemId);

                // Assert
                var result = controller.Get(itemId).Result as StatusCodeResult;

                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }
    }
}
