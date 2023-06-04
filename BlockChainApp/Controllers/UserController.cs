using BlockChainApp.CustomIndexes.Abstractions;
using BlockChainApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISimpleApp _app;
        private readonly IUserIndexFrom _indexFrom;
        private readonly IUserIndexTo _indexTo;
        
        public UserController(
            ISimpleApp app,
            IUserIndexFrom indexFrom, 
            IUserIndexTo indexTo)
        {
            _app = app;
            _indexFrom = indexFrom;
            _indexTo = indexTo;
        }
        
        [HttpGet("new")]
        public MyActionResponse New()
        {
            var keys = _app.GenerateKeys();
            return MyActionResponse.OkResponse(new UserDto{ Public = keys.Public, Private = keys.Private });
        }
        
        [HttpGet("getUserHistory/{publicKey}")]
        public MyActionResponse GetHistory(string publicKey)
        {
            var fromHistory = _indexFrom.GetByKey(publicKey);
            var toHistory = _indexTo.GetByKey(publicKey);
            var wholeHistory = fromHistory.Concat(toHistory)
                .OrderBy(x => x.TransactionPayload.Time)
                .Select(x => 
                    new TransferBlockDto
                    {
                        From = x.TransactionPayload.From,
                        To = x.TransactionPayload.To,
                        Property = x.TransactionPayload.Property,
                        Time = x.TransactionPayload.Time,
                        Sign = x.Sign
                    });
            
            return MyActionResponse.OkResponse(wholeHistory);
        }
    }
}
