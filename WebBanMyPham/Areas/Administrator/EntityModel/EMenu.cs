﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class EMenu
    {
        public int ID { get; set; }

        [MaxLength(250)]
        [Required]
        public string Title { get; set; }

        [DisplayName("Alias")]
        public string Alias { get; set; }

        [DisplayName("Parent menu")]
        public int? ParentID { get; set; }

        [DisplayName("Type show")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select type show")]
        public int Type { get; set; }
        

        public int? Index { get; set; }

        public int Location { get; set; }

        public int Level { get; set; }
        

        [DisplayName("Meta Title")]
        [MaxLength]
        public string MetaTitle { get; set; }

        [DisplayName("Meta Description")]
        [MaxLength]
        public string MetaDescription { get; set; }

        public bool Status { get; set; }

        public string Description { get; set; }
    }
  
}