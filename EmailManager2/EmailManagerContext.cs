using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EmailManager2.Models;

namespace EmailManager2
{
    class EmailManagerContext : DbContext
    {
        public EmailManagerContext() : base("default")
        { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
