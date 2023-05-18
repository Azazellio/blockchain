using BlockchainLib.Cryptography;
using ConsoleBlockChainApp;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockController : ControllerBase
    {
        private readonly SimpleApp _app;
        private readonly ILogger<BlockController> _logger;

        public BlockController(SimpleApp app, ILogger<BlockController> logger)
        {
            _app = app;
            _logger = logger;
        }

        [HttpPost("registerProperty")]
        public ActionResponse Register([FromBody] PropertyRegistrationModel registrationRequest)
        {
            try
            {
                var keys = new KeyPair(registrationRequest.FromKeys.PublicKey, registrationRequest.FromKeys.PrivateKey);
                _app.PerformTransaction(keys, registrationRequest.To, registrationRequest.Property);
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e.Message);
                return ActionResponse.ErrorResponse(e.Message);
            }
        
            return ActionResponse.OkResponse(null);
        }
        
        [HttpPost("transferProperty")]
        public ActionResponse Transfer([FromBody] PropertyTransferModel transferRequest)
        {
            try
            {
                var keys = new KeyPair(transferRequest.FromKeys.PublicKey, transferRequest.FromKeys.PrivateKey);
                _app.PerformTransaction(keys, transferRequest.To, transferRequest.Property);
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e.Message);
                return ActionResponse.ErrorResponse(e.Message);
            }
        
            return ActionResponse.OkResponse(null);
        }
    }

    public class PropertyTransferModel
    {
        public KeyPairDto FromKeys { get; init; }
        public string To { get; init; }
        public string Property { get; init; }
    }

    public class PropertyRegistrationModel 
    {
        public KeyPairDto FromKeys { get; init; }
        public string To => FromKeys.PublicKey;
        public string Property { get; init; }
    }

    public class KeyPairDto
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
