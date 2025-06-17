using FlashThunder.Defs;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashThunder.Managers
{
    /// <summary>
    /// Handles the loading and retrieving of tiles.
    /// </summary>
    public class TileManager
    {
        private const char DefaultName = ',';
        private Dictionary<char, TileDef> _cache;

        public TileManager()
        {
            _cache = [];
        }

        /// <summary>
        /// Indexer to get tile definition with shorter syntax.
        /// </summary>
        /// <param name="tileName"></param>
        /// <returns></returns>
        public TileDef this[char tileName]
        {
            get { return GetTileDefinition(tileName); }
        }

        /// <summary>
        /// Attempts to retrieve the texture by name.
        /// </summary>
        /// <param name="name">The name the texture should be mapped to.</param>
        /// <returns>The Texture2D object mapped to the name.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Throws if neither tex or default could be found.
        /// </exception>
        public TileDef GetTileDefinition(char tileName)
        {
            if (!_cache.TryGetValue(tileName, out TileDef tile))
            {
                Console.WriteLine(
                    $"[WARNING] Tile {tileName} wasn't found in the texture cache." +
                    $"Attempting to retrieve default instead.");

                // if we don't even have a default, throw an exception
                if (!_cache.TryGetValue(DefaultName, out TileDef defaultTile))
                {
                    throw new KeyNotFoundException(
                        $"No default tile was found to replacing missing tile {tileName}.");
                }

                // set the tex to default (we know that it does exist now)
                tile = defaultTile;
            }
            return tile;
        }

        /// <summary>
        /// Clears out everything.
        /// </summary>
        public TileManager Clear()
        {
            _cache.Clear();
            return this;
        }

        /// <summary>
        /// Load JSON file into the TileManager cache.
        /// </summary>
        /// <param name="filePath"></param>
        public TileManager LoadDefinitions(TexManager textures, string filePath)
        {
            _cache = DataLoader.LoadObject<Dictionary<char, TileDef>>(filePath);

            _cache.Values
                .ToList()
                .ForEach(tileDef => tileDef.Texture = textures[tileDef.TextureName]);

            return this;
        }
    }
}