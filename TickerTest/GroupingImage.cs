//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TickerTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupingImage
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int TypeID { get; set; }
        public int OwnerID { get; set; }
        public bool Hidden { get; set; }
        public string Path { get; set; }
    
        public virtual ImageType ImageType { get; set; }
    }
}