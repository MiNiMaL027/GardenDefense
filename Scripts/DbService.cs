using System;
using Enums;
using Godot;
using Items;
using Microsoft.Data.Sqlite;

public static class DbService
{
    public static ItemDatabaseRow GetItem(int id)
    {
        using (SqliteConnection con = new SqliteConnection("Data Source = Db.db"))
        {
            con.Open();
            SqliteCommand sqliteCommand = con.CreateCommand();
            sqliteCommand.CommandText = $"SELECT * FROM Items WHERE Id = {id}";
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    ItemType itemType = (ItemType)Convert.ToInt32(reader["ItemType"]);
                    switch (itemType)
                    {
                        case ItemType.Misc:
                            ItemDatabaseRow i = new ItemDatabaseRow();
                            i.Id = Convert.ToInt32(reader["Id"]);
                            i.ItemName = Convert.ToString(reader["ItemName"]);
                            i.Description = Convert.ToString(reader["Description"]);
                            i.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            i.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            i.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            i.ItemType = itemType;
                            i.MeshPath = Convert.ToString(reader["MeshPath"]);
                            return i;
                        case ItemType.Seed:
                            SeedDatabaseRow seed = new SeedDatabaseRow();
                            seed.Id = Convert.ToInt32(reader["Id"]);
                            seed.ItemName = Convert.ToString(reader["ItemName"]);
                            seed.Description = Convert.ToString(reader["Description"]);
                            seed.BuyPrice = Convert.ToInt32(reader["BuyPrice"]);
                            seed.SellPrice = Convert.ToInt32(reader["SellPrice"]);
                            seed.TextureSpritePath = Convert.ToString(reader["TextureSpritePath"]);
                            seed.ItemType = itemType;
                            seed.MeshPath = Convert.ToString(reader["MeshPath"]);
                            seed.SeedType = (SeedType)Convert.ToInt32(reader["Param1"]);
                            seed.StagesAmount = Convert.ToInt32(reader["Param2"]);
                            seed.MinSecondsToChangeState = Convert.ToInt32(reader["Param3"]);
                            seed.MaxSecondsToChangeState = Convert.ToInt32(reader["Param4"]);
                            seed.GrowUpId = Convert.ToInt32(reader["Param5"]);
                            return seed;
                    }
                }
            }
        }
        return null;
    }
}
