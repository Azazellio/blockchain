using BlockChainApp.CustomIndexes.Abstractions;
using BlockchainLib.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockController : ControllerBase
    {
        private readonly SimpleApp _app;
        private readonly ILogger<BlockController> _logger;
        private readonly IPropertyIndex _propertyIndex;

        public BlockController(
            SimpleApp app,
            ILogger<BlockController> logger,
            IPropertyIndex propertyIndex)
        {
            _app = app;
            _logger = logger;
            _propertyIndex = propertyIndex;
        }

        [HttpPost("registerProperty")]
        public ActionResponse Register([FromBody] PropertyRegistrationModel registrationRequest)
        {
            try
            {
                var keys = new Keys(registrationRequest.FromKeys.PublicKey, registrationRequest.FromKeys.PrivateKey);
                _app.PerformTransaction(keys, registrationRequest.FromKeys.PublicKey, registrationRequest.Property);
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
                var keys = new Keys(transferRequest.FromKeys.PublicKey, transferRequest.FromKeys.PrivateKey);
                _app.PerformTransaction(keys, transferRequest.To, transferRequest.Property);
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e.Message);
                return ActionResponse.ErrorResponse(e.Message);
            }
        
            return ActionResponse.OkResponse(null);
        }
        
        [HttpPost("propertyHistory")]
        public ActionResponse PropertyHistory([FromBody]PropertyHistoryCheckModel model)
        {
            return ActionResponse.OkResponse(_propertyIndex.GetByKey(model.PropertyHash));
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
        public KeyPairDto FromKeys { get; set; }
        // public string To => FromKeys.PublicKey;
        public string Property { get; set; }
    }

    public class KeyPairDto
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public class PropertyHistoryCheckModel
    {
        public string PropertyHash { get; set; }
    }
}
