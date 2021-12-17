using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.SignalR;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
      
        private readonly ITransferDao _transferDao;
        private readonly IAccountDao _accountDao;
        public TransferController(ITransferDao transferDao, IAccountDao accountDao)
        {
            _transferDao = transferDao;
            _accountDao = accountDao;
        }




        [HttpGet("transactions/{transfer_id}")]

        public ActionResult<Transactions> GetTransactionById(int transfer_id)
        {
            Transactions transaction = _transferDao.GetTransactionById(transfer_id);

            if (transaction != null)
            {
                return Ok(transaction);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("transactions/send")]

        public ActionResult<string> SendTeBucks()
        {
            int accountId = _accountDao.GetAccount(Convert.ToInt32(User.FindFirst("sub")?.Value)).AccountId;
            int userId = Convert.ToInt32(User.FindFirst("sub")?.Value);

            string transaction1 = _transferDao.SendTEBucks(accountId, userId);

            if (transaction1 != null)
            {
                return Ok(transaction1);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("transactions/request")]

        public ActionResult<string> RequestTeBucks(int userId_from, int userId_to, decimal amount)
        {
            int user_id = Convert.ToInt32(User.FindFirst("sub")?.Value);
            userId_to = user_id;

            string transaction2 = _transferDao.RequestTEBucks(userId_from, userId_to, amount);

            if (transaction2 != null)
            {
                return Ok(transaction2);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("transactions/past")]

        public ActionResult<List<PastTransfer>> ViewPastTransfers()
        {
            
           int accountId = _accountDao.GetAccount(Convert.ToInt32(User.FindFirst("sub")?.Value)).AccountId;
            List<PastTransfer> transfers = _transferDao.ViewPastTransfers(accountId);

            if (transfers != null)
            {
                return Ok(transfers);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("transactions/pending")]

        public ActionResult<List<PendingTransfer>> ViewPendingTransfers()
        {
            int accountId = _accountDao.GetAccount(Convert.ToInt32(User.FindFirst("sub")?.Value)).AccountId;

            List<PendingTransfer> transfers = _transferDao.PendingTransactions(accountId);

            if (transfers != null)
            {
                return Ok(transfers);
            }
            else
            {
                return NotFound();
            }

        }


    }
}
