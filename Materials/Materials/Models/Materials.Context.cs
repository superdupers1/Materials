//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Materials.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MaterialsEntities : DbContext
    {
        public MaterialsEntities()
            : base("name=MaterialsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Buildings> Buildings { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<PartNumbers> PartNumbers { get; set; }
    }
}
