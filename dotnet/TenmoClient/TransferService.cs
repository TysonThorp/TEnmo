using RestSharp;
using RestSharp.Authenticators;
using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Transactions;
=======
>>>>>>> 7481a4d6f0c95ae13218abfd76a83c5972cc15b3
using TenmoClient.Exceptions;
using TenmoClient.Models;

using System.Transactions;

using System.Collections.Generic;

namespace TenmoClient
{
    public class TransferService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly RestClient client = new RestClient();

        //login endpoints
        
        public string SendTEBucks()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/send");
            IRestResponse<string> response = client.Post<string>(request);
            UserService.GetToken();

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.StatusCode.ToString();
            }

            return null;
        }
        public List<Transaction> ViewPastTransfers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transactions");
            IRestResponse<Transaction> response = client.Post<Transaction>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return (List<Transaction>)response;
            }

            return null;
        }


<<<<<<< HEAD
=======
        public Transactions GetTransactionById(int id)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/{id}");
            request.AddUrlSegment("id", id);
            IRestResponse<Transactions> response = client.Get<Transactions>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return null;
        }

        public string RequestTeBucks()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/request");
            IRestResponse<Transactions> response = client.Post<Transactions>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.StatusCode.ToString();
            }

            return null;
        }

        public List<PastTransfer> ViewPastTransfers()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/past");
            
            IRestResponse<List<PastTransfer>> response = client.Get<List<PastTransfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return null;
        }

        public List<PendingTransfer> ViewPendingTransfer()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/pending");
            
            IRestResponse<List<PendingTransfer>> response = client.Get<List<PendingTransfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return null;
        }

>>>>>>> 7481a4d6f0c95ae13218abfd76a83c5972cc15b3
        private void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new NoResponseException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new NonSuccessException((int)response.StatusCode);
            }
        }
        public List<Transaction> PendingTransactions()
        {
            List<Transaction> pendingTransactions = new List<Transaction>();

            return pendingTransactions;

        }
    }


}

