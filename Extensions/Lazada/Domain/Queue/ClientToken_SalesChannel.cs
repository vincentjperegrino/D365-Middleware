using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Domain.Queue;
using KTI.Moo.Extensions.Lazada.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Domain.Queue;

public class ClientToken_SalesChannel<T, R> : Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<T, R> where T : Core.Model.ClientTokensBase where R : Base.Model.ChannelManagement.SalesChannelBase
{

    private readonly Core.Domain.IClientToken_Oauth2<T> _ClientToken;
    private readonly Base.Domain.IChannelManagement<R> _channelManagementDomain;

    public ClientToken_SalesChannel(IClientToken_Oauth2<T> clientToken, Base.Domain.IChannelManagement<R> channelManagementDomain)
    {
        _ClientToken = clientToken;
        _channelManagementDomain = channelManagementDomain;
    }

    public int CompanyID
    {
        get => _channelManagementDomain.CompanyID;
        set => _channelManagementDomain.CompanyID = value;
    }

    /// <summary>
    /// Use if company Id is already initialize in channelManagement Domain
    /// </summary>
    /// <param name="authorization"></param>
    /// <returns></returns>
    public T Create(string authorization)
    {
        var Token = _ClientToken.Create(authorization);
        return Token;
    }

    public T Refresh(T clientToken)
    {
        var Token = _ClientToken.Refresh(clientToken);

        return Token;
    }

    public T Refresh(T clientToken, R ChannelConfig)
    {
        var Token = _ClientToken.Refresh(clientToken);

        if (Token.AccessToken != ChannelConfig.kti_access_token)
        {
            UpdateToken(Token, ChannelConfig);
        }

        return Token;

    }

    public bool UpdateToken(T Token, R ChannelConfig)
    {
        if (CompanyID == 0)
        {
            throw new SystemException("ClientToken_SalesChannel Company ID not set");
        }

        ChannelConfig.kti_access_token = Token.AccessToken;
        ChannelConfig.kti_refresh_token = Token.RefreshToken;
        ChannelConfig.kti_access_expiration = Token.AccessExpiration;
        ChannelConfig.kti_refresh_expiration = (DateTime)Token.RefreshExpiration;

        _channelManagementDomain.UpdateToken(ChannelConfig);
        return true;
    }
    public R GetToken(string sellerid)
    {
        return _channelManagementDomain.GetbyLazadaSellerID(sellerid);
    }

    public R Get(string StoreCode)
    {
        return _channelManagementDomain.Get(StoreCode);
    }

    public R GetbyLazadaSellerID(string SellerID)
    {
        return _channelManagementDomain.GetbyLazadaSellerID(SellerID);
    }

    public bool UpdateToken(R ChannelConfig)
    {
        return _channelManagementDomain.UpdateToken(ChannelConfig);
    }

    public List<R> GetChannelList()
    {
        return _channelManagementDomain.GetChannelList();
    }

 
}
