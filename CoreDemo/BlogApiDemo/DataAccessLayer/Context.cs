﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApiDemo.DataAccessLayer
{
    public class Context: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=CoreBlogApiDb;integrated security=true;"); //CoreBlogDb sqlde database adı.
        }
        public DbSet<Employee> Employees { get; set; }
        public IEnumerable<object> Users { get; set; }
    }








}
