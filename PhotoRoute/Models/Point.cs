//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhotoRoute.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Point
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int JourneyId { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string file { get; set; }
    
        public virtual Journey Journey { get; set; }
    }
}
