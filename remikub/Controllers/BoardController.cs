namespace remikub.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using remikub.Model;

    [Route("/api/v1/board")]
    public class BoardController : Controller
    {
        [HttpGet]
        public async Task<List<Card>> Get() => new List<Card> {
            new Card(1, CardColor.Red, new Coordinates(0, 0)), new Card(2, CardColor.Red, new Coordinates(0, 1)), new Card(3, CardColor.Red, new Coordinates(0, 2)),
            new Card(6, CardColor.Red, new Coordinates(1, 0)), new Card(6, CardColor.Blue, new Coordinates(1, 1)), new Card(6, CardColor.Orange, new Coordinates(1, 2))
        };
    }
}
