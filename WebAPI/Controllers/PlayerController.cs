using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataAccess;
using Newtonsoft;
using Newtonsoft.Json;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("player")]
    public class PlayerController : Controller
    {
        public IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        [Route("{name}", Name = "GetPlayer")]
        public async Task<IActionResult> GetPlayer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }
            try
            {
                var result = await _playerService.GetPlayerByName(name);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SavePlayer(string name)
        {
            var result = await _playerService.AddNewPlayer(name);
            return CreatedAtRoute(nameof(GetPlayer), new {result.Name}, result);
        }
        
        [HttpGet]
        [Route("score/{id}", Name = "GetPlayerScoresById")]
        public async Task<IActionResult> GetPlayerScoresById(int id)
        {
            var result = await _playerService.GetPlayerScores(id); 
            return Ok(result);
        }
        
        [HttpPost]
        [Route("score")]
        public async Task<IActionResult> AddPlayerScore([FromBody]PlayerScore playerScore)
        {
            var result = await _playerService.AddPlayerScore(playerScore);
            if (result != null)
                return CreatedAtRoute(nameof(GetPlayerScoresById), new{id=playerScore.PlayerId}, playerScore);
            return NotFound();
        }
    }
}