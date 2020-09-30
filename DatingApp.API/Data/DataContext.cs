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
        public DbSet<Like> Likes {get;set;}
        public DbSet<Message> Messages{get;set;}
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

            // introduce foreign key
            builer.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany( u => u.MessageSent)
                .OnDelete(DeleteBehavior.Restrict);
            //introduce foreign key
            builer.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany( u => u.MessageReceived)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}