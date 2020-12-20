using System.ComponentModel.DataAnnotations;

namespace SatisFact.Models
{
	public class Building
	{
		[Key]
		public int Id { get; set; }

		[Required, StringLength(100)]
		public string Title { get; set; }

		[StringLength(100)]
		public string ImageName { get; set; }
	}
}
