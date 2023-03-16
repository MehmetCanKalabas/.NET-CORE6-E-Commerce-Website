﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proje.Models
{
	public class Setting
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }

		public string? Telephone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

		public string? Address { get; set; }
		public int MainPageCount { get; set; }
		public int SubPageCount { get; set; }
	}
}
