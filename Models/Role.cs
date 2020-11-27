﻿using System;
using System.ComponentModel.DataAnnotations;

namespace bugzilla.Models
{
    public class Role
    {
        [Key] public Guid Id { get; set; }
        [Required] public string Name { get; set; }
    }
}