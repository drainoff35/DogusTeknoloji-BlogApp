﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public List<Post>? Posts { get; set; }
    }
}
