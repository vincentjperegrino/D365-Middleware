using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;


namespace CRM_Plugin.Core.Helpers
{
    public static class MultiRequestHelper
    {

        public static ExecuteTransactionRequest MultipleAddRequest(this ExecuteTransactionRequest requestRecords, EntityCollection ForTransaction)
        {

            // Add a CreateRequest for each entity to the request collection.
            foreach (var entity in ForTransaction.Entities)
            {
                CreateRequest createRequest = new CreateRequest { Target = entity };
                requestRecords.Requests.Add(createRequest);
            }


            return requestRecords;

        }

        public static ExecuteTransactionRequest MultipleUpdateRequest(this ExecuteTransactionRequest requestRecords, EntityCollection ForTransaction)
        {
            // Add a CreateRequest for each entity to the request collection.
            foreach (var entity in ForTransaction.Entities)
            {
                UpdateRequest updateRequest = new UpdateRequest { Target = entity };
                requestRecords.Requests.Add(updateRequest);
            }

            return requestRecords;

        }




    }
}
