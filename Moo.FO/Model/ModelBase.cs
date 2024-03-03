namespace Moo.FO.Model
{
    public class ModelBase
    {
        public virtual DateTime modifiedDateTime { get; set; }
        public virtual string modifiedBy { get; set; }
        public virtual DateTime createdDateTime { get; set; }
        public virtual string dataAreaId { get; set; }
        public virtual int recversion { get; set; }
        public virtual int Partition { get; set; }
        public virtual int RecId { get; set; }
    }
}
