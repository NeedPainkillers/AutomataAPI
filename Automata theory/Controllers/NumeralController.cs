using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Automata_theory.Lib;
using Automata_theory.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Automata_theory.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NumeralController : ControllerBase
    {
        private readonly IHandler _handler;

        public NumeralController(IHandler handler)
        {
            _handler = handler;

        }

        // GET api/id
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public string Get()
        {
            return "Nothing";
        }

        // POST api/id
        [HttpPost]
        public Task<Number> Post([FromBody] string item) // return result 
        {
            return GetNumberAsync(item);
        }

        private async Task<Number> GetNumberAsync(string item)
        {
            await Task.Delay(10);
            return _handler.GetNumberRomanian(_handler.GetNumberDecimal(item));
        }
    }
}