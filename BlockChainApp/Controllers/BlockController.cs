using System.Text.Json;
using BlockChainApp.CustomIndexes.Abstractions;
using BlockchainLib.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockController : ControllerBase
    {
        private readonly ISimpleApp _app;
        private readonly ILogger<BlockController> _logger;
        private readonly IPropertyIndex _propertyIndex;
        private readonly IEncryptorService _encryptorService;

        public BlockController(
            ISimpleApp app,
            ILogger<BlockController> logger,
            IPropertyIndex propertyIndex, 
            IEncryptorService encryptorService)
        {
            _app = app;
            _logger = logger;
            _propertyIndex = propertyIndex;
            _encryptorService = encryptorService;
        }

        [HttpPost("registerProperty")]
        public MyActionResponse Register([FromBody] PropertyRegistrationModel registrationRequest)
        {
            try
            {
                var keys = new Keys(registrationRequest.FromKeys.PublicKey, registrationRequest.FromKeys.PrivateKey);
                _app.PerformTransaction(keys, registrationRequest.FromKeys.PublicKey, registrationRequest.Property);
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e.Message);
                return MyActionResponse.ErrorResponse(e.Message);
            }
        
            return MyActionResponse.OkResponse(null);
        }
        
        [HttpPost("transferProperty")]
        public MyActionResponse Transfer([FromBody] PropertyTransferModel transferRequest)
        {
            try
            {
                var keys = new Keys(transferRequest.FromKeys.PublicKey, transferRequest.FromKeys.PrivateKey);
                _app.PerformTransaction(keys, transferRequest.To, transferRequest.Property);
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e.Message);
                return MyActionResponse.ErrorResponse(e.Message);
            }
        
            return MyActionResponse.OkResponse(null);
        }
        
        [HttpPost("property/history")]
        public MyActionResponse PropertyHistory([FromBody]PropertyHistoryCheckModel model)
        {
            return MyActionResponse.OkResponse(_propertyIndex.GetByKey(model.PropertyHash));
        }
        
        [HttpGet("blockchainHistory/{amountOfBlocks}")]
        public MyActionResponse BlockchainHistory(int amountOfBlocks)
        {
            return MyActionResponse.OkResponse(_app.GetBlocks(amountOfBlocks));
        }
        
        [HttpPost("property/checkOwnership")]
        public MyActionResponse CheckOwnership([FromBody]PropertyOwnershipCheckModel model)
        {
            var block = _propertyIndex.GetByKey(model.PropertyHash).LastOrDefault();
            
            if (block is null)
                return MyActionResponse.ErrorResponse("Property was not found");
            
            var result =
                _encryptorService.VerifySignature(model.OwnerPublicKey, JsonSerializer.Serialize(block.TransactionPayload), block.Sign);
            return MyActionResponse.OkResponse(new { IsOwnershipConfirmed = result});
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

    public class PropertyOwnershipCheckModel
    {
        public string OwnerPublicKey { get; set; }
        public string PropertyHash { get; set; }
    }
}
