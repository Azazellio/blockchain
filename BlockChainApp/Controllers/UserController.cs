using ConsoleBlockChainApp;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SimpleApp _app;
        
        public UserController(SimpleApp app)
        {
            _app = app;
        }
        
        [HttpGet]
        [Route("new")]
        public ActionResponse New()
        {
            var keys = _app.GenerateKeys();
            return ActionResponse.OkResponse(new User(keys.PublicKey, keys.PrivateKey));
        }
    }

    public record User(string Public, string Private);
}
