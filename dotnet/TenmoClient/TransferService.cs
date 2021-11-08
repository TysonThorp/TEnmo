using RestSharp;
using RestSharp.Authenticators;
using System;
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
            IRestResponse<Transaction> response = client.Post<Transaction>(request);
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

        public Transaction GetTransactionById(int id)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/{id}");
            request.AddUrlSegment("id", id);
            IRestResponse<Transaction> response = client.Get<Transaction>(request);

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
            IRestResponse<Transaction> response = client.Post<Transaction>(request);

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
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/past");
            
            IRestResponse<List<Transaction>> response = client.Get<List<Transaction>>(request);

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

        public List<Transaction> ViewPendingTransfer(int id)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/pending");
            request.AddUrlSegment("id", id);
            IRestResponse<List<Transaction>> response = client.Get<List<Transaction>>(request);

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
    }


}

