//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class STATUS
    {
        public STATUS()
        {
            this.CARDS = new HashSet<CARDS>();
        }
    
        public int STATUSID { get; set; }
        public string STATUSNAME { get; set; }
    
        public virtual ICollection<CARDS> CARDS { get; set; }
    }
}