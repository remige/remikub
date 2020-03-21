namespace remikub.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using remikub.Model;

    [Route("/api/v1/board")]
    public class BoardController : Controller
    {
        [HttpGet]
        public async Task<Card[][]> Get()
        {
            var combinaison1 = new Card[] { new Card(1, CardColor.Red), new Card(2, CardColor.Red), new Card(3, CardColor.Red) };
            var combinaison2 = new Card[] { new Card(6, CardColor.Red), new Card(6, CardColor.Blue), new Card(6, CardColor.Orange) };

            return new Card[][] { combinaison1, combinaison2 };
        }
    }
}
