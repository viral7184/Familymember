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
    
    public partial class tbl_relationship
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_relationship()
        {
            this.tbl_member = new HashSet<tbl_member>();
        }
    
        public int Id { get; set; }
        public string RELATIONSHIP { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public Nullable<int> LOGIN_BY { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_member> tbl_member { get; set; }
    }
}
