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
        public async Task<List<List<Card>>> Get() => new List<List<Card>> {
            new List<Card> {new Card(1, CardColor.Red), new Card(2, CardColor.Red), new Card(3, CardColor.Red) },
            new List<Card> {new Card(6, CardColor.Red), new Card(6, CardColor.Blue), new Card(6, CardColor.Orange)}
        };
    }
}
