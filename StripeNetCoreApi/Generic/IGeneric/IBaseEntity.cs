using System;
using System.Collections.Generic;
using System.Text;

namespace StripeNetCoreApi.Generic.IGeneric
{
    public interface IBaseEntity
    {
        string DateCreated { get; set; }
        string? DateModified { get; set; }
        string? DateDeleted { get; set; }
    }
}
