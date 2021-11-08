using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System.Collections.Generic;
using System;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
      
        private readonly ITransferDao _transferDao;
        public TransferController(ITransferDao transferDao)
        {
            _transferDao = transferDao;
          
        }




        [HttpGet("transactions/{transfer_id}")]

        public ActionResult<Transaction> GetTransactionById(int transfer_id)
        {
            Transaction transaction = _transferDao.GetTransactionById(transfer_id);

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

        public ActionResult<string> SendTeBucks(int userId_from, int userId_to, decimal amount)
        {
            int user_id = Convert.ToInt32(User.FindFirst("sub")?.Value);
            userId_from = user_id;

            string transaction1 = _transferDao.SendTEBucks(userId_from, userId_to, amount);

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

        public ActionResult<List<Transaction>> ViewPastTransfers(int id)
        {
            int user_id = Convert.ToInt32(User.FindFirst("sub")?.Value);

            List<Transaction> transfers = _transferDao.ViewPastTransfers(user_id);

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

        public ActionResult<List<Transaction>> ViewPendingTransfers(int userId)
        {
            int user_id = Convert.ToInt32(User.FindFirst("sub")?.Value);

            List<Transaction> transfers = _transferDao.PendingTransactions(user_id);

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
