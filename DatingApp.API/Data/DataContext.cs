using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }
        public DbSet<Value>  Values { get; set; }
        public DbSet<User> Users {get;set;}
        
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likers {get;set;}
        protected override void OnModelCreating(ModelBuilder builer)
        {
            // introduce composite keys
            builer.Entity<Like>()
                .HasKey( k => new {k.LikerId, k.LikeeId});
            // introduce foreign key
            builer.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany( u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);
            //introduce foreign key
            builer.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany( u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}