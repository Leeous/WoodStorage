using System;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.Locations;
using StardewValley.GameData.Objects;
using xTile;

namespace WoodStorage
{
  public static class Constants
  {
    public const string WoodpileTileAction = "Leeous.Woodpile_Open";
  }
  /// <summary>Mod entry point</summary>
  internal sealed class ModEntry : Mod
  {
    public override void Entry(IModHelper helper)
    {
      // Edit tiles
      helper.Events.Content.AssetRequested += Content_AssetRequested;
      // Register interact hook
      GameLocation.RegisterTileAction(Constants.WoodpileTileAction, OnWoodpileOpen);
    }

    /// <summary>On Woodpile interact</summary>
    private bool OnWoodpileOpen(GameLocation where, string[] arg2, Farmer who, Point what)
    {
      this.Monitor.Log((this.Helper.Input.GetCursorPosition().Tile.ToString()), LogLevel.Trace);
      return true;
    }

    /// <summary>
    /// Replace woodpile tiles
    /// </summary>
    private void Content_AssetRequested(object? sender, AssetRequestedEventArgs e)
    {
      if (e.NameWithoutLocale.IsEquivalentTo("Data/Buildings"))
      {
        e.Edit(asset =>
        {
          var data = asset.AsDictionary<string, BuildingData>().Data;
          var farmhouse = data["Farmhouse"];
          
          // Draw empty wood pile over farmhouse tiles
          farmhouse.DrawLayers.Add(new BuildingDrawLayer()
          {
            Id = "Leeous.Woodpile_Empty",
            Texture = Helper.ModContent.GetInternalAssetName("assets/Woodpile_Empty.png").Name,
            SourceRect = new()
            {
              Width = 64,
              Height = 64,
            },
            SortTileOffset = (float)2,
            DrawInBackground = false,
            DrawPosition = new()
            {
              X = 0,
              Y = 66
            },
          });
          
          farmhouse.ActionTiles.Add(new()
          {
            Id = "Leeous.Woodpile_Open",
            Tile = new()
            {
              X = 2,
              Y = 2
            },
            Action = "Leeous.Woodpile_Open"
          });
        });
      }
    }
  }
}