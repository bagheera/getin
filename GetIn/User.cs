using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
 public class User{
     public virtual int Id { get; set; }
     public virtual Profile Profile { get; set; }
    }
}
