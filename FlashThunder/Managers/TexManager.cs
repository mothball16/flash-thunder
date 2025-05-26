using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Managers
{
    /// <summary>
    /// Handles the loading and retrieving of textures.
    /// </summary>
    public class TexManager
    {
        private const string DefaultName = "!default";
        private Dictionary<string, Texture2D> _cache;
        private ContentManager _contentManager;

        public TexManager(ContentManager cm, string defaultTex = null)
        {
            _cache = [];
            _contentManager = cm;
            if(defaultTex != null)
                Register(DefaultName, defaultTex);
        }

        //Indexer to make things easier
        public Texture2D this[string name]
        {
            get
            {
                return Get(name);
            }
        }

        public TexManager Register(string name)
            => Register(name, name);

        public TexManager Register(string alias, string name)
        {
            try
            {
                Texture2D tex = _contentManager.Load<Texture2D>(name);
                _cache[alias] = tex;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Failed to load texture {name}. " +
                    $"No texture has been loaded for alias {alias}.\nDetails: {ex.Message}");
                throw;
            }
            return this;
        }


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
            if(!_cache.TryGetValue(name, out Texture2D tex))
            {
                Console.WriteLine(
                    $"[WARNING] Texture {name} wasn't found in the texture cache." +
                    $"Attempting to retrieve default instead.");

                //if we don't even have a default, throw an exception
                if (!_cache.TryGetValue(DefaultName, out Texture2D defaultTile))
                {
                    throw new KeyNotFoundException(
                        $"No default texture was found to replacing missing texture {name}.");   
                }

                //set the tex to default (we know that it does exist now)
                tex = defaultTile;
            }
            return tex;
        }

        /// <summary>
        /// Clears all caches.
        /// </summary>
        public void Clear(bool clearDefault = false)
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
        }
    }
}
