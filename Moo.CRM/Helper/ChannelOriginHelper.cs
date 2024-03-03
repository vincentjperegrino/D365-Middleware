using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Helper;

public class ChannelOrigin
{
    public static readonly string Queuename_lazada = "lazada";
    public static readonly string Queuename_magento = "magento";
    public static readonly string Queuename_octopos = "octopos";
    public static readonly string Queuename_sap = "sap";
    public static readonly string Queuename_shopee = "shopee";

    public static readonly int OptionSet_lazada = 959_080_006;
    public static readonly int OptionSet_magento = 959_080_010;
    public static readonly int OptionSet_octopos = 959_080_011;
    public static readonly int OptionSet_sap = 959_080_009;
    public static readonly int OptionSet_shopee = 959_080_007;



    public static string getquename(int channelOrigin)
    {
        if (OptionSet_lazada == channelOrigin)
        {
            return Queuename_lazada;
        }

        if (OptionSet_magento == channelOrigin)
        {
            return Queuename_magento;
        }

        if (OptionSet_octopos == channelOrigin)
        {
            return Queuename_octopos;
        }

        if (OptionSet_sap == channelOrigin)
        {
            return Queuename_sap;
        }

        if (OptionSet_shopee == channelOrigin)
        {
            return Queuename_shopee;
        }

        return string.Empty;
    }


    public static int getOptionSetValue(string queuename)
    {

        if (Queuename_lazada == queuename)
        {
            return OptionSet_lazada;
        }

        if (Queuename_magento == queuename)
        {
            return OptionSet_magento;
        }

        if (Queuename_octopos == queuename)
        {
            return OptionSet_octopos;
        }

        if (Queuename_sap == queuename)
        {
            return OptionSet_sap;
        }

        if (Queuename_shopee == queuename)
        {
            return OptionSet_shopee;
        }


        return default;
    }


}
