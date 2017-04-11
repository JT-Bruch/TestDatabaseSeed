using System;
using System.Collections.Generic;
using System.Text;

namespace TestDatabaseSeed.Models
{
  public partial class EntityTable
  {
    public int Id { get; set; }
    public int? Occur { get; set; }

    public static IEnumerable<EntityTable> EnsureSeedData()
    {
      var seedData = new List<EntityTable>
      {
        new EntityTable() {Id = 0, Occur = 1},
        new EntityTable() {Id = 682, Occur = 1},
        new EntityTable() {Id = 719, Occur = 1},
      };
      return seedData;
    }
  }
}
