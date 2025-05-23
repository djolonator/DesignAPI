﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DesignCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DesignCategoryId { get; set; }
        public string DesignCategoryName { get; set; }
        public List<Design> Designs { get; set; }
    }
}
