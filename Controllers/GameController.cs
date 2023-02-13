using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGetGameInfo _gameInfo;
        public GameController(IGetGameInfo gameInfo)
        {
            _gameInfo = gameInfo;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(string id)
        {
            var game = await _gameInfo.GetCollection(id);
            return Ok(game);
        }

    }
}
