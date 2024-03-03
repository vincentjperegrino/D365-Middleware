using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Model.ChannelManagement;

public class CustomFieldBase : CustomEntityBase
{
    public string kti_channelfieldname { get; set; }
    public string kti_crmfieldname { get; set; }
    public int kti_crmtable { get; set; }
    public string kti_customfieldmappingId { get; set; }
    public string kti_customfieldname { get; set; }
    public string kti_customvaluename { get; set; }
    public bool kti_hasparentfield { get; set; }
    public string kti_parentfieldname { get; set; }
    public bool kti_iscustomfieldwithnameandvalue { get; set; }
    public bool kti_isstatic { get; set; }
    public bool kti_isuniquekey { get; set; }
    public string kti_name { get; set; }
    public string kti_saleschannel { get; set; }
    public int kti_datatype { get; set; }
    public bool kti_saveoninsert { get; set; }
    public string kti_staticvalue { get; set; }
}
