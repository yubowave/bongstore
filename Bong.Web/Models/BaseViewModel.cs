using System;
using System.ComponentModel.DataAnnotations;

namespace Bong.Web.Models
{
    public abstract class BaseViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
    }
}