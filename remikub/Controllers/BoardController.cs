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
            new Card(1, CardColor.Red, 0, 0), new Card(2, CardColor.Red,0, 1), new Card(3, CardColor.Red, 0, 2),
            new Card(6, CardColor.Red, 1, 0), new Card(6, CardColor.Blue, 1, 1), new Card(6, CardColor.Orange, 1, 2)
        };
    }
}
