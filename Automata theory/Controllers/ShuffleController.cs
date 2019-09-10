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
    public class ShuffleController : ControllerBase
    {
        private readonly IHandler _handler;

        public ShuffleController(IHandler handler)
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
        public Task<ChessLine> Post([FromBody] ChessLine item) // return result 
        {
            return ShuffleLinesAsync(item);
        }

        private async Task<ChessLine> ShuffleLinesAsync(ChessLine item)
        {
            await Task.Delay(10);
            return _handler.GetShuffledLine(item);
        }

    }
}