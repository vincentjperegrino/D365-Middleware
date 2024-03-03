using System;
using KTI.Moo.Extensions.Core.Exception;

namespace KTI.Moo.Extensions.Lazada.Exception
{
    /// <summary>
    /// Lazada integration service exception
    /// </summary>
    public class LazadaIntegrationServiceException : ApiIntegrationServiceException
    {
        public LazadaIntegrationServiceException(string message) : base(message)
        { }
    }

    /// <summary>
    /// Lazop API response exception
    /// </summary>
    public class LazadaApiException : LazadaIntegrationServiceException
    {
        public LazadaApiException(string message) : base("Lazop response: " + message)
        { }
    }

    /// <summary>
    /// Lazop API response exception to be thrown when attempting to get a product that doesn't exist, or doesn't have the required matching value.
    /// </summary>
    public class NullProductException : LazadaApiException
    {
        public NullProductException(string sellerSku) : base($"Lazop response: Product with sellerSku {sellerSku} does not exist.")
        { }

        public NullProductException(long itemId) : base($"Lazop response: Product with ItemId {itemId} does not exist.")
        { }
    }

    /// <summary>
    /// Lazada integration service exception to be thrown when tokens have expired and cannot be refreshed.
    /// </summary>
    public class TokensExpiredException : LazadaIntegrationServiceException
    {
        public TokensExpiredException() : base("Refresh tokens have expired; cannot get new tokens. Seller has to re-authorize the app.")
        { }
    }

    /// <summary>
    /// Lazada integration service exception to be thrown when the requested client tokens cannot be fetched.
    /// </summary>
    public class NullTokensException : LazadaIntegrationServiceException
    {
        public NullTokensException() : base("No available tokens for specified seller.")
        { }

        public NullTokensException(string sellerId) : base($"No available tokens for seller with ID {sellerId}")
        { }
    }
}