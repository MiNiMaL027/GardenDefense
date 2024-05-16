using BinarySerialization;
using Enums;
using System.Collections.Generic;

namespace SaveModels
{
    public class AvailableItemsSave
    {
        [FieldOrder(0)]
        public int KeysLength { get; set; }

        [FieldOrder(1)]
        [FieldLength(nameof(KeysLength))]
        public List<ItemType> Keys { get; set; }

        [FieldOrder(2)]
        public int ValuesLengthsLength { get; set; }

        [FieldOrder(3)]
        [FieldLength(nameof(ValuesLengthsLength))]
        public List<int> ValuesLengths { get; set; } // Lengths of each inner list

        [FieldOrder(4)]
        public int ValuesLength { get; set; } // Total length of all values combined

        [FieldOrder(5)]
        [FieldLength(nameof(ValuesLength))]
        public List<int> Values { get; set; } // Flattened list of all values

        public AvailableItemsSave()
        {
            Keys = new List<ItemType>();
            ValuesLengths = new List<int>();
            Values = new List<int>();
        }

        public AvailableItemsSave(Dictionary<ItemType, List<int>> dictionary)
        {
            Keys = new List<ItemType>();
            ValuesLengths = new List<int>();
            Values = new List<int>();

            foreach (var kvp in dictionary)
            {
                Keys.Add(kvp.Key);
                ValuesLengths.Add(kvp.Value.Count);
                Values.AddRange(kvp.Value);
            }

            KeysLength = Keys.Count;
            ValuesLengthsLength = ValuesLengths.Count;
            ValuesLength = Values.Count;
        }

        public Dictionary<ItemType, List<int>> ToDictionary()
        {
            var dictionary = new Dictionary<ItemType, List<int>>();
            int valueIndex = 0;

            for (int i = 0; i < Keys.Count; i++)
            {
                var key = Keys[i];
                var length = ValuesLengths[i];
                var values = Values.GetRange(valueIndex, length);
                valueIndex += length;
                dictionary[key] = values;
            }

            return dictionary;
        }
    }
}
