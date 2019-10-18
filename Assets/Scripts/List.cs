using System.Collections.Generic;
using UnityEngine.UI;

internal class List : List<Image>
{
    public List(IEnumerable<Image> collection) : base(collection)
    {
    }
}