using System;

namespace Deltadust.ItemManagement {
    public class Item {
        public int ID { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public ItemType Type { get; set; } 

        public Item(int id, string name, string description, ItemType type, int quantity = 1) {
            ID = id;
            Name = name;
            Description = description;
            Type = type;
            Quantity = quantity;
        }

        public bool UseItem(int amount) {
            if (Quantity >= amount) {
                Quantity -= amount;
                return true;
            }
            return false;
        }

        public override string ToString() {
            return $"{Name} (ID: {ID}, Quantity: {Quantity}, Type: {Type})";
        }

        public static Item operator +(Item item1, Item item2) {
            if (item1.ID == item2.ID && item1.Type == item2.Type) {
                return new Item(item1.ID, item1.Name, item1.Description, item1.Type, item1.Quantity + item2.Quantity);
            }
            throw new InvalidOperationException("Cannot combine items with different IDs or types.");
        }
    }

    public enum ItemType {
        Unknown = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3,
        Quest = 4,
        Misc = 5
    }
}
