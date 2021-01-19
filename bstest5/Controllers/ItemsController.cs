using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using bstest5.Models;

namespace bstest5.Controllers
{
    [Route("[controller]")]
    [ApiVersion("0")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly ConcurrentDictionary<int, Item> Items = new ConcurrentDictionary<int, Item>
        {
            [0] = new Item { Id = 0, Value = "value0" },
            [1] = new Item { Id = 1, Value = "value1" }
        };

        /// <summary>
        ///     Gets items
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get()
        {
            return Ok(Items.Values);
        }

        /// <summary>
        ///     Gets a item
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <response code="404">The item was not found</response>
        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {
            return Items.TryGetValue(id, out var item) ? (ActionResult<Item>)Ok(item) : NotFound();
        }

        /// <summary>
        ///     Adds a item
        /// </summary>
        /// <param name="item">Value</param>
        [HttpPost]
        public ActionResult<Item> Post([FromBody] Item item)
        {
            item.Id = Items.Keys.Max() + 1;
            Items[item.Id] = item;
            return Ok(item);
        }

        /// <summary>
        ///     Sets a item
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <param name="item">Value</param>
        [HttpPut("{id}")]
        public ActionResult<Item> Put(int id, [FromBody] Item item)
        {
            item.Id = id;
            Items[id] = item;
            return Ok(item);
        }

        /// <summary>
        ///     Deletes a item
        /// </summary>
        /// <param name="id">Id of the item.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Ok(Items.TryRemove(id, out _));
        }
    }
}
