﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateREcetWorkVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
   


        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
