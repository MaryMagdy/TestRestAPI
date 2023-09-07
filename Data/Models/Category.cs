﻿using System.ComponentModel.DataAnnotations;

namespace TestRestAPI.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string? Notes {  get; set; }

        public List<Item> Items {  get; set; }
    }
}
