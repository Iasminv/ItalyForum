using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ItalyForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ItalyForum.Data
{
    public class ItalyForumContext : IdentityDbContext<ApplicationUser>
    {
        public ItalyForumContext (DbContextOptions<ItalyForumContext> options)
            : base(options)
        {
        }

        public DbSet<ItalyForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<ItalyForum.Models.Comment> Comment { get; set; } = default!;

    }
}
