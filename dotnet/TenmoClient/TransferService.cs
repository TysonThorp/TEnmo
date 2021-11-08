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
            IRestResponse<Transactions> response = client.Post<Transactions>(request);
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

        public List<Transactions> ViewPastTransfers()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/past");
            
            IRestResponse<List<Transactions>> response = client.Get<List<Transactions>>(request);

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

        public List<Transactions> ViewPendingTransfer(int id)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/pending");
            request.AddUrlSegment("id", id);
            IRestResponse<List<Transactions>> response = client.Get<List<Transactions>>(request);

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

