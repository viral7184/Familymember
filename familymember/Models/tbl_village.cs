//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace familymember.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_village
    {
        public int Id { get; set; }
        public Nullable<int> MEMBER_ID { get; set; }
        public string VILLAGE { get; set; }
        public string SUBDISTRICT { get; set; }
        public string DISTRICT { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public Nullable<int> LOGIN_BY { get; set; }
    
        public virtual tbl_member tbl_member { get; set; }
    }
}
