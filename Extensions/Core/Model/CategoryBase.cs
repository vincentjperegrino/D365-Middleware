using System;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Core.Model
{
    public class CategoryBase
    {
        protected int _id;
        protected string _name;
        protected int? _crmId;

        [Required]
        public virtual int CategoryId
        {
            get { return this._id; }
            set
            {
                if (value > 0)
                    this._id = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Category ID must be greater than zero.");
            }
        }

        [Required(AllowEmptyStrings = false)]
        public virtual string Name
        {
            get { return this._name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this._name = value;
                else
                    throw new ArgumentNullException(nameof(value), "Name must not be empty.");
            }
        }

        public virtual int? CrmId
        {
            get { return this._crmId; }
            set
            {
                if (value.HasValue && value.Value > 0)
                    this._crmId = value.Value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "CRM ID must be greater than zero.");
            }
        }
    }
}