﻿using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class StoreroomDto
    {
        public List<String> Locations { get; set; }
        public List<OwnedGrocery> OwnedGroceries { get; set; }
    }
}