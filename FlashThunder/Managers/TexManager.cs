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
    /// Handles the loading and retrieving of textures.
    /// </summary>
    public class TexManager
    {
        private const string DefaultName = "!default";
        private readonly Dictionary<string, Texture2D> _cache;
        private readonly ContentManager _contentManager;

        public TexManager(ContentManager cm, string defaultTex = null)
        {
            _cache = [];
            _contentManager = cm;

            if (defaultTex != null)
                RegisterDefault(defaultTex);
        }

        public Texture2D this[string name]
        {
            get { return Get(name); }
            set { Set(name, value); }
        }

        private bool TryLoad(string name, out Texture2D tex)
        {
            try
            {
                tex = _contentManager.Load<Texture2D>(name);
                return true;
            }
            catch (Exception ex)
            {
                tex = null;

                Console.WriteLine($"[WARNING] Failed to load texture {name}. " +
                    $"\nDetails: {ex.Message}");

                return false;
            }
        }

        /// <summary>
        /// Initialize a texture with the provided alias for future lookup.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TexManager Register(string alias, string name)
        {
            if (TryLoad(name, out Texture2D tex))
                _cache[alias] = tex;
            else
                Console.WriteLine($"[WARNING] Texture {name} failed to register!");

            return this;
        }

        /// <summary>
        /// Initialize a texture as the default texture in the cache.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TexManager RegisterDefault(string name)
            => Register(DefaultName, name);

        /// <summary>
        /// Directly pass a texture to be stored, foregoing the Register logic.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="tex"></param>
        /// <returns></returns>
        public TexManager Set(string alias, Texture2D tex)
        {
            _cache[alias] = tex;
            return this;
        }

        /// <summary>
        /// Directly set the default to a texture, foregoing the Register logic.
        /// </summary>
        /// <param name="tex"></param>
        /// <returns></returns>
        public TexManager SetDefault(Texture2D tex)
            => Set(DefaultName, tex);

        /// <summary>
        /// Attempts to retrieve the texture by name.
        /// </summary>
        /// <param name="name">The name the texture should be mapped to.</param>
        /// <returns>The Texture2D object mapped to the name.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Throws if neither tex or default could be found.
        /// </exception>
        public Texture2D Get(string name)
        {
            if (!_cache.TryGetValue(name, out Texture2D tex))
            {
                Console.WriteLine(
                    $"[WARNING] Texture {name} wasn't found in the texture cache." +
                    $"Attempting to retrieve default instead.");

                // if we don't even have a default, throw an exception
                if (!_cache.TryGetValue(DefaultName, out Texture2D defaultTile))
                {
                    throw new KeyNotFoundException(
                        $"No default texture was found to replacing missing texture {name}.");
                }

                // set the tex to default (we know that it does exist now)
                tex = defaultTile;
            }

            return tex;
        }

        /// <summary>
        /// Clears out everything.
        /// </summary>
        public TexManager Clear(bool clearDefault = false)
        {
            Texture2D temp = null;

            if (!clearDefault && _cache.TryGetValue(DefaultName, out Texture2D val))
            {
                temp = val;
            }

            _cache.Clear();

            if (temp != null)
            {
                _cache[DefaultName] = temp;
            }

            return this;
        }

        public TexManager LoadDefinitions(string path)
        {
            var defs = DataLoader.LoadObject<List<TextureDef>>(path);

            foreach (var def in defs)
            {
                Register(
                    def.TextureAlias ?? def.TextureName,
                    def.TextureName
                    );
            }

            return this;
        }
    }
}